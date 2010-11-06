namespace Rhea
{
    public class ExprSetVar : IExpr
    {
        private ValueSymbol mSymbol;
        private IExpr mValueExpr;
        private SourceInfo mInfo;
        
        public ExprSetVar(ValueSymbol symbol, IExpr valueExpr, SourceInfo info)
        {
            mSymbol = symbol;
            mValueExpr = valueExpr;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mValueExpr.Compile(compiler);
            compiler.Push(new InsnSetVar(mSymbol, mInfo));
        }
    }
}
