using System.Collections.Generic;

namespace Rhea.Subrs
{
    public static class False
    {
        public static void MatchBool(IList<IValue> args, VM vm, SourceInfo info)
        {
            IValueFunc func = args[2] as IValueFunc;
            if (func == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", args[2]),
                    info
                );
            }
            func.Call(new List<IValue>(), vm, info);
        }
    }
}
