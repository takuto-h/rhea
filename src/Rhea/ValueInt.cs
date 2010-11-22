using System.Collections.Generic;

namespace Rhea
{
    public class ValueInt : IValue
    {
        private static KlassHolder smKlassHolder;
        
        public int Value { get; private set; }
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueInt()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Int,
                    Klasses.Object
                }
            );
        }
        
        public ValueInt(int value)
        {
            Value = value;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return Value.ToString();
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
