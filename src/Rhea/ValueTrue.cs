namespace Rhea
{
    public class ValueTrue : IValueBool
    {
        private static ValueSymbol smKlass;
        private static ValueTrue smInstance;
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("True");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
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
        
        public string Show()
        {
            return "$<true>";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
