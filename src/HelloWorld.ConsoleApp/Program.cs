using System;
using Pipes;
using Pipes.Configuration.StructureMap;
using StructureMap;

namespace HelloWorld.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            var source = new DefaultHandlerSource();
            source.ConfigurePool(pool => pool.AddAssembly(typeof(SayHello).Assembly));
            
            container.Configure(x =>
            {
                x.For<IHandlerSource>().Add(source);
                x.For<IHandlerPolicy>().Add<TrapRussianErrorConvention>();
                x.For<IHandlerPolicy>().Add<GermanLanguageNotAllowedPolicy>();
            });
            container.BootstrapPipeline();
            container.Publisher().Publish(new SayHello("Jaime", Console.Out));

            Console.ReadLine();
        }
    }
}
