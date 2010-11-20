using System.Collections.Generic;

namespace Rhea
{
    public class KlassHolder
    {
        private IList<ValueSymbol> mKlasses;
        
        public KlassHolder(IList<ValueSymbol> klasses)
        {
            mKlasses = klasses;
        }
        
        public void Send(
            IValue receiver,
            ValueSymbol selector,
            IList<IValue> args,
            VM vm,
            SourceInfo info
        )
        {
            IValue value = null;
            foreach (ValueSymbol klass in mKlasses)
            {
                if (vm.Env.LookupMethod(klass, selector, out value))
                {
                    break;
                }
            }
            if (value == null)
            {
                throw new RheaException(
                    string.Format("invalid selector for {0}: {1}", receiver, selector.Name), info
                );
            }
            IValueFunc func = value as IValueFunc;
            if (func == null)
            {
                throw new RheaException(
                    string.Format("function required, but got {0}", value), info
                );
            }
            List<IValue> newArgs = new List<IValue>();
            newArgs.Add(receiver);
            newArgs.AddRange(args);
            func.Call(newArgs, vm, info);
        }
    }
}
