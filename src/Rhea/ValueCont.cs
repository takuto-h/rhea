using System.Collections.Generic;

namespace Rhea
{
    public class ValueCont : IValueFunc
    {
        private ISList<IInsn> mInsns;
        private ISList<IValue> mStack;
        private IEnv mEnv;
        private ISList<KeyValuePair<IValueFunc, IValueFunc>> mWinders;
        
        public ValueSymbol Klass
        {
            get { return ValueSymbol.Intern("Cont"); }
        }
        
        public ValueCont(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValueFunc, IValueFunc>> winders
        )
        {
            mInsns = insns;
            mStack = stack;
            mEnv = env;
            mWinders = winders;
        }
        
        public void Call(IList<IValue> args, VM vm, SourceInfo info)
        {
            if (args.Count != 1)
            {
                throw new RheaException(
                    this.WrongNumberOfArguments(1, args.Count), info
                );
            }
            vm.SetDynamicContext(args[0], mInsns, mStack, mEnv, mWinders, info);
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
