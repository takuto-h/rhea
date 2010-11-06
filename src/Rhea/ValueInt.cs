namespace Rhea
{
    public class ValueInt : IValue
    {
        private static ValueSymbol smKlass;
        
        public int IntValue { get; private set; }
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("Int");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
        }
        
        public ValueInt(int intValue)
        {
            IntValue = intValue;
        }
        
        public string Show()
        {
            return IntValue.ToString();
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
