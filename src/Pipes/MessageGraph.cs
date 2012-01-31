using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;

namespace Pipes
{
    public class MessageGraph
    {
        private readonly Cache<Type, IList<MessageChain>> _chains;

        public MessageGraph()
        {
            _chains = new Cache<Type, IList<MessageChain>>(x => new List<MessageChain>());
        }

        public MessageChain NewChainFor(Type message)
        {
            var chain = new MessageChain(message);
            _chains[message].Add(chain);
            return chain;
        }

        public IEnumerable<MessageChain> ChainsOf(Type message)
        {
            return _chains[message];
        }

        public IEnumerable<MessageChain> Chains
        {
            get { return _chains.SelectMany(x => x); }
        }
    }
}