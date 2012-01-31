using Pipes.Nodes;

namespace Pipes
{
    public interface IMessageNodeVisitor
    {
        void Visit(MessageNode node);
    }
}