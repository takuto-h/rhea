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
        
        public string Show()
        {
            return string.Format("(getvar {0} {1})", mSelector.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
