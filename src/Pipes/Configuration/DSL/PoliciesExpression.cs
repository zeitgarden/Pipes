using System;
using System.Collections.Generic;
using FubuMVC.Core.Registration.ObjectGraph;
using Pipes.Nodes;

namespace Pipes.Configuration.DSL
{
    public class PoliciesExpression : IPoliciesExpression
    {
        private readonly IList<ObjectDef> _policies;

        public PoliciesExpression(IList<ObjectDef> policies)
        {
            _policies = policies;
        }

        public IPoliciesExpression Add<T>() where T : class, IHandlerPolicy, new()
        {
            return Add<T>(x => { });
        }

        public IPoliciesExpression Add<T>(Action<T> setup) where T : class, IHandlerPolicy, new()
        {
            var policy = new T();
            setup(policy);
            return Add(policy);
        }

        public IPoliciesExpression Add(IHandlerPolicy policy)
        {
            _policies.Add(ObjectDef.ForValue(policy));
            return this;
        }

        public IPoliciesExpression Add(Type type)
        {
            _policies.Add(new ObjectDef(type));
            return this;
        }

        public IPoliciesExpression ForHandlersOf<T>(Action<IHandlersExpression<T>> cfg)
        {
            var exp = new HandlersExpression<T>(_policies);
            cfg(exp);
            return this;
        }

        public IHandlersExpression<T> WrapHandlersOf<T>()
        {
            var exp = new HandlersExpression<T>(_policies);
            return exp;
        }

        public IPoliciesExpression WrapHandlersWith(Type openHandlerType)
        {
            var policy = new LambdaHandlerPolicy();
            policy.Include(x => x.HasHandler());
            policy.AddAction(x => x.Handler().WrapWith(new HandlerNode(openHandlerType)));
            return this;
        }
    }
    public interface IPoliciesExpression
    {
        IPoliciesExpression Add<T>() where T : class, IHandlerPolicy, new();
        IPoliciesExpression Add<T>(Action<T> setup) where T : class, IHandlerPolicy, new();
        IPoliciesExpression Add(IHandlerPolicy policy);
        IPoliciesExpression Add(Type type);

        IPoliciesExpression ForHandlersOf<T>(Action<IHandlersExpression<T>> cfg);

        IPoliciesExpression WrapHandlersWith(Type openHandlerType);
    }
}