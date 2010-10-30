using System.Collections.Generic;

namespace Rhea
{
    public class InsnMakeClosure : IInsn
    {
        private IList<ValueSymbol> mParams;
        private ISList<IInsn> mInsns;
        private SourceInfo mInfo;
        
        public InsnMakeClosure(
            IList<ValueSymbol> paras,
            ISList<IInsn> insns,
            SourceInfo info
        )
        {
            mParams = paras;
            mInsns = insns;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            vm.Push(new ValueClosure(mParams, mInsns, vm.Env, mInfo));
        }
        
        public string Show()
        {
            return string.Format("(makeclosure {0})", mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
