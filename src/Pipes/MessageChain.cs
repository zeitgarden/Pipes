using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration.ObjectGraph;
using Pipes.Nodes;

namespace Pipes
{
    public class MessageChain : IEnumerable<MessageNode>
    {
        private MessageNode _top;

        public MessageChain(Type messageType)
        {
            MessageType = messageType;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Type MessageType { get; private set; }

        public MessageNode Top
        {
            get { return _top; }
            set
            {
                _top = value;
                _top.Chain = this;
            }
        }

        public bool HasHandler()
        {
            return this.Any(x => x is HandlerNode);
        }

        public HandlerNode Handler()
        {
            return this.OfType<HandlerNode>().FirstOrDefault();
        }

        public ObjectDef ToObjectDef()
        {
            var def = Top.ToObjectDef();
            def.Name = Id.ToString();
            return def;
        }

        public void AcceptVisitor(IMessageNodeVisitor visitor)
        {
            this.Each(visitor.Visit);
        }

        public IEnumerator<MessageNode> GetEnumerator()
        {
            if (Top == null)
            {
                yield break;
            }
            yield return Top;
            foreach (var node in Top)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}