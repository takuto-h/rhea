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
        
        public static IValue GetVariable(
            this IEnv env,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            IValue value;
            if (!env.TryGetVariable(selector, out value))
            {
                throw new RheaException(
                    string.Format(
                        "unbound variable: {0}", selector.Name
                    ), info
                );
            }
            return value;
        }
        
        public static IValue GetMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            IValue value;
            if (!env.TryGetMethod(klass, selector, out value))
            {
                throw new RheaException(
                    string.Format(
                        "unbound method: {0}:{1}", klass.Name, selector.Name
                    ), info
                );
            }
            return value;
        }
    }
}
