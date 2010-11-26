using System.Collections.Generic;

namespace Rhea
{
    public class ValueTrue : IValueBool
    {
        private static KlassHolder smKlassHolder;
        private static ValueTrue smInstance;
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueTrue()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.True,
                    Klasses.Bool,
                    Klasses.Object
                }
            );
        }
        
        public static ValueTrue Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new ValueTrue();
                }
                return smInstance;
            }
        }
        
        private ValueTrue()
        {
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return "true";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
