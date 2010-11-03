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
                LookAhead();
                if (mHeadToken == TokenType.Colon)
                {
                    return ParseMethodDefinition(symbol);
                }
                return ParseVariableDefinition(symbol);
            }
            return ParsePrimary();
        }
        
        private IExpr ParseVariableDefinition(ValueSymbol selector)
        {
            if (mHeadToken != TokenType.Equal)
            {
                throw new RheaException(
                    Expected("Equal"), mLexer.GetSourceInfo()
                );
            }
            SourceInfo info = mLexer.GetSourceInfo();
            LookAhead();
            return new ExprDefVar(selector, ParseExpression(), info);
        }
        
        private IExpr ParseMethodDefinition(ValueSymbol klass)
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
            return new ExprDefMethod(klass, selector, ParseExpression(), info);
        }
        
        private IExpr ParsePrimary()
        {
            IExpr expr = ParseAtom();
            while (mHeadToken == TokenType.LeftParen ||
                   mHeadToken == TokenType.Hat ||
                   mHeadToken == TokenType.BeginBlock ||
                   mHeadToken == TokenType.LeftBrace ||
                   mHeadToken == TokenType.Dot)
            {
                switch (mHeadToken)
                {
                case TokenType.LeftParen:
                case TokenType.Hat:
                case TokenType.BeginBlock:
                case TokenType.LeftBrace:
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
            default:
                throw new RheaException(
                    string.Format("unexpected {0}", mHeadToken),
                    mLexer.GetSourceInfo()
                );
            }
            return expr;
        }
        
        private IExpr ParseInt()
        {
            IExpr expr = new ExprConst(new ValueInt((int)mLexer.Value));
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
                return ParseMethodReference(symbol);
            }
            return ParseVariableReference(symbol, info);
        }
        
        private IExpr ParseMethodReference(ValueSymbol klass)
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
            return new ExprGetMethod(klass, selector, info);
        }
        
        private IExpr ParseVariableReference(ValueSymbol selector, SourceInfo info)
        {
            if (mHeadToken == TokenType.Equal)
            {
                info = mLexer.GetSourceInfo();
                LookAhead();
                return new ExprSetVar(selector, ParseExpression(), info);
            }
            return new ExprGetVar(selector, info);
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
            mLexer.BeginBlock();
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
        
        private string Expected(string expectedTokenNames)
        {
            return string.Format(
                "unexpected {0}, expected {1}", mHeadToken, expectedTokenNames
            );
        }
    }
}
