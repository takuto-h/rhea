using System.Collections.Generic;

namespace Rhea
{
    public class ValueSymbol : IValue
    {
        private static IDictionary<string, ValueSymbol> smInstances;
        
        public string Name { get; private set; }
        
        public ValueSymbol Klass
        {
            get { return ValueSymbol.Intern("Symbol"); }
        }
        
        static ValueSymbol()
        {
            smInstances = new Dictionary<string, ValueSymbol>();
        }
        
        private ValueSymbol(string name)
        {
            Name = name;
        }
        
        public static ValueSymbol Intern(string name)
        {
            ValueSymbol symbol;
            if (!smInstances.TryGetValue(name, out symbol))
            {
                symbol = new ValueSymbol(name);
                smInstances[name] = symbol;
            }
            return symbol;
        }
        
        public string Show()
        {
            return string.Format(":{0}", Name);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
