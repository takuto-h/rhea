namespace Rhea
{
    public class ExprGetVar : IExpr
    {
        private ValueSymbol mSymbol;
        private SourceInfo mInfo;
        
        public ExprGetVar(ValueSymbol symbol, SourceInfo info)
        {
            mSymbol = symbol;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            compiler.Push(new InsnGetVar(mSymbol, mInfo));
        }
    }
}
