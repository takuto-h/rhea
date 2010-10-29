namespace Rhea
{
    public class Compiler
    {
        private ISList<IInsn> mInsns;
        
        public Compiler()
        {
            mInsns = SList.Nil<IInsn>();
        }
        
        public Compiler(Compiler compiler) : this()
        {
        }
        
        public void Push(IInsn insn)
        {
            mInsns = SList.Cons<IInsn>(insn, mInsns);
        }
        
        public ISList<IInsn> GetResult()
        {
            return mInsns.Reverse();
        }
    }
}
