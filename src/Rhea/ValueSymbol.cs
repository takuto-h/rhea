using System.Collections.Generic;

namespace Rhea
{
    public class ValueSymbol : IValue
    {
        private static ValueSymbol smKlass;
        
        private static IDictionary<string, ValueSymbol> smInstances;
        
        public string Name { get; private set; }
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("Symbol");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
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
        
        public static ValueSymbol Generate(string name)
        {
            return new ValueSymbol(name);
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
