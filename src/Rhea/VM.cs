using System;
using System.Collections.Generic;

namespace Rhea
{
    public class VM
    {
        public ISList<IInsn> Insns { get; set; }
        public ISList<IValue> Stack { get; set; }
        public IEnv Env { get; set; }
        
        private ISList<KeyValuePair<IValueFunc, IValueFunc>> mWinders;
        
        public VM(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValueFunc, IValueFunc>> winders
        )
        {
            Insns = insns;
            Stack = stack;
            Env = env;
            mWinders = winders;
        }
        
        public IValue Run()
        {
            while (!Insns.IsNil())
            {
                IInsn insn = Insns.Head;
                Insns = Insns.Tail;
                insn.Execute(this);
            }
            return Peek();
        }
        
        public ValueCont GetCont()
        {
            return new ValueCont(Insns, Stack, Env, mWinders);
        }
        
        public void SetCont(IValue returnValue, ValueCont cont, SourceInfo info)
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
            var winders = cont.Winders;
            if (winders == mWinders)
            {
                Insns = cont.Insns;
                Stack = cont.Stack;
                Env = cont.Env;
                Push(returnValue);
            }
            else if (winders.ContainsSList(mWinders))
            {
                var winder = winders.GetPreviousElementOf(mWinders);
                IValueFunc func = winder.Key;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnPush(func));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnPushWinder(winder));
                insnStack.Push(new InsnPush(cont));
                insnStack.Push(new InsnPush(returnValue));
                insnStack.Push(new InsnCall(1, info));
                Insns = insnStack.ToSList();
                Stack = SList.Nil<IValue>();
            }
            else
            {
                IValueFunc func = mWinders.Head.Value;
                mWinders = mWinders.Tail;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnPush(func));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnPush(cont));
                insnStack.Push(new InsnPush(returnValue));
                insnStack.Push(new InsnCall(1, info));
                Insns = insnStack.ToSList();
                Stack = SList.Nil<IValue>();
            }
        }
        
        public void Push(IValue value)
        {
            Stack = SList.Cons<IValue>(value, Stack);
        }
        
        public IValue Peek()
        {
            if (Stack.IsNil())
            {
                throw new InvalidOperationException("VM: Stack is empty.");
            }
            return Stack.Head;
        }
        
        public IValue Pop()
        {
            IValue value = Peek();
            Stack = Stack.Tail;
            return value;
        }
        
        public void PushWinder(KeyValuePair<IValueFunc, IValueFunc> winder)
        {
            mWinders = SList.Cons<KeyValuePair<IValueFunc, IValueFunc>>(winder, mWinders);
        }
    }
}
