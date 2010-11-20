using System.Collections.Generic;

namespace Rhea
{
    public class InsnSend : IInsn
    {
        private ValueSymbol mSelector;
        private int mArgCount;
        private SourceInfo mInfo;
        
        public InsnSend(ValueSymbol selector, int argCount, SourceInfo info)
        {
            mSelector = selector;
            mArgCount = argCount;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            List<IValue> args = new List<IValue>();
            for (int i = 0; i < mArgCount; i++)
            {
                args.Add(vm.Pop());
            }
            args.Reverse();
            IValue receiver = vm.Pop();
            receiver.Send(mSelector, args, vm, mInfo);
        }
        
        public string Show()
        {
            return string.Format(
                "(send {0} {1} {2})", mSelector, mArgCount, mInfo
            );
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
