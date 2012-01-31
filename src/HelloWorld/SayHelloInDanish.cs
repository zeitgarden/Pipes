using Pipes;

namespace HelloWorld
{
    [WrapWithConditional(typeof(LanguageNotAllowed))]
    public class SayHelloInDanish : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Hej {0}", message.Name);
        }
    }
}