using System.Text;
using System.Collections.Generic;

namespace Rhea
{
    public class Lexer
    {
        private static Dictionary<string, TokenType> smReserved;
        
        private SourceReader mReader;
        
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
        }
        
        public Lexer(SourceReader reader)
        {
            mReader = reader;
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
            case '(':
            case ')':
            case ',':
            case '.':
            case ':':
            case ';':
            case '=':
            case '^':
            case '{':
            case '}':
                Token = (TokenType)mReader.Read();
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
