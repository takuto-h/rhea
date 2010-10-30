namespace Rhea
{
    public interface IInsn : IShowable
    {
        void Execute(VM vm);
    }
}
