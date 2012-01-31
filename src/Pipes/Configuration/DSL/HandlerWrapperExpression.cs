namespace Pipes.Configuration.DSL
{
    public class HandlerWrapperExpression<T> : IHandlerWrapperExpression<T>
    {
        private readonly LambdaHandlerPolicy _policy;

        public HandlerWrapperExpression(LambdaHandlerPolicy policy)
        {
            _policy = policy;
        }

        public void WhenTheTargetHandlerIs<TH>() where TH : IHandler<T>
        {
            _policy.Include(x => x.Handler().HandlerType == typeof (TH));
        }
    }
    public interface IHandlerWrapperExpression<out T>
    {
        void WhenTheTargetHandlerIs<TH>() where TH : IHandler<T>;
    }

}