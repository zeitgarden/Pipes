using Bottles;
using StructureMap.Configuration.DSL;

namespace Pipes.Configuration.StructureMap
{
    public class MessagesRegistry : Registry
    {
        public MessagesRegistry()
        {
            For<MessageGraph>().Use(new MessageGraph());

            For<IHandlerFacility>().Use<HandlerEngine>();
            For<IHandlerFactory>().Use<HandlerEngine>();
            For<IPublisher>().Use<Publisher>();

            For<IHandlerPolicy>().Add<WrapWithConditionalAttributeConvention>();
            For<IHandlerPolicy>().Add<WrapWithHandlerAttributeConvention>();

            For<IActivator>().Add<HandlerDiscover>();
            For<IActivator>().Add<HandlerPolicyRunner>();
            For<IActivator>().Add<HandlerRegister>();
        }
    }
}