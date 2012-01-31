using Pipes;

namespace HelloWorld
{
    [SkipForAutoScanning]
    public class SayHelloLogger : IHandler<SayHello>
    {
        private readonly IHandler<SayHello> _inner;

        public SayHelloLogger(IHandler<SayHello> inner)
        {
            _inner = inner;
        }

        public void Handle(SayHello message)
        {
            message.Output.WriteLine("Before : {0}.", _inner.GetType().Name);
            _inner.Handle(message);
            message.Output.WriteLine("After : {0}.", _inner.GetType().Name);
        }
    }
}