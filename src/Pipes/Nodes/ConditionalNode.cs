using System;
using FubuCore;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Nodes
{
    public class ConditionalNode : MessageNode
    {
        public ConditionalNode(Type conditionType)
        {
            ConditionType = conditionType;
        }

        public Type ConditionType { get; private set; }

        protected override ObjectDef BuildObjectDef()
        {
            var messageType = Chain.MessageType;
            var handlerDef = new ObjectDef(typeof (ConditionalHandler<>), messageType);
            var dependencyType = typeof (ICondition<>).MakeGenericType(messageType);
            var conditionType = ConditionType;
            if(conditionType.IsOpenGeneric())
            {
                conditionType = ConditionType.MakeGenericType(messageType);
            }
            handlerDef.DependencyByType(dependencyType, conditionType);
            return handlerDef;
        }

        public override string Description
        {
            get { return "Condition : {0} for Message : {1}".ToFormat(ConditionType.Name, Chain.MessageType.Name); }
        }
    }
}