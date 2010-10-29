namespace Rhea
{
    public class ExprDefVar : IExpr
    {
        private ValueSymbol mSelector;
        private IExpr mValueExpr;
        private SourceInfo mInfo;
        
        public ExprDefVar(ValueSymbol selector, IExpr valueExpr, SourceInfo info)
        {
            mSelector = selector;
            mValueExpr = valueExpr;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mValueExpr.Compile(compiler);
            compiler.Push(new InsnDefVar(mSelector, mInfo));
        }
    }
}
