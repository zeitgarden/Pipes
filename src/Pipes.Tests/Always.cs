namespace Pipes.Tests
{
    public class Always<T> : ICondition<T>
    {
        public bool Applies(T message)
        {
            return true;
        }
    }

}