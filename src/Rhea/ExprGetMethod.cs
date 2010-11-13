namespace Rhea
{
    public class ExprGetMethod : IExpr
    {
        public IExpr KlassExpr { get; private set; }
        public ValueSymbol Selector { get; private set; }
        private SourceInfo mInfo;
        
        public ExprGetMethod(
            IExpr klassExpr,
            ValueSymbol selector,
            SourceInfo info
        )
        {
            KlassExpr = klassExpr;
            Selector = selector;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            KlassExpr.Compile(compiler);
            compiler.Push(new InsnGetMethod(Selector, mInfo));
        }
    }
}
