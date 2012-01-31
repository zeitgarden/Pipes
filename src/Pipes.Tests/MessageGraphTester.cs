using FubuTestingSupport;
using NUnit.Framework;

namespace Pipes.Tests
{
    [TestFixture]
    public class MessageGraphTester : InteractionContext<MessageGraph>
    {
        [Test]
        public void new_chain_returns_a_chain_with_the_specified_message_type()
        {
            ClassUnderTest.NewChainFor(typeof (NewUserMessage)).ShouldNotBeNull().MessageType.ShouldEqual(typeof (NewUserMessage));
        }

        [Test]
        public void chains_of_returns_all_the_chains_registered_for_the_requested_message_type()
        {
            var chain1 = ClassUnderTest.NewChainFor(typeof (NewUserMessage));
            var chain2 = ClassUnderTest.NewChainFor(typeof (NewUserMessage));
            ClassUnderTest.ChainsOf(typeof (NewUserMessage)).ShouldHaveCount(2).ShouldEqual(new[] {chain1, chain2});
        }

        [Test]
        public void new_chain_for_returns_unique_instances()
        {
            var chain1 = ClassUnderTest.NewChainFor(typeof(NewUserMessage));
            var chain2 = ClassUnderTest.NewChainFor(typeof(NewUserMessage));
            chain1.ShouldNotBeTheSameAs(chain2);
        }
        [Test]
        public void if_no_chains_has_been_registered_for_a_given_message_type_then_chains_of_returns_an_empty_list()
        {
            ClassUnderTest.ChainsOf(typeof (NewUserMessage)).ShouldHaveCount(0);
        }

        [Test]
        public void chains_returns_all_the_registered_chains()
        {
            var chain1 = ClassUnderTest.NewChainFor(typeof(NewUserMessage));
            var chain2 = ClassUnderTest.NewChainFor(typeof(LogoutMessage));
            ClassUnderTest.Chains.ShouldHaveCount(2).ShouldEqual(new[] {chain1, chain2});
        }
    }
}