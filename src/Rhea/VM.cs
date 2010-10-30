using System;
using System.Collections.Generic;

namespace Rhea
{
    public class VM
    {
        public ISList<IInsn> Insns { get; set; }
        public ISList<IValue> Stack { get; set; }
        public IEnv Env { get; set; }
        
        private ISList<KeyValuePair<IValue, IValue>> mWinders;
        
        public VM(
            ISList<IInsn> insns,
            ISList<IValue> stack,
            IEnv env,
            ISList<KeyValuePair<IValue, IValue>> winders
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
                try
                {
                    //Console.WriteLine("insn: {0}", insn);
                    insn.Execute(this);
                    //Console.WriteLine("stack: {0}", Stack);
                }
                catch (RheaException e)
                {
                    Console.WriteLine("{0}: {1}", e.Info, e.Message);
                    ValueCont cont = new ValueCont(
                        SList.Nil<IInsn>(),
                        SList.Nil<IValue>(),
                        Env,
                        SList.Nil<KeyValuePair<IValue, IValue>>()
                    );
                    SetCont(null, cont, e.Info);
                }
            }
            return Peek();
        }
        
        public ValueCont GetCont()
        {
            return new ValueCont(Insns, Stack, Env, mWinders);
        }
        
        public void SetCont(IValue returnValue, ValueCont cont, SourceInfo info)
        {
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
                IValue before = winder.Key;
                IValue after = winder.Value;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnPush(before));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnPushWinder(before, after));
                insnStack.Push(new InsnPush(cont));
                insnStack.Push(new InsnPush(returnValue));
                insnStack.Push(new InsnCall(1, info));
                Insns = insnStack.ToSList();
                Stack = SList.Nil<IValue>();
            }
            else
            {
                IValue after = mWinders.Head.Value;
                mWinders = mWinders.Tail;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnPush(after));
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
        
        public void PushWinder(IValue before, IValue after)
        {
            mWinders = SList.Cons(
                new KeyValuePair<IValue, IValue>(before, after), mWinders
            );
        }
    }
}
