namespace Rhea
{
    public class ExprSetMethod : IExpr
    {
        private IExpr mKlassExpr;
        private ValueSymbol mSelector;
        private IExpr mValueExpr;
        private SourceInfo mInfo;
        
        public ExprSetMethod(
            IExpr klassExpr,
            ValueSymbol selector,
            IExpr valueExpr,
            SourceInfo info
        )
        {
            mKlassExpr = klassExpr;
            mSelector = selector;
            mValueExpr = valueExpr;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mKlassExpr.Compile(compiler);
            mValueExpr.Compile(compiler);
            compiler.Push(new InsnSetMethod(mSelector, mInfo));
        }
    }
}
