using System;

namespace Rhea
{
    public class VM
    {
        private ISList<IInsn> mInsns;
        private ISList<IValue> mStack;
        
        public IEnv Env { get; private set; }
        
        public VM(ISList<IInsn> insns, ISList<IValue> stack, IEnv env)
        {
            mInsns = insns;
            mStack = stack;
            Env = env;
        }
        
        public IValue Run()
        {
            while (!mInsns.IsNil())
            {
                IInsn insn = mInsns.Head;
                mInsns = mInsns.Tail;
                insn.Execute(this);
            }
            return Peek();
        }
        
        public ValueCont GetCont()
        {
            return new ValueCont(mInsns, mStack, Env);
        }
        
        public void SetCont(ISList<IInsn> insns, ISList<IValue> stack, IEnv env)
        {
            mInsns = insns;
            mStack = stack;
            Env = env;
        }
        
        public void Push(IValue value)
        {
            mStack = SList.Cons<IValue>(value, mStack);
        }
        
        public IValue Peek()
        {
            if (mStack.IsNil())
            {
                throw new InvalidOperationException("VM: Stack is empty.");
            }
            return mStack.Head;
        }
        
        public IValue Pop()
        {
            IValue value = Peek();
            mStack = mStack.Tail;
            return value;
        }
    }
}
