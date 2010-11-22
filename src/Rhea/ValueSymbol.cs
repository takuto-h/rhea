using System.Collections.Generic;

namespace Rhea
{
    public class ValueSymbol : IValue
    {
        private static KlassHolder smKlassHolder;
        private static IDictionary<string, ValueSymbol> smInstances;
        
        private bool mInterned;
        
        public string Name { get; private set; }
        
        public IList<ValueSymbol> KlassList
        {
            get { return smKlassHolder.KlassList; }
        }
        
        static ValueSymbol()
        {
            smKlassHolder = null;
            smInstances = new Dictionary<string, ValueSymbol>();
        }
        
        private ValueSymbol(string name, bool interned)
        {
            mInterned = interned;
            Name = name;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            if (smKlassHolder == null)
            {
                smKlassHolder = new KlassHolder(
                    new List<ValueSymbol> {
                        Klasses.Symbol,
                        Klasses.Object
                    }
                );
            }
            smKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public static ValueSymbol Intern(string name)
        {
            ValueSymbol symbol;
            if (!smInstances.TryGetValue(name, out symbol))
            {
                symbol = new ValueSymbol(name, true);
                smInstances[name] = symbol;
            }
            return symbol;
        }
        
        public static ValueSymbol Generate(string name)
        {
            return new ValueSymbol(name, false);
        }
        
        public string Show()
        {
            if (!mInterned)
            {
                return string.Format("$:{0}", Name.ToIdentifier());
            }
            return string.Format(":{0}", Name.ToIdentifier());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
