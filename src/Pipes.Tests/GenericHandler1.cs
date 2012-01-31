namespace Pipes.Tests
{
    public class GenericHandler1<T> : IHandler<T>
    {
        public void Handle(T message)
        {
        }
    }
}