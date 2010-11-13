namespace Rhea
{
    public class ExprGetVar : IExpr
    {
        public ValueSymbol Symbol { get; private set; }
        private SourceInfo mInfo;
        
        public ExprGetVar(ValueSymbol symbol, SourceInfo info)
        {
            Symbol = symbol;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            compiler.Push(new InsnGetVar(Symbol, mInfo));
        }
    }
}
