namespace Rhea
{
    public class ValueString : IValue
    {
        private string mStringValue;
        
        public ValueSymbol Klass
        {
            get { return ValueSymbol.Intern("String"); }
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
