using FubuCore;
using FubuCore.Reflection;
using Pipes.Nodes;

namespace Pipes
{
    public class WrapWithHandlerAttributeConvention : IHandlerPolicy
    {
        public void Configure(MessageChain chain)
        {
            if (!chain.HasHandler())
            {
                return;
            }
            var handler = chain.Handler();
            var handlerType = handler.HandlerType;
            if (!handlerType.HasAttribute<WrapWithHandlerAttribute>())
            {
                return;
            }
            var attributes = handlerType.GetAllAttributes<WrapWithHandlerAttribute>();
            foreach (var attribute in attributes)
            {
                var targetType = attribute.HandlerType;
                if (targetType.IsOpenGeneric())
                {
                    targetType = targetType.MakeGenericType(chain.MessageType);
                }
                else
                {
                    if (targetType.FindParameterTypeTo(typeof(IHandler<>)) != chain.MessageType)
                    {
                        continue;
                    }
                }
                handler.WrapWith(new WrapperNode(targetType));
            }
        }
    }
}