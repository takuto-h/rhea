namespace Rhea
{
    public class InsnPush : IInsn
    {
        private IValue mValue;
        
        public InsnPush(IValue value)
        {
            mValue = value;
        }
        
        public void Execute(VM vm)
        {
            vm.Push(mValue);
        }
        
        public string Show()
        {
            return string.Format("(push {0})", mValue);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
