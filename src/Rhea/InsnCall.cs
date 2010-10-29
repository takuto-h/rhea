using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public class InsnCall : IInsn
    {
        private int mArgCount;
        private SourceInfo mInfo;
        
        public InsnCall(int argCount, SourceInfo info)
        {
            mArgCount = argCount;
            mInfo = info;
        }
        
        public void Execute(VM vm)
        {
            IList<IValue> args = new List<IValue>();
            for (int i = 0; i < mArgCount; i++)
            {
                args.Add(vm.Pop());
            }
            args.Reverse();
            IValue value = vm.Pop();
            IValueFunc func = value as IValueFunc;
            if (func == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", value), mInfo
                );
            }
            func.Call(args, vm, mInfo);
        }
    }
}
