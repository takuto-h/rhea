namespace Rhea
{
    public class InsnGetVar : IInsn
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnGetVar(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            IValue value;
            if (!vm.Env.TryGetVariable(mSelector, out value))
            {
                throw new RheaException(
                    string.Format("unbound variable: {0}", mSelector.Name), mInfo
                );
            }
            vm.Push(value);
        }
    }
}
