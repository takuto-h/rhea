namespace Rhea
{
    public static class ValueFunc
    {
        public static string WrongNumberOfArguments(
            this IValueFunc func,
            int paramCount,
            int argCount
        )
        {
            return string.Format(
                "wrong number of arguments for {0} (required {1}, got {2})",
                func, paramCount, argCount
            );
        }
    }
}
