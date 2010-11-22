using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public class ValueHash : IValue
    {
        private static KlassHolder smKlassHolder;
        
        public IDictionary<IValue, IValue> HashValue { get; private set; }
        
        static ValueHash()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Hash,
                    Klasses.Object
                }
            );
        }
        
        public ValueHash(IDictionary<IValue, IValue> hashValue)
        {
            HashValue = hashValue;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            if (HashValue.Count == 0)
            {
                return "{}";
            }
            return string.Format(
                "{{{0}}}",
                HashValue.Skip(1).Aggregate(
                    ShowKeyValuePair(HashValue.ElementAt(0)),
                    (acc, elem) => string.Format("{0}, {1}", acc, ShowKeyValuePair(elem))
                )
            );
        }
        
        private string ShowKeyValuePair(KeyValuePair<IValue, IValue> kvp)
        {
            return string.Format("{0}=>{1}", kvp.Key, kvp.Value);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
