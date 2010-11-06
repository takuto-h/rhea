namespace Rhea
{
    public class InsnDefVar : IInsn
    {
        private ValueSymbol mSymbol;
        private SourceInfo mInfo;
        
        public InsnDefVar(ValueSymbol symbol, SourceInfo info)
        {
            mSymbol = symbol;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Env.DefineVariable(mSymbol, vm.Peek(), mInfo);
        }
        
        public string Show()
        {
            return string.Format("(defvar {0} {1})", mSymbol.Name, mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
