using System.Collections.Generic;

namespace Rhea
{
    public class ValueFalse : IValue
    {
        private static KlassHolder smKlassHolder;
        private static ValueFalse smInstance;
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueFalse()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.False,
                    Klasses.Object
                }
            );
        }
        
        public static ValueFalse Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new ValueFalse();
                }
                return smInstance;
            }
        }
        
        private ValueFalse()
        {
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return "false";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
