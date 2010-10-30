using System;
using System.Collections.Generic;

namespace Rhea
{
    public class VM
    {
        private ISList<IInsn> mInsns;
        private ISList<IValue> mStack;
        private ISList<KeyValuePair<IValueFunc, IValueFunc>> mWinders;
        
        public IEnv Env { get; private set; }
        
        public VM(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValueFunc, IValueFunc>> winders
        )
        {
            mInsns = insns;
            mStack = stack;
            Env = env;
            mWinders = winders;
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
        
        public ValueCont GetDynamicContext()
        {
            return new ValueCont(mInsns, mStack, Env, mWinders);
        }
        
        public void SetDynamicContext(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValueFunc, IValueFunc>> winders
        )
        {
            mInsns = insns;
            mStack = stack;
            Env = env;
            mWinders = winders;
        }
        
        public void SetStaticContext(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env
        )
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
