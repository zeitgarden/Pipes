namespace Pipes
{
    public class Publisher : IPublisher
    {
        private readonly IHandlerFactory _factory;
        private readonly MessageGraph _graph;

        public Publisher(IHandlerFactory factory, MessageGraph graph)
        {
            _factory = factory;
            _graph = graph;
        }

        public void Publish<T>(T message)
        {
            foreach (var chain in _graph.ChainsOf(typeof(T)))
            {
                var handler = _factory.GetHandler<T>(chain.Id);
                handler.Handle(message);
            }
        }
    }
    public interface IPublisher
    {
        void Publish<T>(T message);
    }

}