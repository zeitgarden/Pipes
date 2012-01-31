using System;
using System.Linq.Expressions;
using FubuCore.Util;

namespace Pipes
{
    public class LambdaHandlerPolicy : IHandlerPolicy
    {
        private CompositeAction<MessageChain> _action = new CompositeAction<MessageChain>();
        private readonly CompositeFilter<MessageChain> _filter = new CompositeFilter<MessageChain>();

        public void AddAction(Action<MessageChain> value)
        {
            _action += value;
        }

        public void Include(Expression<Func<MessageChain, bool>> value)
        {
            _filter.Includes.Add(value);
        }

        public void Exclude(Expression<Func<MessageChain, bool>> value)
        {
            _filter.Excludes.Add(value);
        }


        public void Configure(MessageChain chain)
        {
            if (_filter.MatchesAll(chain))
            {
                _action.Do(chain);
            }
        }
    }

    public class LambdaHandlerPolicy<T, TH> : LambdaHandlerPolicy where TH : IHandler<T>
    {
        public LambdaHandlerPolicy()
        {
            Include(x => x.HasHandler());
            Include(x => x.MessageType == typeof (T));
            Include(x => x.Handler().HandlerType == typeof (TH));
        }
    }
}