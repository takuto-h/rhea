using System.Collections.Generic;

namespace Rhea
{
    public class InsnPushWinder : IInsn
    {
        private KeyValuePair<IValueFunc, IValueFunc> mWinder;
        
        public InsnPushWinder(KeyValuePair<IValueFunc, IValueFunc> winder)
        {
            mWinder = winder;
        }
        
        public void Execute(VM vm)
        {
            vm.PushWinder(mWinder);
        }
    }
}
