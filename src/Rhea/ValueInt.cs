namespace Rhea
{
    public class ValueInt : IValue
    {
        private int mIntValue;
        
        public ValueSymbol Klass
        {
            get { return ValueSymbol.Intern("Int"); }
        }
        
        public ValueInt(int intValue)
        {
            mIntValue = intValue;
        }
        
        public string Show()
        {
            return mIntValue.ToString();
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
