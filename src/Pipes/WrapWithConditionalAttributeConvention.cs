using FubuCore;
using FubuCore.Reflection;
using Pipes.Nodes;

namespace Pipes
{
    public class WrapWithConditionalAttributeConvention : IHandlerPolicy
    {
        public void Configure(MessageChain chain)
        {
            if (!chain.HasHandler())
            {
                return;
            }
            var handler = chain.Handler();
            var handlerType = handler.HandlerType;
            if (!handlerType.HasAttribute<WrapWithConditionalAttribute>())
            {
                return;
            }
            var attributes = handlerType.GetAllAttributes<WrapWithConditionalAttribute>();
            foreach (var attribute in attributes)
            {
                var targetType = attribute.ConditionType;
                if (targetType.IsOpenGeneric())
                {
                    targetType = targetType.MakeGenericType(chain.MessageType);
                }
                else
                {
                    if (targetType.FindParameterTypeTo(typeof(ICondition<>)) != chain.MessageType)
                    {
                        continue;
                    }
                }
                handler.WrapWith(new ConditionalNode(targetType));
            }
        }
    }
}