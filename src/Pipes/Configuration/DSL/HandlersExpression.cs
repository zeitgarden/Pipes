using System;
using System.Collections.Generic;
using FubuMVC.Core.Registration.ObjectGraph;
using Pipes.Nodes;

namespace Pipes.Configuration.DSL
{
    public class HandlersExpression<T> : IHandlersExpression<T>
    {
        private readonly IList<ObjectDef> _policies;

        public HandlersExpression(IList<ObjectDef> policies)
        {
            _policies = policies;
        }

        public void WrapWithHandler<TH>() where TH : IHandler<T>
        {
            WrapWithHandler<TH>(h => { });
        }

        public void WrapWithHandler<TH>(Action<IHandlerWrapperExpression<T>> cfg) where TH : IHandler<T>
        {
            var policy = new LambdaHandlerPolicy();
            var exp = new HandlerWrapperExpression<T>(policy);

            policy.Include(x => x.HasHandler());
            policy.Include(x => x.MessageType == typeof(T));
            policy.AddAction(x => x.Handler().WrapWith(new WrapperNode(typeof(TH))));

            cfg(exp);

            _policies.Add(ObjectDef.ForValue(policy));

        }

        public void WrapWithCondition<TC>() where TC : ICondition<T>
        {
            WrapWithCondition<TC>(h => { });
        }

        public void WrapWithCondition<TC>(Action<IHandlerWrapperExpression<T>> cfg) where TC : ICondition<T>
        {
            var policy = new LambdaHandlerPolicy();
            var exp = new HandlerWrapperExpression<T>(policy);

            policy.Include(x => x.HasHandler());
            policy.Include(x => x.MessageType == typeof(T));
            policy.AddAction(x => x.Handler().WrapWith(new ConditionalNode(typeof(TC))));

            cfg(exp);

            _policies.Add(ObjectDef.ForValue(policy));

        }
    }
    public interface IHandlersExpression<out T>
    {
        void WrapWithHandler<TH>() where TH : IHandler<T>;
        void WrapWithHandler<TH>(Action<IHandlerWrapperExpression<T>> cfg) where TH : IHandler<T>;
        void WrapWithCondition<TC>() where TC : ICondition<T>;
        void WrapWithCondition<TC>(Action<IHandlerWrapperExpression<T>> cfg) where TC : ICondition<T>;
    }

}