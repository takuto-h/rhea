using System.Collections.Generic;

namespace Rhea
{
    public class ValueCont : IValueFunc
    {
        private ISList<IInsn> mInsns;
        private ISList<IValue> mStack;
        private IEnv mEnv;
        
        public ValueCont(ISList<IInsn> insns, ISList<IValue> stack, IEnv env)
        {
            mInsns = insns;
            mStack = stack;
            mEnv = env;
        }
        
        public void Call(IList<IValue> args, VM vm, SourceInfo info)
        {
            if (args.Count != 1)
            {
                throw new RheaException(
                    "continuation can be called with only 1 argument", info
                );
            }
            vm.SetCont(mInsns, mStack, mEnv);
            vm.Push(args[0]);
        }
        
        public string Show()
        {
            return string.Format("#<cont 0x{0:x8}>", GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
