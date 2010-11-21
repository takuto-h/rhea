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
            IValueFunc func
        )
        {
            env.AddMethod(
                klass, ValueSymbol.Intern(selectorName), func
            );
        }
        
        public static void AddVariable(
            this IEnv env,
            string symbolName,
            int paramCount,
            bool allowRest,
            Subr subrValue
        )
        {
            string subrName = symbolName.ToIdentifier();
            AddVariable(
                env, symbolName,
                new ValueSubr(subrName, paramCount, allowRest, subrValue)
            );
        }
        
        public static void AddMethod(
            this IEnv env,
            ValueSymbol klass,
            string selectorName,
            int paramCount,
            bool allowRest,
            Subr subrValue
        )
        {
            string subrName = string.Format(
                "{0}:{1}",
                klass.Name.ToIdentifier(),
                selectorName.ToIdentifier()
            );
            AddMethod(
                env, klass, selectorName,
                new ValueSubr(subrName, paramCount, allowRest, subrValue)
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
                    string.Format(
                        "variable is already defined: {0}",
                        symbol.Name.ToIdentifier()
                    ),
                    info
                );
            }
            env.AddVariable(symbol, value);*/
            env[symbol] = value;
        }
        
        public static void DefineMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            IValueFunc func,
            SourceInfo info
        )
        {
            /*if (env.ContainsMethod(klass, selector))
            {
                throw new RheaException(
                    string.Format(
                        "method is already defined: {0}:{1}",
                        klass.Name.ToIdentifier(),
                        selector.Name.ToIdentifier()
                    ),
                    info
                );
            }
            env.AddMethod(klass, selector, func);*/
            env[klass, selector] = func;
        }
        
        public static IValue GetVariable(
            this IEnv env,
            ValueSymbol symbol,
            SourceInfo info
        )
        {
            IValue value;
            if (!LookupVariable(env, symbol, out value))
            {
                throw new RheaException(
                    string.Format(
                        "variable is not defined: {0}",
                        symbol.Name.ToIdentifier()
                    ),
                    info
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
            IValueFunc func;
            if (!LookupMethod(env, klass, selector, out func))
            {
                throw new RheaException(
                    string.Format(
                        "method is not defined: {0}:{1}",
                        klass.Name.ToIdentifier(),
                        selector.Name.ToIdentifier()
                    ),
                    info
                );
            }
            return func;
        }
        
        public static bool LookupVariable(
            this IEnv env,
            ValueSymbol symbol,
            out IValue value
        )
        {
            while (!env.IsGlobal())
            {
                if (env.TryGetVariable(symbol, out value))
                {
                    return true;
                }
                env = env.OuterEnv;
            }
            if (env.TryGetVariable(symbol, out value))
            {
                return true;
            }
            return false;
        }
        
        public static bool LookupMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            out IValueFunc func
        )
        {
            while (!env.IsGlobal())
            {
                if (env.TryGetMethod(klass, selector, out func))
                {
                    return true;
                }
                env = env.OuterEnv;
            }
            if (env.TryGetMethod(klass, selector, out func))
            {
                return true;
            }
            return false;
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
                string.Format(
                    "variable is not defined: {0}",
                    symbol.Name.ToIdentifier()
                ),
                info
            );
        }
        
        public static IValue SetMethod(
            this IEnv env,
            ValueSymbol klass,
            ValueSymbol selector,
            IValueFunc func,
            SourceInfo info
        )
        {
            while (!env.IsGlobal())
            {
                if (env.ContainsMethod(klass, selector))
                {
                    env[klass, selector] = func;
                    return func;
                }
                env = env.OuterEnv;
            }
            if (env.ContainsMethod(klass, selector))
            {
                env[klass, selector] = func;
                return func;
            }
            throw new RheaException(
                string.Format(
                    "method is not defined: {0}:{1}",
                    klass.Name.ToIdentifier(),
                    selector.Name.ToIdentifier()
                ),
                info
            );
        }
    }
}
