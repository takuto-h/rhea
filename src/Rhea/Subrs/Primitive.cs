using System;
using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Primitive
    {
        public static void P(IList<IValue> args, VM vm, SourceInfo info)
        {
            Console.WriteLine(args[0]);
            vm.Push(args[0]);
        }
        
        public static void DynamicWind(IList<IValue> args, VM vm, SourceInfo info)
        {
            ValueCont cont = vm.GetCont();
            var winder = new KeyValuePair<IValue, IValue>(args[0], args[2]);
            var winders = SList.Cons(winder, vm.Winders);
            Stack<IInsn> insnStack = new Stack<IInsn>();
            insnStack.Push(new InsnCall(0, info));
            insnStack.Push(InsnPop.Instance);
            insnStack.Push(new InsnSetWinders(winders));
            insnStack.Push(new InsnPush(args[1]));
            insnStack.Push(new InsnCall(0, info));
            insnStack.Push(new InsnCall(1, info));
            vm.Insns = insnStack.ToSList();
            vm.Stack = SList.List<IValue>(args[0], cont);
        }
        
        public static void Callcc(IList<IValue> args, VM vm, SourceInfo info)
        {
            IValueFunc func = args[0] as IValueFunc;
            if (func == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", args[0]),
                    info
                );
            }
            ValueCont cont = vm.GetCont();
            IList<IValue> newArgs = new List<IValue> { cont };
            func.Call(newArgs, vm, info);
        }
        
        public static void MakeSymbol(IList<IValue> args, VM vm, SourceInfo info)
        {
            ValueString str = args[0] as ValueString;
            if (str == null)
            {
                throw new RheaException(
                    string.Format("string required, but got {0}", args[0]),
                    info
                );
            }
            vm.Push(ValueSymbol.Generate(str.StringValue));
        }
        
        public static void MakeObject(IList<IValue> args, VM vm, SourceInfo info)
        {
            ValueSymbol klass = args[0] as ValueSymbol;
            if (klass == null)
            {
                throw new RheaException(
                    string.Format("symbol required, but got {0}", args[0]),
                    info
                );
            }
            vm.Push(new ValueObject(klass));
        }
        
        public static void GetSlot(IList<IValue> args, VM vm, SourceInfo info)
        {
            ValueObject obj = args[0] as ValueObject;
            ValueSymbol symbol = args[1] as ValueSymbol;
            if (obj == null)
            {
                throw new RheaException(
                    string.Format("object required, but got {0}", args[0]),
                    info
                );
            }
            if (symbol == null)
            {
                throw new RheaException(
                    string.Format("symbol required, but got {0}", args[1]),
                    info
                );
            }
            IValue value;
            if (!obj.TryGetSlot(symbol, out value))
            {
                throw new RheaException(
                    string.Format("slot is not defined in {0}: {1}", obj, symbol.Name),
                    info
                );
            }
            vm.Push(value);
        }
        
        public static void SetSlot(IList<IValue> args, VM vm, SourceInfo info)
        {
            ValueObject obj = args[0] as ValueObject;
            ValueSymbol symbol = args[1] as ValueSymbol;
            if (obj == null)
            {
                throw new RheaException(
                    string.Format("object required, but got {0}", args[0]),
                    info
                );
            }
            if (symbol == null)
            {
                throw new RheaException(
                    string.Format("symbol required, but got {0}", args[1]),
                    info
                );
            }
            vm.Push(obj.SetSlot(symbol, args[2]));
        }
    }
}
