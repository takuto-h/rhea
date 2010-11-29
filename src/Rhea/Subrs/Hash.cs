using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Hash
    {
        private static void Check(Arguments args, int count, SourceInfo info)
        {
            for (int i = 0; i < count; i++)
            {
                if (!(args[i] is ValueHash))
                {
                    throw new RheaException(
                        string.Format("hash required, but got {0}", args[i]),
                        info
                    );
                }
            }
        }
        
        public static void MakeHash(Arguments args, VM vm, SourceInfo info)
        {
            vm.Push(new ValueHash(args.Dict));
        }
        
        public static void GetItem(Arguments args, VM vm, SourceInfo info)
        {
            Check(args, 1, info);
            ValueHash hash = (ValueHash)args[0];
            IValue key = args[1];
            if (!hash.Value.ContainsKey(key))
            {
                throw new RheaException(
                    string.Format("key not found for {0}: {1}", hash, key),
                    info
                );
            }
            vm.Push(hash.Value[key]);
        }
        
        public static void SetItem(Arguments args, VM vm, SourceInfo info)
        {
            Check(args, 1, info);
            ValueHash hash = (ValueHash)args[0];
            IValue key = args[1];
            IValue value = args[2];
            vm.Push(hash.Value[key] = value);
        }
    }
}
