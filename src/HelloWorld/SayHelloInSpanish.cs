using Pipes;

namespace HelloWorld
{
    public class SayHelloInSpanish : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Hola {0}", message.Name);
        }
    }
}