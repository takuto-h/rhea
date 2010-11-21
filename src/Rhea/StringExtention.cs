namespace Rhea
{
    public static class StringExtention
    {
        public static string ToIdentifier(this string str)
        {
            if (!Lexer.IsIdentifier(str))
            {
                return string.Format("|{0}|", str);
            }
            return str;
        }
    }
}
