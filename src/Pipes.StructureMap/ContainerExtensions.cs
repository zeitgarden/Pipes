using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using StructureMap;

namespace Pipes.StructureMap
{
    public static class ContainerExtensions
    {
        public static void BootstrapPipeline(this IContainer container)
        {
            container.Configure(x => x.IncludeRegistry<MessagesRegistry>());
            container.GetAllInstances<IActivator>().Each(x => x.Activate(null, new PackageLog()));
        }

        public static IPublisher Publisher(this IContainer container)
        {
            return container.GetInstance<IPublisher>();
        }
    }
}