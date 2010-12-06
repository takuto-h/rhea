using System.Collections.Generic;

namespace Rhea
{
    public class ValueInstance : IValue
    {
        private KlassHolder mKlassHolder;
        private IDictionary<ValueSymbol, IValueFunc> mMethods;
        
        public IList<ValueSymbol> KlassList
        {
            get { return mKlassHolder.KlassList; }
        }
        
        public ValueInstance(
            IList<ValueSymbol> klasses,
            IDictionary<ValueSymbol, IValueFunc> methods
        )
        {
            mKlassHolder = new KlassHolder(klasses);
            mMethods = methods;
        }
        
        public void Send(ValueSymbol selector, IList<IValue> args, VM vm, SourceInfo info)
        {
            IValueFunc func;
            if (mMethods.TryGetValue(selector, out func))
            {
                List<IValue> newArgs = new List<IValue>();
                newArgs.Add(this);
                newArgs.AddRange(args);
                func.Call(newArgs, vm, info);
                return;
            }
            mKlassHolder.Send(this, selector, args, vm, info);
        }
        
        public string Show()
        {
            return string.Format("$<instance 0x{0:x8}>", GetHashCode());
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
