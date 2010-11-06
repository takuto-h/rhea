using System;
using System.Collections.Generic;

namespace Rhea
{
    public class EnvDefault : EnvGlobal
    {
        public EnvDefault()
        {
            this.AddVariable("p", 1, P);
            this.AddVariable("dynamic_wind", 3, DynamicWind);
            this.AddVariable("callcc", 1, Callcc);
            this.AddVariable("make_symbol", 1, MakeSymbol);
            this.AddVariable("make_object", 1, MakeObject);
            this.AddVariable("get_slot", 2, GetSlot);
            this.AddVariable("set_slot", 3, SetSlot);
            this.AddVariable("Closure", ValueClosure.GetKlass());
            this.AddVariable("Cont", ValueCont.GetKlass());
            this.AddVariable("Int", ValueInt.GetKlass());
            this.AddVariable("String", ValueString.GetKlass());
            this.AddVariable("Subr", ValueSubr.GetKlass());
            this.AddVariable("Symbol", ValueSymbol.GetKlass());
        }
        
        private static void P(IList<IValue> args, VM vm, SourceInfo info)
        {
            Console.WriteLine(args[0]);
            vm.Push(args[0]);
        }
        
        private static void DynamicWind(IList<IValue> args, VM vm, SourceInfo info)
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
        
        private static void Callcc(IList<IValue> args, VM vm, SourceInfo info)
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
        
        private static void MakeSymbol(IList<IValue> args, VM vm, SourceInfo info)
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
        
        private static void MakeObject(IList<IValue> args, VM vm, SourceInfo info)
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
        
        private static void GetSlot(IList<IValue> args, VM vm, SourceInfo info)
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
        
        private static void SetSlot(IList<IValue> args, VM vm, SourceInfo info)
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
