namespace Rhea
{
    public class InsnSetMethod : IInsn
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnSetMethod(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            IValue value1 = vm.Pop();
            IValue value2 = vm.Pop();
            IValueFunc method = value1 as IValueFunc;
            ValueSymbol klass = value2 as ValueSymbol;
            if (method == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", value1),
                    mInfo
                );
            }
            if (klass == null)
            {
                throw new RheaException(
                    string.Format("symbol required, but got {0}", value2),
                    mInfo
                );
            }
            vm.Env.SetMethod(klass, mSelector, method, mInfo);
            vm.Push(method);
        }
        
        public string Show()
        {
            return string.Format("(setmethod {0} {1})", mSelector.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
