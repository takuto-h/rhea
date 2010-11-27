using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public class ValueArray : IValue
    {
        private static KlassHolder smKlassHolder;
        
        public IList<IValue> Value { get; private set; }
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueArray()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Array,
                    Klasses.Object
                }
            );
        }
        
        public ValueArray(IList<IValue> value)
        {
            Value = value;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            if (Value.Count == 0)
            {
                return "[]";
            }
            return string.Format(
                "[{0}]",
                Value.Skip(1).Aggregate(
                    Value[0].ToString(),
                    (acc, elem) => string.Format("{0}, {1}", acc, elem)
                )
            );
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
