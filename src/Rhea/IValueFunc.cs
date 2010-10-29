using System.Collections.Generic;

namespace Rhea
{
    public interface IValueFunc : IValue
    {
        void Call(IList<IValue> args, VM vm, SourceInfo info);
    }
}
