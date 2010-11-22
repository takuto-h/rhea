using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public delegate void Subr(Arguments args, VM vm, SourceInfo info);
    
    public class ValueSubr : IValueFunc
    {
        private static KlassHolder smKlassHolder;
        
        private string mName;
        private int mParamCount;
        private bool mAllowRest;
        private bool mAllowOtherKeys;
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
            string name,
            Subr subrValue,
            int paramCount,
            bool allowRest,
            bool allowOtherKeys
        )
        {
            mName = name;
            mParamCount = paramCount;
            mAllowRest = allowRest;
            mAllowOtherKeys = allowOtherKeys;
            mSubrValue = subrValue;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public void Call(IList<IValue> args, VM vm, SourceInfo info)
        {
            int argCount = args.Count;
            if (argCount < mParamCount ||
                argCount > mParamCount && !mAllowRest && !mAllowOtherKeys)
            {
                throw new RheaException(
                    this.WrongNumberOfArguments(mParamCount, argCount), info
                );
            }
            var dict = new Dictionary<IValue, IValue>();
            if (mAllowOtherKeys)
            {
                foreach (IValue arg in args.Skip(mParamCount))
                {
                    ValueArray pair = arg as ValueArray;
                    if (pair == null || pair.Count != 2)
                    {
                        throw new RheaException(
                            "keyword arguments should occur pairwise", info
                        );
                    }
                    dict.Add(pair[0], pair[1]);
                }
            }
            var newArgs = new Arguments(args, dict);
            mSubrValue(newArgs, vm, info);
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
