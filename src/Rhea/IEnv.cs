namespace Rhea
{
    public interface IEnv
    {
        IEnv OuterEnv { get; }
        bool IsGlobal();
        IValue this[ValueSymbol symbol] { get; set; }
        IValue this[ValueSymbol klass, ValueSymbol selector] { get; set; }
        void AddVariable(ValueSymbol symbol, IValue value);
        void AddMethod(ValueSymbol klass, ValueSymbol selector, IValue value);
        bool TryGetVariable(ValueSymbol symbol, out IValue value);
        bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValue value);
        bool ContainsVariable(ValueSymbol symbol);
        bool ContainsMethod(ValueSymbol klass, ValueSymbol selector);
    }
}
