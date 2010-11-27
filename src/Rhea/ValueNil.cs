using System.Collections.Generic;

namespace Rhea
{
    public class ValueNil : IValue
    {
        private static KlassHolder smKlassHolder;
        private static ValueNil smInstance;
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueNil()
        {
            smKlassHolder = new KlassHolder(
                new List<ValueSymbol> {
                    Klasses.Nil,
                    Klasses.Object
                }
            );
        }
        
        public static ValueNil Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new ValueNil();
                }
                return smInstance;
            }
        }
        
        private ValueNil()
        {
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return "nil";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
