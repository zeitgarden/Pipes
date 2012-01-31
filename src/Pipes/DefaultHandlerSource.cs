using System;
using System.Collections.Generic;
using FubuCore;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuMVC.Core.Registration;

namespace Pipes
{
    public class DefaultHandlerSource : IHandlerSource
    {
        private readonly CompositeFilter<Type> _filter = new CompositeFilter<Type>();
        private CompositeAction<TypePool> _actions = new CompositeAction<TypePool>();
        public DefaultHandlerSource()
        {
            _filter.Includes.Add(x => x.Closes(typeof (IHandler<>)));
            _filter.Includes.Add(x => x.IsClass);
            _filter.Excludes.Add(x => x.IsAbstract);
            _filter.Excludes.Add(x => x.IsGenericTypeDefinition);
            _filter.Excludes.Add(x => x.HasAttribute<SkipForAutoScanningAttribute>());
        }

        public DefaultHandlerSource ConfigurePool(Action<TypePool> cfg)
        {
            _actions += cfg;
            return this;
        }

        public DefaultHandlerSource ExcludeHandler(Type handler)
        {
            _filter.Excludes.Add(x => x == handler);
            return this;
        }

        public IEnumerable<Type> GetHandlers()
        {
            var pool = new TypePool(null);
            _actions.Do(pool);
            return pool.TypesMatching(_filter.MatchesAll);
        }
    }
}