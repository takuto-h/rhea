using System.Text;
using System.Collections.Generic;

namespace Rhea
{
    public class Lexer
    {
        private static Dictionary<string, TokenType> smReserved;
        
        private SourceReader mReader;
        private Stack<int> mOffsideLines;
        private bool mBeginningOfLine;
        private bool mBeginningOfBlock;
        
        public TokenType Token { get; private set; }
        public object Value { get; private set; }
        
        public SourceInfo GetSourceInfo()
        {
            return mReader.GetSourceInfo();
        }
        
        static Lexer()
        {
            smReserved = new Dictionary<string, TokenType>();
            smReserved.Add("def", TokenType.Def);
            smReserved.Add("end", TokenType.End);
        }
        
        public Lexer(SourceReader reader)
        {
            mReader = reader;
            mBeginningOfLine = false;
            mBeginningOfBlock = false;
            mOffsideLines = new Stack<int>();
            mOffsideLines.Push(1);
        }
        
        public bool Advance()
        {
            SkipWhiteSpace();
            int c = mReader.Peek();
            switch (c)
            {
            case -1:
                return false;
            case '#':
                SkipLineComment();
                return Advance();
            default:
                return LexToken();
            }
        }
        
        private bool LexToken()
        {
            if (mBeginningOfBlock)
            {
                return LexBeginningOfBlock();
            }
            else if (mBeginningOfLine)
            {
                return LexBeginningOfLine();
            }
            else
            {
                return LexVisibleToken();
            }
        }
        
        private bool LexBeginningOfBlock()
        {
            mBeginningOfBlock = false;
            if (mBeginningOfLine)
            {
                mBeginningOfLine = false;
            }
            int column = mReader.Column;
            int offsideLine = mOffsideLines.Peek();
            if (column > offsideLine)
            {
                mOffsideLines.Push(column);
                Token = TokenType.BeginBlock;
                return true;
            }
            else
            {
                return LexVisibleToken();
            }
        }
        
        private bool LexBeginningOfLine()
        {
            mBeginningOfLine = false;
            int column = mReader.Column;
            int offsideLine = mOffsideLines.Peek();
            if (column > offsideLine)
            {
                return LexVisibleToken();
            }
            else if (column == offsideLine)
            {
                Token = TokenType.NewLine;
                return true;
            }
            else
            {
                mOffsideLines.Pop();
                Token = TokenType.EndBlock;
                return true;
            }
        }
        
        private bool LexVisibleToken()
        {
            int c = mReader.Peek();
            switch (c)
            {
            case '(':
            case ')':
            case ',':
            case '.':
            case ';':
            case '=':
            case '^':
            case '{':
            case '}':
                Token = (TokenType)mReader.Read();
                break;
            case ':':
                mReader.Read();
                c = mReader.Peek();
                if (c != -1 && char.IsWhiteSpace((char)c))
                {
                    mBeginningOfBlock = true;
                    return Advance();
                }
                Token = TokenType.Colon;
                break;
            default:
                if (char.IsDigit((char)c))
                {
                    LexInt();
                }
                else if (IsIdentifierStart((char)c))
                {
                    LexIdentifier();
                }
                else
                {
                    throw new RheaException(
                        string.Format("unknown character: {0}", (char)c),
                        GetSourceInfo()
                    );
                }
                break;
            }
            return true;
        }
        
        private bool IsIdentifierStart(char c)
        {
            return char.IsLetter(c) || c == '_';
        }
        
        private bool IsIdentifierPart(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }
        
        private void LexInt()
        {
            int num = mReader.Read() - '0';
            int c = mReader.Peek();
            while (c != -1 && char.IsDigit((char)c))
            {
                num = num * 10 + mReader.Read() - '0';
                c = mReader.Peek();
            }
            Token = TokenType.Int;
            Value = num;
        }
        
        private void LexIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((char)mReader.Read());
            int c = mReader.Peek();
            while (c != -1 && IsIdentifierPart((char)c))
            {
                sb.Append((char)mReader.Read());
                c = mReader.Peek();
            }
            TokenType token;
            string word = sb.ToString();
            if (smReserved.TryGetValue(word, out token))
            {
                Token = token;
            }
            else
            {
                Token = TokenType.Identifier;
                Value = word;
            }
        }
        
        private void SkipWhiteSpace()
        {
            int c = mReader.Peek();
            while (c != -1 && char.IsWhiteSpace((char)c))
            {
                if (c == '\n')
                {
                    mBeginningOfLine = true;
                }
                mReader.Read();
                c = mReader.Peek();
            }
        }
        
        private void SkipLineComment()
        {
            int c = mReader.Peek();
            while (c != -1 && c != '\n')
            {
                mReader.Read();
                c = mReader.Peek();
            }
        }
    }
}
