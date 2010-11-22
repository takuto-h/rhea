using System.Collections.Generic;
using System.Linq;

namespace Rhea.Subrs
{
    public static class Instance
    {
        public static void MakeInstance(Arguments args, VM vm, SourceInfo info)
        {
            ValueArray array = args[0] as ValueArray;
            ValueHash hash = args[1] as ValueHash;
            if (array == null ||
                array.ArrayValue.Any((elem) => !(elem is ValueSymbol)))
            {
                throw new RheaException(
                    string.Format("array(symbol) required, but got {0}", args[0]), info
                );
            }
            if (hash == null ||
                hash.HashValue.Any((kvp) => !(kvp.Key is ValueSymbol) || !(kvp.Value is IValueFunc)))
            {
                throw new RheaException(
                    string.Format("hash(symbol, function) required, but got {0}", args[1]), info
                );
            }
            IList<ValueSymbol> klasses = array.ArrayValue.Select(
                (elem) => (ValueSymbol)elem
            ).ToList();
            IDictionary<ValueSymbol, IValueFunc> slots = hash.HashValue.ToDictionary(
                (kvp) => (ValueSymbol)kvp.Key,
                (kvp) => (IValueFunc)kvp.Value
            );
            vm.Push(new ValueInstance(klasses, slots));
        }
    }
}
