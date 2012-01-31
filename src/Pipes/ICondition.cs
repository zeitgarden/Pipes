namespace Pipes
{
    public interface ICondition<in T>
    {
        bool Applies(T message);
    }
}