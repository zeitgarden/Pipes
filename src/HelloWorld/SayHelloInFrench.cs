using Pipes;

namespace HelloWorld
{
    public class SayHelloInFrench : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Salut {0}", message.Name);
        }
    }
}