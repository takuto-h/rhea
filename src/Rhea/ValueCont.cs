using System.Collections.Generic;

namespace Rhea
{
    public class ValueCont : IValueFunc
    {
        private static ValueSymbol smKlass;
        
        public ISList<IInsn> Insns { get; private set; }
        public ISList<IValue> Stack { get; private set; }
        public IEnv Env { get; private set; }
        public ISList<KeyValuePair<IValue, IValue>> Winders { get; private set; }
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("Cont");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
        }
        
        public ValueCont(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValue, IValue>> winders
        )
        {
            Insns = insns;
            Stack = stack;
            Env = env;
            Winders = winders;
        }
        
        public void Call(IList<IValue> args, VM vm, SourceInfo info)
        {
            if (args.Count != 1)
            {
                throw new RheaException(
                    this.WrongNumberOfArguments(1, args.Count), info
                );
            }
            vm.SetCont(args[0], this, info);
        }
        
        public string Show()
        {
            return string.Format("$<cont 0x{0:x8}>", GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
