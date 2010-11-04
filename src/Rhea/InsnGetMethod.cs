namespace Rhea
{
    public class InsnGetMethod : IInsn
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnGetMethod(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            IValue value = vm.Pop();
            ValueSymbol klass = value as ValueSymbol;
            if (klass == null)
            {
                throw new RheaException(
                    string.Format("symbol required, but got {0}", value),
                    mInfo
                );
            }
            vm.Push(vm.Env.GetMethod(klass, mSelector, mInfo));
        }
        
        public string Show()
        {
            return string.Format("(getmethod {0} {1})", mSelector.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
