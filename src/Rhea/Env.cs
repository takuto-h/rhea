namespace Rhea
{
    public static class Env
    {
        public static void AddVariable(
            this IEnv env,
            string symbolName,
            IValue value
        )
        {
            env.AddVariable(
                ValueSymbol.Intern(symbolName), value
            );
        }
        
        public static void AddMethod(
            this IEnv env,
            ValueSymbol klass,
            string selectorName,
            IValue value
        )
        {
            env.AddMethod(
                klass, ValueSymbol.Intern(selectorName), value
            );
        }
        
        public static void AddVariable(
            this IEnv env,
            string symbolName,
            int paramCount,
            Subr subrValue
        )
        {
            AddVariable(
                env,
                symbolName,
                new ValueSubr(
                    string.Format("{0}", symbolName),
                    paramCount,
                    subrValue
                )
            );
        }
        
        public static void AddMethod(
            this IEnv env,
            ValueSymbol klass,
            string selectorName,
            int paramCount,
            Subr subrValue
        )
        {
            AddMethod(
                env,
                klass,
                selectorName,
                new ValueSubr(
                    string.Format("{0}:{1}", klass.Name, selectorName),
                    paramCount,
                    subrValue
                )
            );
        }
        
        public static void DefineVariable(
            this IEnv env,
            ValueSymbol symbol,
            IValue value,
            SourceInfo info
        )
        {
            /*if (env.ContainsVariable(symbol))
            {
                throw new RheaException(
                    string.Format("variable is already defined: {0}", symbol.Name), info
                );
            }
            env.AddVariable(symbol, value);*/
            env[symbol] = value;
        }
        
        public static void DefineMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            IValue value,
            SourceInfo info
        )
        {
            /*if (env.ContainsMethod(klass, selector))
            {
                throw new RheaException(
                    string.Format(
                        "method is already defined: {0}:{1}",
                        klass.Name, selector.Name
                    ), info
                );
            }
            env.AddMethod(klass, selector, value);*/
            env[klass, selector] = value;
        }
        
        public static IValue GetVariable(
            this IEnv env,
            ValueSymbol symbol,
            SourceInfo info
        )
        {
            IValue value;
            while (!env.IsGlobal())
            {
                if (env.TryGetVariable(symbol, out value))
                {
                    return value;
                }
                env = env.OuterEnv;
            }
            if (env.TryGetVariable(symbol, out value))
            {
                return value;
            }
            throw new RheaException(
                string.Format("variable is not defined: {0}", symbol.Name), info
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
                env = env.OuterEnv;
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
            ValueSymbol symbol,
            IValue value,
            SourceInfo info
        )
        {
            while (!env.IsGlobal())
            {
                if (env.ContainsVariable(symbol))
                {
                    env[symbol] = value;
                    return value;
                }
                env = env.OuterEnv;
            }
            if (env.ContainsVariable(symbol))
            {
                env[symbol] = value;
                return value;
            }
            throw new RheaException(
                string.Format("variable is not defined: {0}", symbol.Name), info
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
