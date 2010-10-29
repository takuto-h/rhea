namespace Rhea
{
    public class SListCons<T> : ISList<T>
    {
        public T Head { get; private set; }
        public ISList<T> Tail { get; private set; }
        
        public SListCons(T head, ISList<T> tail)
        {
            Head = head;
            Tail = tail;
        }
        
        public bool IsNil()
        {
            return false;
        }
        
        public override string ToString()
        {
            return this.Show();
        }
    }
}
