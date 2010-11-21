using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Array
    {
        public static void MakeArray(IList<IValue> args, VM vm, SourceInfo info)
        {
            vm.Push(new ValueArray(args));
        }
    }
}
