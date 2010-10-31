using System;
using System.Collections.Generic;

namespace Rhea
{
    public class EnvGlobal : IEnv
    {
        private IDictionary<ValueSymbol, IValue> mVariables;
        private IDictionary<KeyValuePair<ValueSymbol, ValueSymbol>, IValue> mMethods;
        
        public IEnv OuterEnv
        {
            get { throw new NotSupportedException(); }
        }
        
        public EnvGlobal()
        {
            mVariables = new Dictionary<ValueSymbol, IValue>();
            mMethods = new Dictionary<KeyValuePair<ValueSymbol, ValueSymbol>, IValue>();
        }
        
        public bool IsGlobal()
        {
            return true;
        }
        
        public void AddVariable(ValueSymbol selector, IValue value)
        {
            mVariables.Add(selector, value);
        }
        
        public void AddMethod(ValueSymbol klass, ValueSymbol selector, IValue value)
        {
            mMethods.Add(MakePair(klass, selector), value);
        }
        
        public bool TryGetVariable(ValueSymbol selector, out IValue value)
        {
            return mVariables.TryGetValue(selector, out value);
        }
        
        public bool TryGetMethod(ValueSymbol klass, ValueSymbol selector, out IValue value)
        {
            return mMethods.TryGetValue(MakePair(klass, selector), out value);
        }
        
        public bool ContainsVariable(ValueSymbol selector)
        {
            return mVariables.ContainsKey(selector);
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
