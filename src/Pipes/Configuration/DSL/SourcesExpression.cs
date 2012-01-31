using System;
using System.Collections.Generic;
using FubuCore;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes.Configuration.DSL
{
    public class SourcesExpression : ISourcesExpression
    {
        private readonly IList<ObjectDef> _sources;

        public SourcesExpression(IList<ObjectDef> sources)
        {
            _sources = sources;
        }

        public ISourcesExpression Add<T>() where T : class, IHandlerSource, new()
        {
            return Add<T>(x => { });
        }

        public ISourcesExpression Add<T>(Action<T> setup) where T : class, IHandlerSource, new()
        {
            var source = new T();
            setup(source);
            return Add(source);
        }

        public ISourcesExpression Add(IHandlerSource source)
        {
            _sources.Add(ObjectDef.ForValue(source));
            return this;
        }

        public ISourcesExpression Add(Type type)
        {
            if (type.CanBeCastTo<IHandlerSource>())
            {
                _sources.Add(new ObjectDef(type));
            }
            else
            {
                throw new ArgumentException("Not a valid handler source type: " + type, "type");
            }
            return this;
        }
    }
    public interface ISourcesExpression
    {
        ISourcesExpression Add<T>() where T : class, IHandlerSource, new();
        ISourcesExpression Add<T>(Action<T> setup) where T : class, IHandlerSource, new();
        ISourcesExpression Add(IHandlerSource source);
        ISourcesExpression Add(Type type);
    }

}