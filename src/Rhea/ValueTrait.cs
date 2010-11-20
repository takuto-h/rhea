using System.Collections.Generic;

namespace Rhea
{
    public class ValueTrait : IValue
    {
        private KlassHolder mKlassHolder;
        private Dictionary<ValueSymbol, IValue> mSlots;
        
        public ValueTrait(
            IList<ValueSymbol> klasses,
            Dictionary<ValueSymbol, IValue> slots
        )
        {
            mKlassHolder = new KlassHolder(klasses);
            mSlots = slots;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            IValue value;
            if (mSlots.TryGetValue(selector, out value))
            {
                IValueFunc func = value as IValueFunc;
                if (func == null)
                {
                    throw new RheaException(
                        string.Format("function required, but got {0}", value), info
                    );
                }
                List<IValue> newArgs = new List<IValue>();
                newArgs.Add(this);
                newArgs.AddRange(args);
                func.Call(newArgs, vm, info);
                return;
            }
            mKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return string.Format("$<trait 0x{1:x8}>", GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
