using System.Collections.Generic;

namespace Rhea
{
    public class ExprSend : IExpr
    {
        private IExpr mRecvExpr;
        private ValueSymbol mSelector;
        private IList<IExpr> mArgExprs;
        private SourceInfo mInfo;
        
        public ExprSend(
            IExpr recvExpr,
            ValueSymbol selector,
            IList<IExpr> argExprs,
            SourceInfo info
        )
        {
            mRecvExpr = recvExpr;
            mSelector = selector;
            mArgExprs = argExprs;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mRecvExpr.Compile(compiler);
            foreach (IExpr argExpr in mArgExprs)
            {
                argExpr.Compile(compiler);
            }
            compiler.Push(new InsnSend(mSelector, mArgExprs.Count, mInfo));
        }
    }
}
