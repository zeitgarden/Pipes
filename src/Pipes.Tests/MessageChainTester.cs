using System;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuTestingSupport;
using NUnit.Framework;
using Pipes.Nodes;
using Rhino.Mocks;

namespace Pipes.Tests
{
    [TestFixture]
    public class MessageChainTester : InteractionContext<MessageChain>
    {
        protected override void beforeEach()
        {
            Services.Inject(new MessageChain(typeof (NewUserMessage)));
        }

        [Test]
        public void id_is_not_empty()
        {
            ClassUnderTest.Id.ShouldNotEqual(Guid.Empty);
        }

        [Test]
        public void message_type_test()
        {
            ClassUnderTest.MessageType.ShouldEqual(typeof (NewUserMessage));
        }

        [Test]
        public void has_handler_returns_false_if_no_handler_node_exists()
        {
            ClassUnderTest.HasHandler().ShouldBeFalse();
        }

        [Test]
        public void has_handler_returns_true_if_a_handler_node_exists()
        {
            ClassUnderTest.Top = new HandlerNode(typeof (NewUserMessage));
            ClassUnderTest.HasHandler().ShouldBeTrue();
        }

        [Test]
        public void handler_returns_null_if_no_handler_node_exists()
        {
            ClassUnderTest.Handler().ShouldBeNull();
        }

        [Test]
        public void handler_returns_the_handler_node_if_exists()
        {
            var top = new HandlerNode(typeof (GenericHandler1<NewUserMessage>));
            var wrapper = new WrapperNode(typeof (GenericHandler1<NewUserMessage>));
            ClassUnderTest.Top = top;
            top.WrapWith(wrapper);
            ClassUnderTest.Handler().ShouldEqual(top);
        }

        [Test]
        public void setting_the_top_node_also_sets_the_chain_of_the_node()
        {
            ClassUnderTest.Top = MockFor<MessageNode>();
            MockFor<MessageNode>().Chain.ShouldEqual(ClassUnderTest);
        }

        [Test]
        public void to_objected_def_returns_the_object_def_of_the_top_node_and_sets_the_def_name_as_the_chain_id()
        {
            var def = new ObjectDef();
            MockFor<MessageNode>().Stub(x => x.ToObjectDef()).Return(def);
            ClassUnderTest.Top = MockFor<MessageNode>();
            ClassUnderTest.ToObjectDef().ShouldEqual(def);
            def.Name.ShouldEqual(ClassUnderTest.Id.ToString());
        }


    }


    [TestFixture]
    public class Chain_Alteration_Tester : InteractionContext<MessageChain>
    {
        private HandlerNode _top;

        protected override void beforeEach()
        {
            Services.Inject(new MessageChain(typeof (NewUserMessage)));
            _top = new HandlerNode(typeof (GenericHandler1<NewUserMessage>));
            ClassUnderTest.Top = _top;
        }

        [TestFixture]
        public class when_wrapping_top_handler : Chain_Alteration_Tester
        {
            private MessageNode _wrapper;

            protected override void beforeEach()
            {
                base.beforeEach();
                _wrapper = new ConditionalNode(typeof(Always<>));
                _top.WrapWith(_wrapper);
            }

            [Test]
            public void chain_top_is_wrapper()
            {
                ClassUnderTest.Top.ShouldEqual(_wrapper);
            }

            [Test]
            public void wrapper_child_is_original_node()
            {
                _wrapper.Child.ShouldEqual(_top);
            }

            [Test]
            public void wrapper_parent_is_null()
            {
                _wrapper.Parent.ShouldBeNull();
            }

