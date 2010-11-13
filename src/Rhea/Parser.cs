using System.Collections.Generic;

namespace Rhea
{
    public class Parser
    {
        private Lexer mLexer;
        private TokenType mHeadToken;
        
        private void LookAhead()
        {
            if (mLexer.Advance())
            {
                mHeadToken = mLexer.Token;
            }
            else
            {
                mHeadToken = TokenType.EOF;
            }
        }
        
        public Parser(Lexer lexer)
        {
            mLexer = lexer;
            LookAhead();
        }
        
        public IExpr Parse()
        {
            if (mHeadToken == TokenType.EOF)
            {
                return null;
            }
            return ParseStatement();
        }
        
        private IExpr ParseStatement()
        {
            IExpr expr = ParseExpression();
            switch (mHeadToken)
            {
            case TokenType.EOF:
                break;
            case TokenType.Semicolon:
            case TokenType.NewLine:
                LookAhead();
                break;
            default:
                throw new RheaException(
                    Expected("NewLine or Semicolon"), mLexer.GetSourceInfo()
                );
            }
            return expr;
        }
        
        private IExpr ParseExpression()
        {
            return ParseDefinition();
        }
        
        private IExpr ParseDefinition()
        {
            if (mHeadToken == TokenType.Def)
            {
                LookAhead();
                if (mHeadToken != TokenType.Identifier)
                {
                    throw new RheaException(
                        Expected("Identifier"), mLexer.GetSourceInfo()
                    );
                }
                ValueSymbol symbol = ValueSymbol.Intern((string)mLexer.Value);
                SourceInfo info = mLexer.GetSourceInfo();
                LookAhead();
                if (mHeadToken == TokenType.Colon)
                {
                    return ParseMethodDefinition(new ExprGetVar(symbol, info));
                }
                return ParseVariableDefinition(symbol);
            }
            return ParseAssignment();
        }
        
        private IExpr ParseVariableDefinition(ValueSymbol symbol)
        {
            if (mHeadToken != TokenType.Equal)
            {
                throw new RheaException(
                    Expected("Equal"), mLexer.GetSourceInfo()
                );
            }
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            return new ExprDefVar(symbol, ParseExpression(), info);
        }
        
