namespace Pipes.Tests
{
    public class Never<T> : ICondition<T>
    {
        public bool Applies(T message)
        {
            return false;
        }
    }
}