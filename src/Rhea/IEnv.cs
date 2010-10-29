namespace Rhea
{
    public interface IEnv
    {
        void AddVariable(ValueSymbol selector, IValue value);
        void AddMethod(ValueSymbol klass, ValueSymbol selector, IValue value);
        bool TryGetVariable(ValueSymbol selector, out IValue value);
        bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValue value);
        bool ContainsVariable(ValueSymbol selector);
        bool ContainsMethod(ValueSymbol klass, ValueSymbol selector);
    }
}
