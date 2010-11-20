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
        
        public IValue this[ValueSymbol symbol]
        {
            get { return mInnerEnv[symbol]; }
            set {  mInnerEnv[symbol] = value; }
        }
        
        public IValueFunc this[ValueSymbol klass, ValueSymbol selector]
        {
            get { return mInnerEnv[klass, selector]; }
            set { mInnerEnv[klass, selector] = value; }
        }
        
        public void AddVariable(ValueSymbol symbol, IValue value)
        {
            mInnerEnv.AddVariable(symbol, value);
        }
        
        public void AddMethod(ValueSymbol klass, ValueSymbol selector, IValueFunc func)
        {
            mInnerEnv.AddMethod(klass, selector, func);
        }
        
        public bool TryGetVariable(ValueSymbol symbol, out IValue value)
        {
            return mInnerEnv.TryGetVariable(symbol, out value);
        }
        
        public bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValueFunc func)
        {
            return mInnerEnv.TryGetMethod(klass, selector, out func);
        }
        
        public bool ContainsVariable(ValueSymbol symbol)
        {
            return mInnerEnv.ContainsVariable(symbol);
        }
        
        public bool ContainsMethod(ValueSymbol klass, ValueSymbol selector)
        {
            return mInnerEnv.ContainsMethod(klass, selector);
        }
    }
}
