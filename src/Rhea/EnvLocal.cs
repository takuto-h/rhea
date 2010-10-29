namespace Rhea
{
    public class EnvLocal : EnvGlobal
    {
        private IEnv mOuterEnv;
        
        public EnvLocal(IEnv outerEnv) : base()
        {
            mOuterEnv = outerEnv;
        }
        
        public override bool TryGetVariable(ValueSymbol selector, out IValue value)
        {
            return base.TryGetVariable(selector, out value)
              || mOuterEnv.TryGetVariable(selector, out value);
        }
        
        public override bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValue value)
        {
            return base.TryGetMethod(klass, selector, out value)
              || mOuterEnv.TryGetMethod(klass, selector, out value);
        }
    }
}
