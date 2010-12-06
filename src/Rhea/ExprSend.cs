using System.Collections.Generic;

namespace Rhea
{
    public class ExprSend : IExpr
    {
        public IExpr RecvExpr { get; private set; }
        public ValueSymbol Selector { get; private set; }
        public IList<IExpr> ArgExprs { get; private set; }
        private SourceInfo mInfo;
        
        public ExprSend(
            IExpr recvExpr,
            ValueSymbol selector,
            IList<IExpr> argExprs,
            SourceInfo info
        )
        {
            RecvExpr = recvExpr;
            Selector = selector;
            ArgExprs = argExprs;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            RecvExpr.Compile(compiler);
            foreach (IExpr argExpr in ArgExprs)
            {
                argExpr.Compile(compiler);
            }
            compiler.Push(new InsnSend(Selector, ArgExprs.Count, mInfo));
        }
    }
}
