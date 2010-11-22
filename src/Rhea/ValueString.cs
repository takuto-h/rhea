using System.Collections.Generic;

namespace Rhea
{
    public class ValueString : IValue
    {
        private static KlassHolder smKlassHolder;
        
        public string Value { get; private set; }
        
        static ValueString()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.String,
                    Klasses.Object
                }
            );
        }
        
        public ValueString(string value)
        {
            Value = value;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return string.Format("\"{0}\"", Value);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
