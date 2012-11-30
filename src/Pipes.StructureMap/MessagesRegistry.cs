using Bottles;
using StructureMap.Configuration.DSL;

namespace Pipes.StructureMap
{
    public class MessagesRegistry : Registry
    {
        public MessagesRegistry()
        {
            For<MessageGraph>().Use(new MessageGraph());
            
            For<IHandlerFacility>().Use<StructureMapHandlerEngine>();
            For<IHandlerFactory>().Use<StructureMapHandlerEngine>();
            For<IPublisher>().Use<Publisher>();

            For<IHandlerPolicy>().Add<WrapWithConditionalAttributeConvention>();
            For<IHandlerPolicy>().Add<WrapWithHandlerAttributeConvention>();

            For<IActivator>().Add<HandlerDiscover>();
            For<IActivator>().Add<HandlerPolicyRunner>();
            For<IActivator>().Add<HandlerRegister>();
        }
    }
}