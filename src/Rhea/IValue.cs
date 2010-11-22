using System.Collections.Generic;

namespace Rhea
{
    public interface IValue : IShowable
    {
        IList<ValueSymbol> KlassList { get; }
        void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info);
    }
}
