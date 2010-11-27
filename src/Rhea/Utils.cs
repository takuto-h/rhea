namespace Rhea
{
    public static class Utils
    {
        public static string ToIdentifier(this string str)
        {
            if (!Lexer.IsIdentifier(str))
            {
                return string.Format("|{0}|", str);
            }
            return str;
        }
        
        public static IValue ToValueBool(this bool boolean)
        {
            if (boolean)
            {
                return ValueTrue.Instance;
            }
            else
            {
                return ValueFalse.Instance;
            }
        }
    }
}
