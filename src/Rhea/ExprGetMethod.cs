namespace Rhea
{
    public class ExprGetMethod : IExpr
    {
        private IExpr mKlassExpr;
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public ExprGetMethod(
            IExpr klassExpr,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            mKlassExpr = klassExpr;
            mSelector = selector;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mKlassExpr.Compile(compiler);
            compiler.Push(new InsnGetMethod(mSelector, mInfo));
        }
    }
}
