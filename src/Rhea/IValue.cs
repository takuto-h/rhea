using System.Collections.Generic;

namespace Rhea
{
    public interface IValue : IShowable
    {
        void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info);
    }
}
