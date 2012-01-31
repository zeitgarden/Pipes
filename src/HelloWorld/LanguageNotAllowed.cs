using Pipes;

namespace HelloWorld
{
    public class LanguageNotAllowed : ICondition<SayHello>
    {
        public bool Applies(SayHello message)
        {
            return false;
        }
    }
}