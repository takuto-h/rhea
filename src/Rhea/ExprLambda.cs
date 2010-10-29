using System.Collections.Generic;

namespace Rhea
{
    public class ExprLambda : IExpr
    {
        private IList<ValueSymbol> mParams;
        private IList<IExpr> mBodyExprs;
        private SourceInfo mInfo;
        
        public ExprLambda(
            IList<ValueSymbol> paras,
            IList<IExpr> bodyExprs,
            SourceInfo info
        )
        {
            mParams = paras;
            mBodyExprs = bodyExprs;
            mInfo = info;
        }
        
        public void Compile(Compiler compiler)
        {
            Compiler bodyCompiler = new Compiler(compiler);
            foreach (IExpr expr in mBodyExprs)
            {
                expr.Compile(bodyCompiler);
            }
            bodyCompiler.Push(new InsnCall(1, mInfo));
            ISList<IInsn> insns = bodyCompiler.GetResult();
            compiler.Push(new InsnMakeClosure(mParams, insns, mInfo));
        }
    }
}
