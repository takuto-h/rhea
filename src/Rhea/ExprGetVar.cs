namespace Rhea
{
    public class ExprGetVar : IExpr
    {
        private ValueSymbol mSelector;
        private SourceInfo mInfo;
        
        public ExprGetVar(ValueSymbol selector, SourceInfo info)
        {
            mSelector = selector;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            compiler.Push(new InsnGetVar(mSelector, mInfo));
        }
    }
}
