using System.Collections.Generic;

namespace Rhea
{
    public delegate void Subr(IList<IValue> args, VM vm, SourceInfo info);
    
    public class ValueSubr : IValueFunc
    {
        private static KlassHolder smKlassHolder;
        
        private string mName;
        private int mParamCount;
        private Subr mSubrValue;
        
        static ValueSubr()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Subr,
                    Klasses.Func,
                    Klasses.Object
                }
            );
        }
        
        public ValueSubr(
            string name, int paramCount, Subr subrValue
        )
        {
            mName = name;
            mParamCount = paramCount;
            mSubrValue = subrValue;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
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
            return string.Format("$<subr {0}>", mName);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
