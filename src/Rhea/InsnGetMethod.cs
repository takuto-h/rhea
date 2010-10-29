namespace Rhea
{
    public class InsnGetMethod : IInsn
    {
        private ValueSymbol mKlass;
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnGetMethod(ValueSymbol klass, ValueSymbol selector, SourceInfo info)
        {
            mKlass = klass;
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            IValue value;
            if (!vm.Env.TryGetMethod(mKlass, mSelector, out value))
            {
                throw new RheaException(
                    string.Format(
                        "unbound method: {0}:{1}", mKlass.Name, mSelector.Name
                    ), mInfo
                );
            }
            vm.Push(value);
        }
    }
}
