using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Array
    {
        private static void Check(Arguments args, int count, SourceInfo info)
        {
            for (int i = 0; i < count; i++)
            {
                if (!(args[i] is ValueArray))
                {
                    throw new RheaException(
                        string.Format("array required, but got {0}", args[i]),
                        info
                    );
                }
            }
        }
        
        public static void MakeArray(Arguments args, VM vm, SourceInfo info)
        {
            vm.Push(new ValueArray(args.List));
        }
        
        public static void GetItem(Arguments args, VM vm, SourceInfo info)
        {
            Check(args, 1, info);
            ValueArray array = (ValueArray)args[0];
            ValueInt index = args[1] as ValueInt;
            if (index == null)
            {
                throw new RheaException(
                    string.Format("int required, but got {0}", args[1]),
                    info
                );
            }
            if (index.Value >= array.Value.Count)
            {
                throw new RheaException(
                    string.Format("out of range for {0}: {1}", array, index),
                    info
                );
            }
            vm.Push(array.Value[index.Value]);
        }
        
        public static void SetItem(Arguments args, VM vm, SourceInfo info)
        {
            Check(args, 1, info);
            ValueArray array = (ValueArray)args[0];
            ValueInt index = args[1] as ValueInt;
            IValue value = args[2];
            if (index == null)
            {
                throw new RheaException(
                    string.Format("int required, but got {0}", args[1]),
                    info
                );
            }
            if (index.Value >= array.Value.Count)
            {
                throw new RheaException(
                    string.Format("out of range for {0}: {1}", array, index),
                    info
                );
            }
            vm.Push(array.Value[index.Value] = value);
        }
    }
}
