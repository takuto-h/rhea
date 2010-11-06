namespace Rhea
{
    public class ValueFalse : IValue
    {
        private static ValueSymbol smKlass;
        private static ValueFalse smInstance;
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("False");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
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
        
        public string Show()
        {
            return "$<false>";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
