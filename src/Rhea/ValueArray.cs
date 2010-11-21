using System.Collections.Generic;
using System.Linq;

namespace Rhea
{
    public class ValueArray : IValue
    {
        private static KlassHolder smKlassHolder;
        
        private IList<IValue> mArrayValue;
        
        static ValueArray()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Array,
                    Klasses.Object
                }
            );
        }
        
        public ValueArray(IList<IValue> arrayValue)
        {
            mArrayValue = arrayValue;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            if (mArrayValue.Count == 0)
            {
                return "[]";
            }
            return string.Format(
                "[{0}]",
                mArrayValue.Skip(1).Aggregate(
                    mArrayValue[0].ToString(),
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
