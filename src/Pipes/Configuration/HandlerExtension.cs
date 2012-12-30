using Bottles;
using FubuMVC.Core;
using FubuMVC.Core.Registration;

namespace Pipes.Configuration
{
    public class HandlerExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(services);
        }

        private static void services(ServiceRegistry services)
        {
            services.SetServiceIfNone(new MessageGraph());
            services.SetServiceIfNone<IHandlerFacility, HandlerFacility>();
            services.SetServiceIfNone<IHandlerFactory, HandlerFactory>();
            services.SetServiceIfNone<IPublisher, Publisher>();

            services.FillType<IHandlerPolicy, WrapWithConditionalAttributeConvention>();
            services.FillType<IHandlerPolicy, WrapWithHandlerAttributeConvention>();

            services.FillType<IActivator, HandlerDiscover>();
            services.FillType<IActivator, HandlerPolicyRunner>();
            services.FillType<IActivator, HandlerRegister>();
        }
    }
}