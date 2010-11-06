namespace Rhea
{
    public class ValueString : IValue
    {
        private static ValueSymbol smKlass;
        
        public string StringValue { get; private set; }
        
        public static ValueSymbol GetKlass()
        {
            if (smKlass == null)
            {
                smKlass = ValueSymbol.Generate("String");
            }
            return smKlass;
        }
        
        public ValueSymbol Klass
        {
            get { return GetKlass(); }
        }
        
        public ValueString(string stringValue)
        {
            StringValue = stringValue;
        }
        
        public string Show()
        {
            return string.Format("\"{0}\"", StringValue);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
