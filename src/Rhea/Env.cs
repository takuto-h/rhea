namespace Rhea
{
    public static class Env
    {
        public static void AddVariable(
            this IEnv env,
            string selectorName,
            int paramCount,
            Subr subrValue
        )
        {
            env.AddVariable(
                ValueSymbol.Intern(selectorName),
                new ValueSubr(
                    string.Format("{0}", selectorName),
                    paramCount,
                    subrValue
                )
            );
        }
        
        public static void AddMethod(
            this IEnv env,
            string klassName,
            string selectorName,
            int paramCount,
            Subr subrValue
        )
        {
            env.AddMethod(
                ValueSymbol.Intern(klassName),
                ValueSymbol.Intern(selectorName),
                new ValueSubr(
                    string.Format("{0}:{1}", klassName, selectorName),
                    paramCount,
                    subrValue
                )
            );
        }
    }
}
