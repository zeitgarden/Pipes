using System;
using FubuTestingSupport;
using NUnit.Framework;

namespace Pipes.Tests
{
    [TestFixture]
    public class WrapWithHandlerAttributeTester
    {
        [Test]
        public void allows_closed_generic_condition_type()
        {
            WrapWithHandlerAttribute attribute = null;
            Assert.DoesNotThrow(() => attribute = new WrapWithHandlerAttribute(typeof(GenericHandler1<string>)));
            attribute.ShouldNotBeNull().HandlerType.ShouldEqual(typeof(GenericHandler1<string>));
        }

        [Test]
        public void allows_open_generic_condition_type()
        {
            WrapWithHandlerAttribute attribute = null;
            Assert.DoesNotThrow(() => attribute = new WrapWithHandlerAttribute(typeof(GenericHandler1<>)));
            attribute.ShouldNotBeNull().HandlerType.ShouldEqual(typeof(GenericHandler1<>));
        }

        [Test]
        public void throws_when_is_not_a_condition_type()
        {
            Assert.Throws<ArgumentException>(() => new WrapWithHandlerAttribute(typeof(string)));
        }
    }
}