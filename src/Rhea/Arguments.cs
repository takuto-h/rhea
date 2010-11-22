using System.Collections.Generic;

namespace Rhea
{
    public class Arguments
    {
        public IList<IValue> List { get; private set; }
        public IDictionary<IValue, IValue> Dict { get; private set; }
        
        public IValue this[int index]
        {
            get
            {
                return List[index];
            }
        }
        
        public IValue this[string keywordName]
        {
            get
            {
                return Dict[ValueSymbol.Intern(keywordName)];
            }
        }
        
        public Arguments(IList<IValue> list, IDictionary<IValue, IValue> dict)
        {
            List = list;
            Dict = dict;
        }
    }
}
