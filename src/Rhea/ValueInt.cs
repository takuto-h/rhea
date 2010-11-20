using System.Collections.Generic;

namespace Rhea
{
    public class ValueInt : IValue
    {
        private static KlassHolder smKlassHolder;
        
        public int IntValue { get; private set; }
        
        static ValueInt()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Int,
                    Klasses.Object
                }
            );
        }
        
        public ValueInt(int intValue)
        {
            IntValue = intValue;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return IntValue.ToString();
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
