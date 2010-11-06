using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Int
    {
        private static void Check(IList<IValue> args, int count, SourceInfo info)
        {
            for (int i = 0; i < count; i++)
            {
                if (!(args[i] is ValueInt))
                {
                    throw new RheaException(
                        string.Format("int required, but got {0}", args[i]),
                        info
                    );
                }
            }
        }
        
        public static void Add(IList<IValue> args, VM vm, SourceInfo info)
        {
            Check(args, 2, info);
            ValueInt integer1 = (ValueInt)args[0];
            ValueInt integer2 = (ValueInt)args[1];
            ValueInt newInteger = new ValueInt(
                integer1.IntValue + integer2.IntValue
            );
            vm.Push(newInteger);
        }
        
        public static void Sub(IList<IValue> args, VM vm, SourceInfo info)
        {
            Check(args, 2, info);
            ValueInt integer1 = (ValueInt)args[0];
            ValueInt integer2 = (ValueInt)args[1];
            ValueInt newInteger = new ValueInt(
                integer1.IntValue - integer2.IntValue
            );
            vm.Push(newInteger);
        }
        
        public static void Mul(IList<IValue> args, VM vm, SourceInfo info)
        {
            Check(args, 2, info);
            ValueInt integer1 = (ValueInt)args[0];
            ValueInt integer2 = (ValueInt)args[1];
            ValueInt newInteger = new ValueInt(
                integer1.IntValue * integer2.IntValue
            );
            vm.Push(newInteger);
        }
    }
}