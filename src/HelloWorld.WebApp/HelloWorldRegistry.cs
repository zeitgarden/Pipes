using FubuMVC.Core;
using FubuMVC.Spark;
using Pipes.Configuration.DSL;

namespace HelloWorld.WebApp
{
    public class HelloWorldRegistry : FubuRegistry
    {
        public HelloWorldRegistry()
        {
            IncludeDiagnostics(true);

            Actions.IncludeType<SayHelloEndpoint>();
            Actions.IncludeType<HomeHandler>();

            Routes.HomeIs<HomeHandler>(x => x.Index(new IndexModel()));
            Routes.IgnoreControllerNamespaceEntirely();
            Routes.IgnoreClassSuffix("Endpoint");
            Routes.IgnoreMethodsNamed("Command");

            Views.TryToAttachWithDefaultConventions();
            this.UseSpark();

            Output.ToJson.WhenTheOutputModelIs<SayHelloOutput>();
            
            this.Pipelines(x =>
            {
                x.Scan(s => s.AppliesToAssemblyOfType<SayHelloInRussian>());
                x.Policies
                    .ForHandlersOf<SayHello>(z =>
                    {
                        z.WrapWithHandler<TrapSayHelloError>();
                        z.WrapWithCondition<LanguageNotAllowed>(h => h.WhenTheTargetHandlerIs<SayHelloInGerman>());
                    });
            });
        }
    }
}