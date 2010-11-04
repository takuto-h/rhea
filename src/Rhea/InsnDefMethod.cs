namespace Rhea
{
    public class InsnDefMethod : IInsn
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnDefMethod(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            IValue method = vm.Pop();
            IValue value = vm.Pop();
            ValueSymbol klass = value as ValueSymbol;
            if (klass == null)
            {
                throw new RheaException(
                    string.Format("symbol required, but got {0}", value),
                    mInfo
                );
            }
            vm.Env.DefineMethod(klass, mSelector, method, mInfo);
            vm.Push(method);
        }
        
        public string Show()
        {
            return string.Format("(defmethod {0} {1})", mSelector.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
