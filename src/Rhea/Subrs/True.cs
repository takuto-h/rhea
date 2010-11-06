using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class True
    {
        public static void MatchBool(IList<IValue> args, VM vm, SourceInfo info)
        {
            IValueFunc func = args[1] as IValueFunc;
            if (func == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", args[1]),
                    info
                );
            }
            func.Call(new List<IValue>(), vm, info);
        }
    }
}
