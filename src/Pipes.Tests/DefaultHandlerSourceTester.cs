using System;
using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;

namespace Pipes.Tests
{
    [TestFixture]
    public class DefaultHandlerSourceTester : InteractionContext<DefaultHandlerSource>
    {
        private IEnumerable<Type> _types;

        protected override void beforeEach()
        {
            ClassUnderTest.ConfigurePool(x => x.AddAssembly(GetType().Assembly));

            ClassUnderTest.ExcludeHandler(typeof (ExcludedHandler));

            _types = ClassUnderTest.GetHandlers()
                .Where(x => x.DeclaringType == GetType())
                .ToList();
        }

        [Test]
        public void type_must_be_a_handler()
        {
            _types.ShouldNotContain(typeof (NotAHandler));
        }

        [Test]
        public void type_must_be_a_closed_generic()
        {
            _types.ShouldNotContain(typeof (OpenGenericHandler<>));
        }

        [Test]
        public void type_must_be_a_class()
        {
            _types.ShouldNotContain(typeof (StructHandler));
        }

        [Test]
        public void excluded_handlers_are_not_taked_into_account()
        {
            _types.ShouldNotContain(typeof (ExcludedHandler));
        }

        [Test]
        public void types_decorated_with_skip_for_auto_scanning()
        {
            _types.ShouldNotContain(typeof (SkippedHandler));
        }

        [Test]
        public void type_must_not_be_abstract()
        {
            _types.ShouldNotContain(typeof (AbstractHandler));
        }


        [Test]
        public void valid_handler_types_are_included()
        {
            _types.ShouldEqual(new[] {typeof (Valid1Handler), typeof (Valid2Handler)});
        }

        public class NotAHandler
        {
        }

        public abstract class AbstractHandler : IHandler<string>
        {
            public void Handle(string message)
            {
            }
        }

        public class OpenGenericHandler<T> : IHandler<T>
        {
            public void Handle(T message)
            {
            }
        }

        public struct StructHandler : IHandler<string>
        {
            public void Handle(string message)
            {
            }
        }

        [SkipForAutoScanning]
        public class SkippedHandler : IHandler<string>
        {
            public void Handle(string message)
            {
            }
        }

        public class ExcludedHandler : IHandler<string>
        {
            public void Handle(string message)
            {
            }
        }

        public class Valid1Handler : IHandler<string>
        {
            public void Handle(string message)
            {
            }
        }

        public class Valid2Handler : IHandler<string>
        {
            public void Handle(string message)
            {
            }
        }
    }
}