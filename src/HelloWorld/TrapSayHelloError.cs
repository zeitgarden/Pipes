using System;
using Pipes;

namespace HelloWorld
{
    [SkipForAutoScanning]
    public class TrapSayHelloError : IHandler<SayHello>
    {
        private readonly IHandler<SayHello> _inner;

        public TrapSayHelloError(IHandler<SayHello> inner)
        {
            _inner = inner;
        }

        public void Handle(SayHello message)
        {
            try
            {
                _inner.Handle(message);
            }
            catch (Exception ex)
            {
                var error = string.Format("Couldn't say Hello using the [{0}] handler.\nThe error was : [{1}]",
                                          _inner.GetType().Name, ex.Message);
                message.Output.WriteLine(error);
            }
        }
    }
}