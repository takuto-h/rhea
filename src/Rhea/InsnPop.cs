namespace Rhea
{
    public class InsnPop : IInsn
    {
        private static IInsn smInstance;
        
        public static IInsn Instance
        {
            get
            {
                if (smInstance == null)
                {
                    smInstance = new InsnPop();
                }
                return smInstance;
            }
        }
        
        private InsnPop()
        {
        }
        
        public void Execute(VM vm)
        {
            vm.Pop();
        }
        
        public string Show()
        {
            return "(pop)";
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
