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
            if (vm.Env.ContainsVariable(mSelector))
            {
                throw new RheaException(
                    string.Format("variable is already defined: {0}", mSelector.Name), mInfo
                );
            }
            vm.Env.AddVariable(mSelector, vm.Peek());
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
