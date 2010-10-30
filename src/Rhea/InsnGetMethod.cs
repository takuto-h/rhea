namespace Rhea
{
    public class InsnGetMethod : IInsn
    {
        private ValueSymbol mKlass;
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public InsnGetMethod(ValueSymbol klass, ValueSymbol selector, SourceInfo info)
        {
            mKlass = klass;
            mSelector = selector;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Push(vm.Env.GetMethod(mKlass, mSelector, mInfo));
        }
        
        public string Show()
        {
            return string.Format(
                "(getmethod {0} {1} {2})", mKlass.Name, mSelector.Name, mInfo
            );
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
