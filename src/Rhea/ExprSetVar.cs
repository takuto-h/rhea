namespace Rhea
{
    public class ExprSetVar : IExpr
    {
        private ValueSymbol mSelector;
        private IExpr mValueExpr;
        private SourceInfo mInfo;
        
        public ExprSetVar(ValueSymbol selector, IExpr valueExpr, SourceInfo info)
        {
            mSelector = selector;
            mValueExpr = valueExpr;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mValueExpr.Compile(compiler);
            compiler.Push(new InsnSetVar(mSelector, mInfo));
        }
    }
}
