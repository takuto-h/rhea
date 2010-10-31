namespace Rhea
{
    public class InsnDefVar : IInsn
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnDefVar(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Env.DefineVariable(mSelector, vm.Peek(), mInfo);
        }
        
        public string Show()
        {
            return string.Format("(defvar {0} {1})", mSelector.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
