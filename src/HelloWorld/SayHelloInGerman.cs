using Pipes;

namespace HelloWorld
{
    public class SayHelloInGerman : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Hallo {0}", message.Name);
        }
    }
}