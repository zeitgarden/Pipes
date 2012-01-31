using Pipes;
using Pipes.Nodes;

namespace HelloWorld
{
    public class TrapRussianErrorConvention : IHandlerPolicy
    {
        public void Configure(MessageChain chain)
        {
            if (chain.MessageType == typeof(SayHello) && chain.HasHandler() && chain.Handler().HandlerType == typeof(SayHelloInRussian))
            {
                chain.Handler().WrapWith(new HandlerNode(typeof (TrapSayHelloError)));
            }
        }
    }
}