using Pipes;

namespace HelloWorld
{
    public class SayHelloInEnglish : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Hello {0}", message.Name);
        }
    }
}