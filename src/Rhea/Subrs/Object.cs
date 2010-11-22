using System.Collections.Generic;
using System.Linq;

namespace Rhea.Subrs
{
    public static class Object
    {
        public static void Klasses(Arguments args, VM vm, SourceInfo info)
        {
            IList<IValue> list = args[0].KlassList.Select(
                (elem) => (IValue)elem
            ).ToList();
            vm.Push(new ValueArray(list));
        }
    }
}
