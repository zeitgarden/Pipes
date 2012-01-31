using System;
using FubuCore;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Nodes
{
    public class WrapperNode : MessageNode
    {
        public WrapperNode(Type wrapperType)
        {
            WrapperType = wrapperType;
        }
        
        public Type WrapperType { get; private set; }

        protected override ObjectDef BuildObjectDef()
        {
            var wrapperType = WrapperType;
            if (wrapperType.IsOpenGeneric())
            {
                wrapperType = wrapperType.MakeGenericType(Chain.MessageType);
            }
            return new ObjectDef(wrapperType);
        }

        public override string Description
        {
            get { return "Wrapper : {0} for Message : {1}".ToFormat(WrapperType.Name, Chain.MessageType.Name); }
        }
    }
}