using System.Collections.Generic;

namespace Rhea
{
    public class InsnSetWinders : IInsn
    {
        private ISList<KeyValuePair<IValue, IValue>> mWinders;
        
        public InsnSetWinders(ISList<KeyValuePair<IValue, IValue>> winders)
        {
            mWinders = winders;
        }
        
        public void Execute(VM vm)
        {
            vm.Winders = mWinders;
        }
        
        public string Show()
        {
            return string.Format("(setwinders {0})", mWinders);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
