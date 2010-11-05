namespace Rhea
{
    public class ValueInt : IValue
    {
        private static ValueSymbol smKlass;
        
        private int mIntValue;
        
        public ValueSymbol Klass
        {
            get
            {
                if (smKlass == null)
                {
                    smKlass = ValueSymbol.Generate("Int");
                }
                return smKlass;
            }
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
