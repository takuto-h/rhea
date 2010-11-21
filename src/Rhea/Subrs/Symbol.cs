using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class Symbol
    {
        public static void MakeSymbol(IList<IValue> args, VM vm, SourceInfo info)
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
    }
}
