namespace Pipes
{
    public interface IHandler<in T>
    {
        void Handle(T message);
    }
}