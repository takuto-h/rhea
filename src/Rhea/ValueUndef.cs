using System.Collections.Generic;

namespace Rhea
{
    public class ValueUndef : IValue
    {
        private static KlassHolder smKlassHolder;
        private static ValueUndef smInstance;
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueUndef()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.UndefinedObject,
                    Klasses.Object
                }
            );
        }
        
        public static ValueUndef Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new ValueUndef();
                }
                return smInstance;
            }
        }
        
        private ValueUndef()
        {
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return "$<undef>";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
