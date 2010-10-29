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
                IExpr valueExpr = ParseExpression();
                return new ExprDefVar(selector, valueExpr, info);
            }
            return ParsePrimary();
        }
        
        private IExpr ParsePrimary()
        {
            IExpr expr = ParseAtom();
            while (mHeadToken == TokenType.LeftParen)
            {
                SourceInfo info = mLexer.GetSourceInfo();
                expr = new ExprCall(expr, ParseArguments(), info);
            }
            return expr;
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
                expr = new ExprGetVar(
                    ValueSymbol.Intern((string)mLexer.Value),
                    mLexer.GetSourceInfo()
                );
                LookAhead();
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
            while (mHeadToken != TokenType.RightBrace)
            {
                exprs.Add(ParseStatement());
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
