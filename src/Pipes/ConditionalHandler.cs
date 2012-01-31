namespace Pipes
{
    public class ConditionalHandler<T> : IHandler<T>
    {
        private readonly ICondition<T> _condition;
        private readonly IHandler<T> _inner;

        public ConditionalHandler(ICondition<T> condition, IHandler<T> inner)
        {
            _condition = condition;
            _inner = inner;
        }

        public void Handle(T message)
        {
            if (_condition.Applies(message))
            {
                _inner.Handle(message);
            }
        }
    }
}