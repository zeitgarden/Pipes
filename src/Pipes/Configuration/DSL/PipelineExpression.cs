using System;
using System.Collections.Generic;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Configuration.DSL
{
    public class PipelineExpression : IPipelineExpression
    {
        private readonly IList<ObjectDef> _sources;
        private readonly IList<ObjectDef> _policies;

        public PipelineExpression(IList<ObjectDef> sources, IList<ObjectDef> policies)
        {
            _sources = sources;
            _policies = policies;
        }

        public IPoliciesExpression Policies
        {
            get { return new PoliciesExpression(_policies); }
        }

        public ISourcesExpression Sources
        {
            get { return new SourcesExpression(_sources); }
        }

        public IPipelineExpression Scan(Action<IScanExpression> cfg)
        {
            var source = new DefaultHandlerSource();
            var exp = new ScanExpression(source);
            cfg(exp);
            Sources.Add(source);
            return this;
        }
    }
    public interface IPipelineExpression
    {
        IPoliciesExpression Policies { get; }
        ISourcesExpression Sources { get; }
        IPipelineExpression Scan(Action<IScanExpression> cfg);
    }
}