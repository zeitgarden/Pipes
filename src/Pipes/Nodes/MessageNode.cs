using System.Collections;
using System.Collections.Generic;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Nodes
{
    public abstract class MessageNode : IEnumerable<MessageNode>
    {
        public MessageNode Child { get; private set; }

        public MessageNode Parent { get; private set; }

        public MessageChain Chain { get; set; }

        public void ReplaceWith(MessageNode node)
        {
            node.Child = Child;
            node.Chain = Chain;
            if (Parent != null)
            {
                node.Parent = Parent;
                Parent.Child = node;
            }
            if (Chain.Top == this)
            {
                Chain.Top = node;
            }
            Parent = null;
            Child = null;
            Chain = null;
        }

        public void WrapWith(MessageNode node)
        {
            node.Chain = Chain;
            node.Child = this;
            if (Parent != null)
            {
                Parent.Child = node;
                node.Parent = Parent;
            }
            Parent = node;
            if (Chain.Top == this)
            {
                Chain.Top = node;
            }
        }

        public ObjectDef ToObjectDef()
        {
            var def = BuildObjectDef();
            if (Child != null)
            {
                def.Dependency(typeof(IHandler<>).MakeGenericType(Chain.MessageType), Child.ToObjectDef());
            }
            return def;
        }

        protected abstract ObjectDef BuildObjectDef();

        public IEnumerator<MessageNode> GetEnumerator()
        {
            if (Child == null)
            {
                yield break;
            }
            yield return Child;
            foreach (var node in Child)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract string Description { get; }

    }
}