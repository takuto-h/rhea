using System.Collections.Generic;

namespace Rhea
{
    public class InsnPushWinder : IInsn
    {
        private IValue mBefore;
        private IValue mAfter;
        
        public InsnPushWinder(IValue before, IValue after)
        {
            mBefore = before;
            mAfter = after;
        }
        
        public void Execute(VM vm)
        {
            vm.PushWinder(mBefore, mAfter);
        }
    }
}
