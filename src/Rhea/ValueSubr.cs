using System.Collections.Generic;

namespace Rhea
{
    public delegate void Subr(IList<IValue> args, VM vm, SourceInfo info);
    
    public class ValueSubr : IValueFunc
    {
        private static ValueSymbol smKlass;
        
        private string mName;
        private int mParamCount;
        private Subr mSubrValue;
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("Subr");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
        }
        
        public ValueSubr(
            string name, int paramCount, Subr subrValue
        )
        {
            mName = name;
            mParamCount = paramCount;
            mSubrValue = subrValue;
        }
        
        public void Call(IList<IValue> args, VM vm, SourceInfo info)
        {
            int argCount = args.Count;
            if (argCount != mParamCount)
            {
                throw new RheaException(
                    this.WrongNumberOfArguments(mParamCount, argCount), info
                );
            }
            mSubrValue(args, vm, info);
        }
        
        public string Show()
        {
            return string.Format("#<subr {0}>", mName);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
