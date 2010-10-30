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
    }
}
