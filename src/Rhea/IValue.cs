namespace Rhea
{
    public interface IValue : IShowable
    {
        ValueSymbol Klass { get; }
    }
}
