namespace Rhea
{
    public class ValueObject : IValue
    {
        public ValueSymbol Klass { get; private set; }
        
        public ValueObject(ValueSymbol klass)
        {
            Klass = klass;
        }
        
        public string Show()
        {
            return string.Format("$<object {0}:0x{1:x8}>", Klass.Name, GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
