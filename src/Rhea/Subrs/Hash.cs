using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Hash
    {
        public static void MakeHash(Arguments args, VM vm, SourceInfo info)
        {
            vm.Push(new ValueHash(args.Dict));
        }
    }
}
