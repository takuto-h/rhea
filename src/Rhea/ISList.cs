namespace Rhea
{
    public interface ISList<T>
    {
        T Head { get; }
        ISList<T> Tail { get; }
        bool IsNil();
    }
}
