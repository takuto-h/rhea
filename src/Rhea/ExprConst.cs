namespace Rhea
{
    public class ExprConst : IExpr
    {
        private IValue mValue;
        
        public ExprConst(IValue value)
        {
            mValue = value;
        }
        
        public void Compile(Compiler compiler)
        {
            compiler.Push(new InsnPush(mValue));
        }
    }
}
