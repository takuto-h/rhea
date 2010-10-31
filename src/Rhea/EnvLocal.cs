namespace Rhea
{
    public class EnvLocal : IEnv
    {
        private IEnv mInnerEnv;
        
        public IEnv OuterEnv { get; private set; }
        
        public EnvLocal(IEnv outerEnv)
        {
            mInnerEnv = new EnvGlobal();
            OuterEnv = outerEnv;
        }
        
        public bool IsGlobal()
        {
            return false;
        }
        
        public void AddVariable(ValueSymbol selector, IValue value)
        {
            mInnerEnv.AddVariable(selector, value);
        }
        
        public void AddMethod(ValueSymbol klass, ValueSymbol selector, IValue value)
        {
            mInnerEnv.AddMethod(klass, selector, value);
        }
        
        public bool TryGetVariable(ValueSymbol selector, out IValue value)
        {
            return mInnerEnv.TryGetVariable(selector, out value);
        }
        
        public bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValue value)
        {
            return mInnerEnv.TryGetMethod(klass, selector, out value);
        }
        
        public bool ContainsVariable(ValueSymbol selector)
        {
            return mInnerEnv.ContainsVariable(selector);
        }
        
        public bool ContainsMethod(ValueSymbol klass, ValueSymbol selector)
        {
            return mInnerEnv.ContainsMethod(klass, selector);
        }
    }
}
