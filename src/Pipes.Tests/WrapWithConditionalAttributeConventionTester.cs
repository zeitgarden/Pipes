using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;

namespace Pipes.Tests
{
    [TestFixture]
    public class WrapWithConditionalAttributeConventionTester : InteractionContext<WrapWithConditionalAttributeConvention>
    {
        private MessageChain _chain1;
        private MessageChain _chain2;
        private MessageChain _chain3;

        protected override void beforeEach()
        {
            _chain1 = new MessageChain(typeof(NewUserMessage))
            {
                Top = new HandlerNode(typeof(Handler1))
            };
            _chain2 = new MessageChain(typeof(NewUserMessage))
            {
                Top = new HandlerNode(typeof(Handler2))
            };
            _chain3 = new MessageChain(typeof(NewUserMessage))
            {
                Top = new HandlerNode(typeof(Handler3))
            };

            ClassUnderTest.Configure(_chain1);
            ClassUnderTest.Configure(_chain2);
            ClassUnderTest.Configure(_chain3);
        }

        [Test]
        public void the_chain_handler_is_wrapped_with_conditional_nodes()
        {
            _chain1
                .Top.ShouldNotBeNull().ShouldBeOfType<ConditionalNode>()
                .Child.ShouldNotBeNull().ShouldBeOfType<ConditionalNode>()
                .Child.ShouldNotBeNull().ShouldBeOfType<HandlerNode>()
                .HandlerType.ShouldEqual(typeof (Handler1));
        }

        [Test]
        public void wraps_the_chain_with_the_decorated_conditions()
        {
            _chain1.OfType<ConditionalNode>()
                .ShouldContain(x => x.ConditionType == typeof (Condition1<NewUserMessage>));
        }

        [Test]
        public void can_wrap_with_decorated_open_generic_conditions()
        {
            _chain1.OfType<ConditionalNode>()
                .ShouldContain(x => x.ConditionType == typeof (Condition2<NewUserMessage>));
        }

        [Test]
        public void if_the_decorated_condition_does_not_match_the_message_type_the_chain_is_not_modified()
        {
            _chain2.Top
                .ShouldNotBeNull().ShouldBeOfType<HandlerNode>()
                .HandlerType.ShouldEqual(typeof (Handler2));
            _chain2.Top.Child.ShouldBeNull();
        }

        [Test]
        public void if_the_handler_is_not_decorated_the_chain_is_not_modified()
        {
            _chain3.Top
                .ShouldNotBeNull().ShouldBeOfType<HandlerNode>()
                .HandlerType.ShouldEqual(typeof(Handler3));
            _chain3.Top.Child.ShouldBeNull();
        }

        public class Condition1<T> : ICondition<T>
        {
            public bool Applies(T message)
            {
                return true;
            }
        }
        public class Condition2<T> : ICondition<T>
        {
            public bool Applies(T message)
            {
                return true;
            }
        }

        [WrapWithConditional(typeof(Condition1<NewUserMessage>))]
        [WrapWithConditional(typeof(Condition2<>))]
        public class Handler1 : IHandler<NewUserMessage>
        {
            public void Handle(NewUserMessage message)
            {
            }
        }

        [WrapWithConditional(typeof(Condition1<LogoutMessage>))]
        public class Handler2 : IHandler<NewUserMessage>
        {
            public void Handle(NewUserMessage message)
            {
            }
        }

        public class Handler3 : IHandler<NewUserMessage>
        {
            public void Handle(NewUserMessage message)
            {
            }
        }
    }
}