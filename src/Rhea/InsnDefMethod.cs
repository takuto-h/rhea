namespace Rhea
{
    public class InsnDefMethod : IInsn
    {
        private ValueSymbol mKlass;
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnDefMethod(
            ValueSymbol klass,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            mKlass = klass;
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            if (vm.Env.ContainsMethod(mKlass, mSelector))
            {
                throw new RheaException(
                    string.Format(
                        "method is already defined: {0}:{1}",
                        mKlass.Name, mSelector.Name
                    ), mInfo
                );
            }
            vm.Env.AddMethod(mKlass, mSelector, vm.Peek());
        }
        
        public string Show()
        {
            return string.Format(
                "(defmethod {0} {1} {2})", mKlass.Name, mSelector.Name, mInfo
            );
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
