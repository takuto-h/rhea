using System.Collections.Generic;

namespace Rhea
{
    public class KlassHolder
    {
        public IList<ValueSymbol> KlassList { get; private set; }
        
        public KlassHolder(IList<ValueSymbol> klassList)
        {
            KlassList = klassList;
        }
        
        public void Send(
            IValue receiver,
            ValueSymbol selector,
            IList<IValue> args,
            VM vm,
            SourceInfo info
        )
        {
            IValueFunc func = null;
            foreach (ValueSymbol klass in KlassList)
            {
                if (vm.Env.LookupMethod(klass, selector, out func))
                {
                    break;
                }
            }
            if (func == null)
            {
                throw new RheaException(
                    string.Format(
                        "invalid selector for {0}: {1}",
                        receiver,
                        selector.Name.ToIdentifier()
                    ),
                    info
                );
            }
            List<IValue> newArgs = new List<IValue>();
            newArgs.Add(receiver);
            newArgs.AddRange(args);
            func.Call(newArgs, vm, info);
        }
    }
}
