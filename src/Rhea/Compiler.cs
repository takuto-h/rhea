using System;

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
        
        public IInsn Peek()
        {
            if (mInsns.IsNil())
            {
                throw new InvalidOperationException("Compiler: Insns is empty.");
            }
            return mInsns.Head;
        }
        
        public IInsn Pop()
        {
            IInsn insn = Peek();
            mInsns = mInsns.Tail;
            return insn;
        }
        
        public ISList<IInsn> GetResult()
        {
            return mInsns.Reverse();
        }
    }
}
