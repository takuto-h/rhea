using System;
using System.Collections.Generic;

namespace Rhea
{
    public class VM
    {
        public ISList<IInsn> Insns { get; set; }
        public ISList<IValue> Stack { get; set; }
        public IEnv Env { get; set; }
        public ISList<KeyValuePair<IValue, IValue>> Winders { get; set; }
        
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
            Winders = winders;
        }
        
        public IValue Run()
        {
            while (!Insns.IsNil())
            {
                IInsn insn = Insns.Head;
                Insns = Insns.Tail;
                //Console.WriteLine("insn: {0}", insn);
                insn.Execute(this);
                //Console.WriteLine("stack: {0}", Stack);
            }
            return Peek();
        }
        
        public ValueCont GetCont()
        {
            return new ValueCont(Insns, Stack, Env, Winders);
        }
        
        public void SetCont(IValue returnValue, ValueCont cont, SourceInfo info)
        {
            var winders = cont.Winders;
            if (winders == Winders)
            {
                Insns = cont.Insns;
                Stack = cont.Stack;
                Env = cont.Env;
                Push(returnValue);
            }
            else if (winders.ContainsList(Winders))
            {
                var newWinders = winders.GetPreviousList(Winders);
                IValue before = newWinders.Head.Key;
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnPush(before));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnSetWinders(newWinders));
                insnStack.Push(new InsnPush(cont));
                insnStack.Push(new InsnPush(returnValue));
                insnStack.Push(new InsnCall(1, info));
                Insns = insnStack.ToSList();
                Stack = SList.Nil<IValue>();
            }
            else
            {
                IValue after = Winders.Head.Value;
                Winders = Winders.Tail;
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
    }
}
