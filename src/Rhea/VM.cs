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
            IValue returnValue,
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValueFunc, IValueFunc>> winders,
            SourceInfo info
        )
        {
            /*先頭があるか
            なかったら、
            　　そいつを除いたmWindersのもとでoutを実行、
            　　windersを含む継続を呼ぶ
            あったら、
            　　windersの上層に変なものがないか探す
            　　なかったら、
            　　　　SetStaticContext
            　　あったら、
　　            　　mWindersのもとでwindersの下層にあるinを実行
    　　        　　mWindersに下層を載せる
        　　    　　windersを含む継続を呼ぶ*/
            if (winders == mWinders)
            {
                SetStaticContext(insns, stack, env);
                Push(returnValue);
            }
            else if (winders.ContainsSList(mWinders))
            {
                var winder = winders.GetPreviousElementOf(mWinders);
                IValueFunc func = winder.Key;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                IValue cont = new ValueCont(insns, stack, env, winders);
                insnStack.Push(new InsnPush(func));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnPushWinder(winder));
                insnStack.Push(new InsnPush(cont));
                insnStack.Push(new InsnPush(returnValue));
                insnStack.Push(new InsnCall(1, info));
                mInsns = insnStack.ToSList();
                mStack = SList.Nil<IValue>();
            }
            else
            {
                IValueFunc func = mWinders.Head.Value;
                mWinders = mWinders.Tail;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                IValue cont = new ValueCont(insns, stack, env, winders);
                insnStack.Push(new InsnPush(func));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnPush(cont));
                insnStack.Push(new InsnPush(returnValue));
                insnStack.Push(new InsnCall(1, info));
                mInsns = insnStack.ToSList();
                mStack = SList.Nil<IValue>();
            }
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
        
        public void PushWinder(KeyValuePair<IValueFunc, IValueFunc> winder)
        {
            mWinders = SList.Cons<KeyValuePair<IValueFunc, IValueFunc>>(winder, mWinders);
        }
    }
}
