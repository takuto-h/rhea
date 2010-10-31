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
        
        public static void DefineVariable(
            this IEnv env,
            ValueSymbol selector,
            IValue value,
            SourceInfo info
        )
        {
            if (env.ContainsVariable(selector))
            {
                throw new RheaException(
                    string.Format("variable is already defined: {0}", selector.Name), info
                );
            }
            env.AddVariable(selector, value);
        }
        
        public static void DefineMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            IValue value,
            SourceInfo info
        )
        {
            if (env.ContainsMethod(klass, selector))
            {
                throw new RheaException(
                    string.Format(
                        "method is already defined: {0}:{1}",
                        klass.Name, selector.Name
                    ), info
                );
            }
            env.AddMethod(klass, selector, value);
        }
        
        public static IValue GetVariable(
            this IEnv env,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            IValue value;
            while (!env.IsGlobal())
            {
                if (env.TryGetVariable(selector, out value))
                {
                    return value;
                }
                env = env.OuterEnv;
            }
            if (env.TryGetVariable(selector, out value))
            {
                return value;
            }
            throw new RheaException(
                string.Format("variable is not defined: {0}", selector.Name), info
            );
        }
        
        public static IValue GetMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            IValue value;
            while (!env.IsGlobal())
            {
                if (env.TryGetMethod(klass, selector, out value))
                {
                    return value;
                }
            }
            if (env.TryGetMethod(klass, selector, out value))
            {
                return value;
            }
            throw new RheaException(
                string.Format("method is not defined: {0}:{1}", klass.Name, selector.Name), info
            );
        }
        
        public static IValue SetVariable(
            this IEnv env,
            ValueSymbol selector,
            IValue value,
            SourceInfo info
        )
        {
            while (!env.IsGlobal())
            {
                if (env.ContainsVariable(selector))
                {
                    env[selector] = value;
                    return value;
                }
                env = env.OuterEnv;
            }
            if (env.ContainsVariable(selector))
            {
                env[selector] = value;
                return value;
            }
            throw new RheaException(
                string.Format("variable is not defined: {0}", selector.Name), info
            );
        }
        
        public static IValue SetMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            IValue value,
            SourceInfo info
        )
        {
            while (!env.IsGlobal())
            {
                if (env.ContainsMethod(klass, selector))
                {
                    env[klass, selector] = value;
                    return value;
                }
                env = env.OuterEnv;
            }
            if (env.ContainsMethod(klass, selector))
            {
                env[klass, selector] = value;
                return value;
            }
            throw new RheaException(
                string.Format("method is not defined: {0}:{1}", klass.Name, selector.Name), info
            );
        }
    }
}
