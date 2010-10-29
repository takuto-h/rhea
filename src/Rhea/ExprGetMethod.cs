namespace Rhea
{
    public class ExprGetMethod : IExpr
    {
        private ValueSymbol mKlass;
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public ExprGetMethod(ValueSymbol klass, ValueSymbol selector, SourceInfo info)
        {
            mKlass = klass;
            mSelector = selector;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            compiler.Push(new InsnGetMethod(mKlass, mSelector, mInfo));
        }
    }
}
