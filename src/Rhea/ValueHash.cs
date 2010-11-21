using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public class ValueHash : IValue
    {
        private static KlassHolder smKlassHolder;
        
        private IDictionary<IValue, IValue> mHashValue;
        
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
            mHashValue = hashValue;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            if (mHashValue.Count == 0)
            {
                return "{}";
            }
            return string.Format(
                "{{{0}}}",
                mHashValue.Skip(1).Aggregate(
                    ShowKeyValuePair(mHashValue.ElementAt(0)),
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
