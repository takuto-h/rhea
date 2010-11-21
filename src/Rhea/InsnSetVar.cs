namespace Rhea
{
    public class InsnSetVar : IInsn
    {
        private ValueSymbol mSymbol;
        private SourceInfo mInfo;
        
        public InsnSetVar(ValueSymbol symbol, SourceInfo info)
        {
            mSymbol = symbol;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Env.SetVariable(mSymbol, vm.Peek(), mInfo);
        }
        
        public string Show()
        {
            return string.Format(
                "(setvar {0} {1})", mSymbol.Name.ToIdentifier(), mInfo
            );
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
