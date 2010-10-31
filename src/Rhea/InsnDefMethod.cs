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
            vm.Env.DefineMethod(mKlass, mSelector, vm.Peek(), mInfo);
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
