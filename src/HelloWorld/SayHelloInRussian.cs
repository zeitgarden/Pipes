using System;
using Pipes;

namespace HelloWorld
{
    public class SayHelloInRussian : IHandler<SayHello>
    {
        public void Handle(SayHello message)
        {
            throw new NotImplementedException("I don't speak Russian.");
        }
    }
}