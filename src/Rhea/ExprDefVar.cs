namespace Rhea
{
    public class ExprDefVar : IExpr
    {
        private ValueSymbol mSymbol;
        private IExpr mValueExpr;
        private SourceInfo mInfo;
        
        public ExprDefVar(ValueSymbol symbol, IExpr valueExpr, SourceInfo info)
        {
            mSymbol = symbol;
            mValueExpr = valueExpr;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mValueExpr.Compile(compiler);
            compiler.Push(new InsnDefVar(mSymbol, mInfo));
        }
    }
}
