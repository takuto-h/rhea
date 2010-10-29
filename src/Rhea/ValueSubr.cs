using System.Collections.Generic;

namespace Rhea
{
    public delegate void Subr(IList<IValue> args, VM vm, SourceInfo info);
    
    public class ValueSubr : IValueFunc
    {
        private string mName;
        private int mParamCount;
        private Subr mSubrValue;
        
        public ValueSymbol Klass
        {
            get { return ValueSymbol.Intern("Subr"); }
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
                    string.Format(
                        "wrong number of arguments for {0} (required {1}, got {2})",
                        this, mParamCount, argCount
                    ), info
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
