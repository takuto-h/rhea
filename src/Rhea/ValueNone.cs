using System.Collections.Generic;

namespace Rhea
{
    public class ValueNone : IValue
    {
        private static KlassHolder smKlassHolder;
        private static ValueNone smInstance;
        
        static ValueNone()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.None,
                    Klasses.Object
                }
            );
        }
        
        public static ValueNone Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new ValueNone();
                }
                return smInstance;
            }
        }
        
        private ValueNone()
        {
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return "$<none>";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
