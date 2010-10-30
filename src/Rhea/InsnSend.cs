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
            ValueSymbol klass = receiver.Klass;
            IValue value = vm.Env.GetMethod(klass, mSelector, mInfo);
            IValueFunc func = value as IValueFunc;
            if (func == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", value), mInfo
                );
            }
            List<IValue> newArgs = new List<IValue>();
            newArgs.Add(receiver);
            newArgs.AddRange(args);
            func.Call(newArgs, vm, mInfo);
        }
    }
}
