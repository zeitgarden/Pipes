using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Configuration.DSL
{
    public static class FubuRegistryExtensions
    {
        public static FubuRegistry Pipelines(this FubuRegistry registry, Action<IPipelineExpression> cfg)
        {
            IList<ObjectDef> sources = new List<ObjectDef>();
            IList<ObjectDef> policies = new List<ObjectDef>();
            IPipelineExpression exp = new PipelineExpression(sources, policies);

            cfg(exp);

            sources.Each(source => registry.Services(x => x.AddService(typeof(IHandlerSource), source)));
            policies.Each(policy => registry.Services(x => x.AddService(typeof(IHandlerPolicy), policy)));

            return registry;
        }
    }
}