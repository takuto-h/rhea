using System;

namespace Rhea
{
    public class SListNil<T> : ISList<T>
    {
        private static ISList<T> smInstance;
        
        public static ISList<T> Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new SListNil<T>();
                }
                return smInstance;
            }
        }
        
        public T Head
        {
            get { throw new NotSupportedException(); }
        }
        
        public ISList<T> Tail
        {
            get { throw new NotSupportedException(); }
        }
        
        private SListNil()
        {
        }
        
        public bool IsNil()
        {
            return true;
        }
        
        public override string ToString()
        {
            return this.Show();
        }
    }
}
