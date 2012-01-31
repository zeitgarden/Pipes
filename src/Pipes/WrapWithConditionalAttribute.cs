using System;
using FubuCore;

namespace Pipes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class WrapWithConditionalAttribute : Attribute
    {
        public WrapWithConditionalAttribute(Type conditionType)
        {
            if (conditionType.Closes(typeof(ICondition<>)))
            {
                ConditionType = conditionType;
            }
            else
            {
                throw new ArgumentException("Type is not a condition.", "conditionType");
            }
        }
        
        public Type ConditionType { get; private set; }
    }
}