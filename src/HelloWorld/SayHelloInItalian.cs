using Pipes;

namespace HelloWorld
{
    [WrapWithHandler(typeof(SayHelloLogger))]
    public class SayHelloInItalian : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Ciao {0}", message.Name);
        }
    }
}