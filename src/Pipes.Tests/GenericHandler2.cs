namespace Pipes.Tests
{
    public class GenericHandler2<T> : IHandler<T>
    {
        public void Handle(T message)
        {
        }
    }
}