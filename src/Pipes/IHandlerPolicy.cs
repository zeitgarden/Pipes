namespace Pipes
{
    public interface IHandlerPolicy
    {
        void Configure(MessageChain chain);
    }
}