        private IExpr ParseMethodDefinition(IExpr klassExpr)
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new RheaException(
                    Expected("Identifier"), mLexer.GetSourceInfo()
                );
            }
            ValueSymbol selector = ValueSymbol.Intern((string)mLexer.Value);
            LookAhead();
            if (mHeadToken != TokenType.Equal)
            {
                throw new RheaException(
                    Expected("Equal"), mLexer.GetSourceInfo()
                );
            }
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            return new ExprDefMethod(klassExpr, selector, ParseExpression(), info);
        }
        
        private IExpr ParseAssignment()
        {
            IExpr expr = ParseSimpleExpression();
            if (mHeadToken == TokenType.Equal)
            {
                SourceInfo info = mLexer.GetSourceInfo();
                if (expr is ExprGetVar)
                {
                    ExprGetVar variableRef = (ExprGetVar)expr;
                    LookAhead();
                    return new ExprSetVar(
                        variableRef.Symbol,
                        ParseExpression(),
                        info
                    );
                }
                else if (expr is ExprGetMethod)
                {
                    ExprGetMethod methodRef = (ExprGetMethod)expr;
                    LookAhead();
                    return new ExprSetMethod(
                        methodRef.KlassExpr,
                        methodRef.Selector,
                        ParseExpression(),
                        info
                    );
                }
                throw new RheaException(Unexpected(), mLexer.GetSourceInfo());
            }
            return expr;
        }
        
        private IExpr ParseSimpleExpression()
        {
            return ParseEqualityExpression();
        }
        
        private IExpr ParseEqualityExpression()
        {
            IExpr expr = ParseRelationalExpression();
            if (mHeadToken == TokenType.DoubleEqual)
            {
                SourceInfo info = mLexer.GetSourceInfo();
                ValueSymbol selector = null;
                switch (mHeadToken)
                {
                case TokenType.DoubleEqual:
                    selector = ValueSymbol.Intern("__eq__");
                    break;
                }
                LookAhead();
                IList<IExpr> argExprs = new List<IExpr> {
                    ParseRelationalExpression()
                };
                expr = new ExprSend(expr, selector, argExprs, info);
            }
            return expr;
        }
        
        private IExpr ParseRelationalExpression()
        {
            return ParseAdditiveExpression();
        }
        
        private IExpr ParseAdditiveExpression()
        {
            IExpr expr = ParseMultiplicativeExpression();
            while (mHeadToken == TokenType.Plus ||
                   mHeadToken == TokenType.Minus)
            {
                SourceInfo info = mLexer.GetSourceInfo();
                ValueSymbol selector = null;
                switch (mHeadToken)
                {
                case TokenType.Plus:
                    selector = ValueSymbol.Intern("__add__");
                    break;
                case TokenType.Minus:
                    selector = ValueSymbol.Intern("__sub__");
                    break;
                }
                LookAhead();
                IList<IExpr> argExprs = new List<IExpr> {
                    ParseMultiplicativeExpression()
                };
                expr = new ExprSend(expr, selector, argExprs, info);
            }
            return expr;
        }
        
        private IExpr ParseMultiplicativeExpression()
        {
            IExpr expr = ParseUnaryExpression();
            while (mHeadToken == TokenType.Asterisk)
            {
                SourceInfo info = mLexer.GetSourceInfo();
                ValueSymbol selector = null;
                switch (mHeadToken)
                {
                case TokenType.Asterisk:
                    selector = ValueSymbol.Intern("__mul__");
                    break;
                }
                LookAhead();
                IList<IExpr> argExprs = new List<IExpr> {
                    ParseUnaryExpression()
                };
                expr = new ExprSend(expr, selector, argExprs, info);
            }
            return expr;
        }
        
        private IExpr ParseUnaryExpression()
        {
            return ParsePrimary();
        }
        
        private IExpr ParsePrimary()
        {
            IExpr expr = ParseAtom();
            while (mHeadToken == TokenType.LeftParen ||
                   mHeadToken == TokenType.Hat ||
                   mHeadToken == TokenType.BeginBlock ||
                   mHeadToken == TokenType.LeftBrace ||
                   mHeadToken == TokenType.Identifier ||
                   mHeadToken == TokenType.Dot)
            {
                switch (mHeadToken)
                {
                case TokenType.LeftParen:
                case TokenType.Hat:
                case TokenType.BeginBlock:
                case TokenType.LeftBrace:
                case TokenType.Identifier:
                    expr = ParseFunctionCall(expr);
                    break;
                case TokenType.Dot:
                    expr = ParseMessageSend(expr);
                    break;
                }
            }
            return expr;
        }
        
        private IExpr ParseFunctionCall(IExpr funcExpr)
        {
            SourceInfo info = mLexer.GetSourceInfo();
            return new ExprCall(funcExpr, ParseArguments(), info);
        }
        
        private IExpr ParseMessageSend(IExpr recvExpr)
        {
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new RheaException(
                    Expected("Identifier"), mLexer.GetSourceInfo()
                );
            }
            ValueSymbol selector = ValueSymbol.Intern((string)mLexer.Value);
            LookAhead();
            return new ExprSend(recvExpr, selector, ParseArguments(), info);
        }
        
        private IList<IExpr> ParseArguments()
        {
            List<IExpr> argExprs = new List<IExpr>();
            argExprs.AddRange(ParseBlockArguments());
            while (mHeadToken == TokenType.Identifier)
            {
                LookAhead();
                argExprs.AddRange(ParseBlockArguments());
            }
            if (mHeadToken == TokenType.End)
            {
                LookAhead();
            }
            return argExprs;
        }
        
        private IList<IExpr> ParseBlockArguments()
        {
            List<IExpr> argExprs = new List<IExpr>();
            if (mHeadToken == TokenType.LeftParen)
            {
                argExprs.AddRange(ParseParenthesizedArguments());
            }
            switch (mHeadToken)
            {
            case TokenType.Hat:
                argExprs.Add(ParseLambdaExpression());
                break;
            case TokenType.BeginBlock:
            case TokenType.LeftBrace:
                argExprs.Add(ParseLambdaExpressionOmittingParameters());
                break;
            }
            return argExprs;
        }
        
        private IList<IExpr> ParseParenthesizedArguments()
        {
            List<IExpr> argExprs = new List<IExpr>();
            LookAhead();
            if (mHeadToken != TokenType.RightParen)
            {
                argExprs.Add(ParseExpression());
                while (mHeadToken == TokenType.Comma)
                {
                    LookAhead();
                    argExprs.Add(ParseExpression());
                }
                if (mHeadToken != TokenType.RightParen)
                {
                    throw new RheaException(
                        Expected("RightParen"), mLexer.GetSourceInfo()
                    );
                }
            }
            LookAhead();
            return argExprs;
        }
        
        private IExpr ParseAtom()
        {
            IExpr expr;
            switch (mHeadToken)
            {
            case TokenType.Int:
                expr = ParseInt();
                break;
            case TokenType.String:
                expr = ParseString();
                break;
            case TokenType.Identifier:
                expr = ParseReference();
                break;
            case TokenType.Colon:
                expr = ParseSymbolLiteral();
                break;
            case TokenType.Hat:
                expr = ParseLambdaExpression();
                if (mHeadToken == TokenType.End)
                {
                    LookAhead();
                }
                break;
            case TokenType.LeftParen:
                expr = ParseParenthesizedExpression();
                break;
            default:
                throw new RheaException(Unexpected(), mLexer.GetSourceInfo());
            }
            return expr;
        }
        
        private IExpr ParseInt()
        {
            IExpr expr = new ExprConst(new ValueInt((int)mLexer.Value));
            LookAhead();
            return expr;
        }
        
        private IExpr ParseString()
        {
            IExpr expr = new ExprConst(new ValueString((string)mLexer.Value));
            LookAhead();
            return expr;
        }
        
        private IExpr ParseReference()
        {
            ValueSymbol symbol = ValueSymbol.Intern((string)mLexer.Value);
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            if (mHeadToken == TokenType.Colon)
            {
                return ParseMethodReference(new ExprGetVar(symbol, info));
            }
            return ParseVariableReference(symbol, info);
        }
        
        private IExpr ParseMethodReference(IExpr klassExpr)
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new RheaException(
                    Expected("Identifier"), mLexer.GetSourceInfo()
                );
            }
            ValueSymbol selector = ValueSymbol.Intern((string)mLexer.Value);
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            return new ExprGetMethod(klassExpr, selector, info);
        }
        
        private IExpr ParseVariableReference(ValueSymbol symbol, SourceInfo info)
        {
            return new ExprGetVar(symbol, info);
        }
        
        private IExpr ParseSymbolLiteral()
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new RheaException(
                    Expected("Identifier"), mLexer.GetSourceInfo()
                );
            }
            IExpr expr = new ExprConst(
                ValueSymbol.Intern((string)mLexer.Value)
            );
            LookAhead();
            return expr;
        }
        
        private IExpr ParseLambdaExpression()
        {
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            if (mHeadToken != TokenType.LeftParen)
            {
                throw new RheaException(
                    Expected("LeftParen"), mLexer.GetSourceInfo()
                );
            }
            IList<ValueSymbol> paras = ParseParameters();
            IList<IExpr> bodyExprs = ParseBlock();
            return new ExprLambda(paras, bodyExprs, info);
        }
        
        private IExpr ParseLambdaExpressionOmittingParameters()
        {
            SourceInfo info = mLexer.GetSourceInfo();
            IList<ValueSymbol> paras = new List<ValueSymbol>();
            IList<IExpr> bodyExprs = ParseBlock();
            return new ExprLambda(paras, bodyExprs, info);
        }
        
        private IList<ValueSymbol> ParseParameters()
        {
            IList<ValueSymbol> paras = new List<ValueSymbol>();
            LookAhead();
            if (mHeadToken != TokenType.RightParen)
            {
                paras.Add(ParseParameter());
                while (mHeadToken == TokenType.Comma)
                {
                    LookAhead();
                    paras.Add(ParseParameter());
                }
                if (mHeadToken != TokenType.RightParen)
                {
                    throw new RheaException(
                        Expected("RightParen"), mLexer.GetSourceInfo()
                    );
                }
            }
            LookAhead();
            return paras;
        }
        
        private ValueSymbol ParseParameter()
        {
            if (mHeadToken != TokenType.Identifier)
            {
                throw new RheaException(
                    Expected("Identifier"), mLexer.GetSourceInfo()
                );
            }
            ValueSymbol param = ValueSymbol.Intern((string)mLexer.Value);
            LookAhead();
            return param;
        }
        
        private IList<IExpr> ParseBlock()
        {
            IList<IExpr> bodyExprs;
            switch (mHeadToken)
            {
            case TokenType.LeftBrace:
                bodyExprs = ParseBracedBlock();
                break;
            case TokenType.BeginBlock:
                bodyExprs = ParseIndentedBlock();
                break;
            default:
                throw new RheaException(
                    Expected("BeginBlock or LeftBrace"), mLexer.GetSourceInfo()
                );
            }
            return bodyExprs;
        }
        
        private IList<IExpr> ParseBracedBlock()
        {
            IList<IExpr> exprs = new List<IExpr>();
            LookAhead();
            exprs.Add(ParseExpression());
            while (mHeadToken == TokenType.Semicolon)
            {
                LookAhead();
                exprs.Add(ParseExpression());
            }
            if (mHeadToken != TokenType.RightBrace)
            {
                throw new RheaException(
                    Expected("RightBrace"), mLexer.GetSourceInfo()
                );
            }
            LookAhead();
            return exprs;
        }
        
        private IList<IExpr> ParseIndentedBlock()
        {
            IList<IExpr> exprs = new List<IExpr>();
            LookAhead();
            exprs.Add(ParseExpression());
            while (mHeadToken == TokenType.Semicolon ||
                   mHeadToken == TokenType.NewLine)
            {
                LookAhead();
                exprs.Add(ParseExpression());
            }
            if (mHeadToken != TokenType.EndBlock)
            {
                throw new RheaException(
                    Expected("EndBlock"), mLexer.GetSourceInfo()
                );
            }
            LookAhead();
            return exprs;
        }
        
        private IExpr ParseParenthesizedExpression()
        {
            LookAhead();
            IExpr expr = ParseExpression();
            if (mHeadToken != TokenType.RightParen)
            {
                throw new RheaException(
                    Expected("RightParen"), mLexer.GetSourceInfo()
                );
            }
            LookAhead();
            return expr;
        }
        
        private string Unexpected()
        {
            return string.Format("unexpected {0}", mHeadToken);
        }
        
        private string Expected(string expectedTokenNames)
        {
            return string.Format(
                "unexpected {0}, expected {1}", mHeadToken, expectedTokenNames
            );
        }
    }
}
