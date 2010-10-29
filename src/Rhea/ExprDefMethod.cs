namespace Rhea
{
    public class ExprDefMethod : IExpr
    {
        private ValueSymbol mKlass;
        private ValueSymbol mSelector;
        private IExpr mValueExpr;
        private SourceInfo mInfo;
        
        public ExprDefMethod(
            ValueSymbol klass,
            ValueSymbol selector,
            IExpr valueExpr,
            SourceInfo info
        )
        {
            mKlass = klass;
            mSelector = selector;
            mValueExpr = valueExpr;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mValueExpr.Compile(compiler);
            compiler.Push(new InsnDefMethod(mKlass, mSelector, mInfo));
        }
    }
}
