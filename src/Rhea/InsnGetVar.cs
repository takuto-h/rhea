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
            vm.Push(vm.Env.GetVariable(mSelector, mInfo));
        }
    }
}
