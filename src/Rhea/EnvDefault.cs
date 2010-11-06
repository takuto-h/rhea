using System;
using System.Collections.Generic;

namespace Rhea
{
    public class EnvDefault : EnvGlobal
    {
        public EnvDefault()
        {
            this.AddVariable("p", 1, (args, vm, info) => {
                Console.WriteLine(args[0]);
                vm.Push(args[0]);
            });
            this.AddVariable("dynamic_wind", 3, (args, vm, info) => {
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
            });
            this.AddVariable("callcc", 1, (args, vm, info) => {
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
            });
            this.AddVariable("make_class", 1, (args, vm, info) => {
                ValueString str = args[0] as ValueString;
                if (str == null)
                {
                    throw new RheaException(
                        string.Format("string required, but got {0}", args[0]),
                        info
                    );
                }
                vm.Push(ValueSymbol.Generate(str.StringValue));
            });
            this.AddVariable("Closure", ValueClosure.GetKlass());
            this.AddVariable("Cont", ValueCont.GetKlass());
            this.AddVariable("Int", ValueInt.GetKlass());
            this.AddVariable("String", ValueString.GetKlass());
            this.AddVariable("Subr", ValueSubr.GetKlass());
            this.AddVariable("Symbol", ValueSymbol.GetKlass());
        }
    }
}
