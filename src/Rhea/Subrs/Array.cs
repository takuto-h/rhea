using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Array
    {
        public static void MakeArray(Arguments args, VM vm, SourceInfo info)
        {
            vm.Push(new ValueArray(args.List));
        }
    }
}
