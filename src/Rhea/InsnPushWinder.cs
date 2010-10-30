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
        
        public string Show()
        {
            return string.Format("(pushwinder {0} {1})", mBefore, mAfter);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
