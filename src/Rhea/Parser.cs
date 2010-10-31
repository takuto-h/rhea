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
            case TokenType.Semicolon:
                LookAhead();
                break;
            default:
                throw new RheaException(
                    Expected("Semicolon"), mLexer.GetSourceInfo()
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
                   mHeadToken == TokenType.Dot)
            {
                SourceInfo info = mLexer.GetSourceInfo();
                switch (mHeadToken)
                {
                case TokenType.LeftParen:
                    expr = new ExprCall(expr, ParseArguments(), info);
                    break;
                case TokenType.Dot:
                    expr = ParseMessageSend(expr);
                    break;
                }
            }
            return expr;
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
            if (mHeadToken != TokenType.LeftParen)
            {
                throw new RheaException(
                    Expected("LeftParen"), mLexer.GetSourceInfo()
                );
            }
            return new ExprSend(recvExpr, selector, ParseArguments(), info);
        }
        
        private IList<IExpr> ParseArguments()
        {
            IList<IExpr> argExprs = new List<IExpr>();
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
                expr = new ExprConst(new ValueInt((int)mLexer.Value));
                LookAhead();
                break;
            case TokenType.Identifier:
                expr = ParseReference();
                break;
            case TokenType.Colon:
                expr = ParseSymbolLiteral();
                break;
            case TokenType.Hat:
                expr = ParseLambdaExpression();
                break;
            default:
                throw new RheaException(
                    string.Format("unexpected {0}", mHeadToken),
                    mLexer.GetSourceInfo()
                );
            }
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
            if (mHeadToken != TokenType.LeftBrace)
            {
                throw new RheaException(
                    Expected("LeftBrace"), mLexer.GetSourceInfo()
                );
            }
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
            IList<IExpr> exprs = new List<IExpr>();
            LookAhead();
            do
            {
                exprs.Add(ParseStatement());
            }
            while (mHeadToken != TokenType.RightBrace);
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
