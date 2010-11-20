namespace Rhea
{
    public interface IEnv
    {
        IEnv OuterEnv { get; }
        bool IsGlobal();
        IValue this[ValueSymbol symbol] { get; set; }
        IValueFunc this[ValueSymbol klass, ValueSymbol selector] { get; set; }
        void AddVariable(ValueSymbol symbol, IValue value);
        void AddMethod(ValueSymbol klass, ValueSymbol selector, IValueFunc func);
        bool TryGetVariable(ValueSymbol symbol, out IValue value);
        bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValueFunc func);
        bool ContainsVariable(ValueSymbol symbol);
        bool ContainsMethod(ValueSymbol klass, ValueSymbol selector);
    }
}
