using Pipes;
using Pipes.Nodes;

namespace HelloWorld
{
    public class GermanLanguageNotAllowedPolicy : IHandlerPolicy
    {
        public void Configure(MessageChain chain)
        {
            if (chain.MessageType == typeof(SayHello) && chain.HasHandler() && chain.Handler().HandlerType == typeof(SayHelloInGerman))
            {
                chain.Handler().WrapWith(new ConditionalNode(typeof (LanguageNotAllowed)));
            }
        }
    }
}