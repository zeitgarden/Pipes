using System;
using System.Reflection;

namespace Pipes.Configuration.DSL
{
    public class ScanExpression : IScanExpression
    {
        private readonly DefaultHandlerSource _source;

        public ScanExpression(DefaultHandlerSource source)
        {
            _source = source;
        }

        public IScanExpression AppliesToAssemblyOfType<T>()
        {
            return AppliesTo(typeof (T).Assembly);
        }

        public IScanExpression AppliesTo(Assembly assembly)
        {
            _source.ConfigurePool(x => x.AddAssembly(assembly));
            return this;
        }

        public IScanExpression ExcludeHandler(Type handler)
        {
            _source.ExcludeHandler(handler);
            return this;
        }
    }
    public interface IScanExpression
    {
        IScanExpression AppliesToAssemblyOfType<T>();
        IScanExpression AppliesTo(Assembly assembly);
        IScanExpression ExcludeHandler(Type handler);
    }

}