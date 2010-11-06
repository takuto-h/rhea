using System.Collections.Generic;

namespace Rhea
{
    public class ValueObject : IValue
    {
        private Dictionary<ValueSymbol, IValue> mSlots;
        
        public ValueSymbol Klass { get; private set; }
        
        public ValueObject(ValueSymbol klass)
        {
            mSlots = new Dictionary<ValueSymbol, IValue>();
            Klass = klass;
        }
        
        public bool TryGetSlot(ValueSymbol symbol, out IValue value)
        {
            return mSlots.TryGetValue(symbol, out value);
        }
        
        public IValue SetSlot(ValueSymbol symbol, IValue value)
        {
            return mSlots[symbol] = value;
        }
        
        public string Show()
        {
            return string.Format("$<object {0}:0x{1:x8}>", Klass.Name, GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
