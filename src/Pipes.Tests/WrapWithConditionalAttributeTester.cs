using System;
using FubuTestingSupport;
using NUnit.Framework;

namespace Pipes.Tests
{
    [TestFixture]
    public class WrapWithConditionalAttributeTester
    {
        [Test]
        public void allows_closed_generic_condition_type()
        {
            WrapWithConditionalAttribute attribute = null;
            Assert.DoesNotThrow(() => attribute = new WrapWithConditionalAttribute(typeof (Always<string>)));
            attribute.ShouldNotBeNull().ConditionType.ShouldEqual(typeof (Always<string>));
        }

        [Test]
        public void allows_open_generic_condition_type()
        {
            WrapWithConditionalAttribute attribute = null;
            Assert.DoesNotThrow(() => attribute = new WrapWithConditionalAttribute(typeof (Always<>)));
            attribute.ShouldNotBeNull().ConditionType.ShouldEqual(typeof (Always<>));
        }

        [Test]
        public void throws_when_is_not_a_condition_type()
        {
            Assert.Throws<ArgumentException>(() => new WrapWithConditionalAttribute(typeof (string)));
        }
    }
}