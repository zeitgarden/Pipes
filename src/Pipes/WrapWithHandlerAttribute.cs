using System;
using FubuCore;

namespace Pipes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class WrapWithHandlerAttribute : Attribute
    {
        public WrapWithHandlerAttribute(Type handlerType)
        {
            if (handlerType.Closes(typeof (IHandler<>)))
            {
                HandlerType = handlerType;
            }
            else
            {
                throw new ArgumentException("Type is not a handler.", "handlerType");
            }
        }

        public Type HandlerType { get; private set; }
    }
}