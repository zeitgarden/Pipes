using System;
using FubuCore;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Nodes
{
    public class HandlerNode : MessageNode
    {
        public HandlerNode(Type handlerType)
        {
            HandlerType = handlerType;
        }

        public Type HandlerType { get; private set; }

        protected override ObjectDef BuildObjectDef()
        {
            var handlerType = HandlerType;
            if (handlerType.IsOpenGeneric())
            {
                handlerType = handlerType.MakeGenericType(Chain.MessageType);
            }
            return new ObjectDef(handlerType);
        }

        public override string Description
        {
            get { return "Handler : {0} for Message : {1}".ToFormat(HandlerType.Name, Chain.MessageType.Name); }
        }
    }
}