using System;
using System.Collections.Generic;

namespace Rhea
{
    public class EnvGlobal : IEnv
    {
        private IDictionary<ValueSymbol, IValue> mVariables;
        private IDictionary<KeyValuePair<ValueSymbol, ValueSymbol>, IValueFunc> mMethods;
        
        public IEnv OuterEnv
        {
            get { throw new NotSupportedException(); }
        }
        
        public EnvGlobal()
        {
            mVariables = new Dictionary<ValueSymbol, IValue>();
            mMethods = new Dictionary<KeyValuePair<ValueSymbol, ValueSymbol>, IValueFunc>();
        }
        
        public bool IsGlobal()
        {
            return true;
        }
        
        public IValue this[ValueSymbol symbol]
        {
            get
            {
                return mVariables[symbol];
            }
            set
            {
                mVariables[symbol] = value;
            }
        }
        
        public IValueFunc this[ValueSymbol klass, ValueSymbol selector]
        {
            get
            {
                return mMethods[MakePair(klass, selector)];
            }
            set
            {
                mMethods[MakePair(klass, selector)] = value;
            }
        }
        
        public void AddVariable(ValueSymbol symbol, IValue value)
        {
            mVariables.Add(symbol, value);
        }
        
        public void AddMethod(ValueSymbol klass, ValueSymbol selector, IValueFunc func)
        {
            mMethods.Add(MakePair(klass, selector), func);
        }
        
        public bool TryGetVariable(ValueSymbol symbol, out IValue value)
        {
            return mVariables.TryGetValue(symbol, out value);
        }
        
        public bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValueFunc func)
        {
            return mMethods.TryGetValue(MakePair(klass, selector), out func);
        }
        
        public bool ContainsVariable(ValueSymbol symbol)
        {
            return mVariables.ContainsKey(symbol);
        }
        
        public bool ContainsMethod(ValueSymbol klass, ValueSymbol selector)
        {
            return mMethods.ContainsKey(MakePair(klass, selector));
        }
        
        private static KeyValuePair<ValueSymbol, ValueSymbol> MakePair(ValueSymbol klass, ValueSymbol selector)
        {
            return new KeyValuePair<ValueSymbol, ValueSymbol>(klass, selector);
        }
    }
}
