namespace Rhea
{
    public class InsnGetVar : IInsn
    {
        private ValueSymbol mSymbol;
        private SourceInfo mInfo;
        
        public InsnGetVar(ValueSymbol symbol, SourceInfo info)
        {
            mSymbol = symbol;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Push(vm.Env.GetVariable(mSymbol, mInfo));
        }
        
        public string Show()
        {
            return string.Format(
                "(getvar {0} {1})", mSymbol.Name.ToIdentifier(), mInfo
            );
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
