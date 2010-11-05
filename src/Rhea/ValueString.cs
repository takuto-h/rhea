namespace Rhea
{
    public class ValueString : IValue
    {
        private static ValueSymbol smKlass;
        
        private string mStringValue;
        
        public ValueSymbol Klass
        {
            get
            {
                if (smKlass == null)
                {
                    smKlass = ValueSymbol.Generate("String");
                }
                return smKlass;
            }
        }
        
        public ValueString(string stringValue)
        {
            mStringValue = stringValue;
        }
        
        public string Show()
        {
            return string.Format("\"{0}\"", mStringValue);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
