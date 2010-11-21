using System.Collections.Generic;

namespace Rhea
{
    public class ValueInstance : IValue
    {
        private KlassHolder mKlassHolder;
        private Dictionary<ValueSymbol, IValueFunc> mSlots;
        
        public ValueInstance(
            IList<ValueSymbol> klasses,
            Dictionary<ValueSymbol, IValueFunc> slots
        )
        {
            mKlassHolder = new KlassHolder(klasses);
            mSlots = slots;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            IValueFunc func;
            if (mSlots.TryGetValue(selector, out func))
            {
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
            return string.Format("$<instance 0x{0:x8}>", mKlassHolder, GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}