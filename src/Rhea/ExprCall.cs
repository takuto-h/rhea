using System.Collections.Generic;

namespace Rhea
{
    public class ExprCall : IExpr
    {
        private IExpr mFuncExpr;
        private IList<IExpr> mArgExprs;
        private SourceInfo mInfo;
        
        public ExprCall(IExpr funcExpr, IList<IExpr> argExprs, SourceInfo info)
        {
            mFuncExpr = funcExpr;
            mArgExprs = argExprs;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            mFuncExpr.Compile(compiler);
            foreach (IExpr argExpr in mArgExprs)
            {
                argExpr.Compile(compiler);
            }
            compiler.Push(new InsnCall(mArgExprs.Count, mInfo));
        }
    }
}