            [Test]
            public void wrapper_chain_is_class_under_test()
            {
                _wrapper.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void original_node_parent_is_wrapper()
            {
                _top.Parent.ShouldEqual(_wrapper);
            }

            [Test]
            public void original_node_chain_is_class_under_test()
            {
                _top.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void original_node_child_is_null()
            {
                _top.Child.ShouldBeNull();
            }
        }

        [TestFixture]
        public class when_wrapping_wrapped_node : Chain_Alteration_Tester
        {
            private MessageNode _wrapper1;
            private MessageNode _wrapper2;

            protected override void beforeEach()
            {
                base.beforeEach();
                _wrapper1 = new ConditionalNode(typeof(Always<>));
                _wrapper2 = new ConditionalNode(typeof(Never<>));
                _top.WrapWith(_wrapper1);
                _top.WrapWith(_wrapper2);
            }

            [Test]
            public void chain_top_is_first_wrapper()
            {
                ClassUnderTest.Top.ShouldEqual(_wrapper1);
            }

            [Test]
            public void first_wrapper_child_is_inner_wrapper()
            {
                _wrapper1.Child.ShouldEqual(_wrapper2);
            }

            [Test]
            public void first_wrapper_parent_is_null()
            {
                _wrapper1.Parent.ShouldBeNull();
            }

            [Test]
            public void first_wrapper_chain_is_class_under_test()
            {
                _wrapper1.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void original_node_parent_is_inner_wrapper()
            {
                _top.Parent.ShouldEqual(_wrapper2);
            }

            [Test]
            public void original_node_chain_is_class_under_test()
            {
                _top.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void original_node_child_is_null()
            {
                _top.Child.ShouldBeNull();
            }

            [Test]
            public void inner_wrapper_parent_is_first_wrapper()
            {
                _wrapper2.Parent.ShouldEqual(_wrapper1);
            }

            [Test]
            public void inner_wrapper_child_is_original_node()
            {
                _wrapper2.Child.ShouldEqual(_top);
            }

            [Test]
            public void inner_wrapper_chain_is_class_under_test()
            {
                _wrapper2.Chain.ShouldEqual(ClassUnderTest);
            }
        }


        [TestFixture]
        public class when_replacing_top_handler : Chain_Alteration_Tester
        {
            private HandlerNode _newTop;

            protected override void beforeEach()
            {
                base.beforeEach();
                _newTop = new HandlerNode(typeof(GenericHandler2<NewUserMessage>));
                _top.ReplaceWith(_newTop);
            }

            [Test]
            public void chain_top_points_to_the_new_node()
            {
                ClassUnderTest.Top.ShouldEqual(_newTop);
            }

            [Test]
            public void new_top_child_is_null()
            {
                _newTop.Child.ShouldBeNull();
            }

            [Test]
            public void new_top_parent_is_null()
            {
                _newTop.Parent.ShouldBeNull();
            }

            [Test]
            public void new_top_chain_is_class_under_test()
            {
                _newTop.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void original_top_child_is_null()
            {
                _top.Child.ShouldBeNull();
            }

            [Test]
            public void original_top_parent_is_null()
            {
                _top.Parent.ShouldBeNull();
            }

            [Test]
            public void original_top_chain_is_null()
            {
                _top.Chain.ShouldBeNull();
            }
        }

        [TestFixture]
        public class when_replacing_wrapped_node : Chain_Alteration_Tester
        {
            private MessageNode _wrapper1;
            private MessageNode _wrapper2;
            private MessageNode _newHandlerNode;
            protected override void beforeEach()
            {
                base.beforeEach();
                _wrapper1 = new ConditionalNode(typeof(Never<>));
                _wrapper2 = new ConditionalNode(typeof(Always<>));
                _newHandlerNode = new HandlerNode(typeof (GenericHandler2<>));
                _top.WrapWith(_wrapper1);
                _top.WrapWith(_wrapper2);
                _top.ReplaceWith(_newHandlerNode);
            }

            [Test]
            public void chain_top_points_to_first_wrapper()
            {
                ClassUnderTest.Top.ShouldEqual(_wrapper1);
            }

            [Test]
            public void first_wrapper_parent_is_null()
            {
                _wrapper1.Parent.ShouldBeNull();
            }

            [Test]
            public void first_wrapper_child_points_to_inner_wrapper()
            {
                _wrapper1.Child.ShouldEqual(_wrapper2);
            }

            [Test]
            public void first_wrapper_chain_is_class_under_test()
            {
                _wrapper1.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void inner_wrapper_parent_is_first_wrapper()
            {
                _wrapper2.Parent.ShouldEqual(_wrapper1);
            }

            [Test]
            public void inner_wrapper_child_points_to_the_new_node()
            {
                _wrapper2.Child.ShouldEqual(_newHandlerNode);
            }

            [Test]
            public void inner_wrapper_chain_is_class_under_test()
            {
                _wrapper2.Chain.ShouldEqual(ClassUnderTest);
            }

            [Test]
            public void new_node_parent_is_inner_wrapper()
            {
                _newHandlerNode.Parent.ShouldEqual(_wrapper2);
            }

            [Test]
            public void new_node_child_is_null()
            {
                _newHandlerNode.Child.ShouldBeNull();
            }

            [Test]
            public void new_node_chain_is_class_under_test()
            {
                _newHandlerNode.Chain.ShouldEqual(ClassUnderTest);
            }
        }
    }
}