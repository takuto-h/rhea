namespace Rhea
{
    public class InsnSetVar : IInsn
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnSetVar(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Env.SetVariable(mSelector, vm.Peek(), mInfo);
        }
        
        public string Show()
        {
            return string.Format("(setvar {0} {1})", mSelector.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
