using System.IO;
using FubuMVC.Core;
using Pipes;

namespace HelloWorld.WebApp
{
    public class SayHelloEndpoint
    {
        private readonly IPublisher _publisher;

        public SayHelloEndpoint(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public SayHelloOutput Command(SayHelloRequest request)
        {
            var writer = new StringWriter();
            _publisher.Publish(new SayHello(request.Name, writer));
            var messages = writer.ToString();
            return new SayHelloOutput
            {
                Messages = messages
            };
        }
    }

    public class SayHelloOutput
    {
        public string Messages { get; set; }
    }

    public class SayHelloRequest
    {
        [QueryString]
        public string Name { get; set; }
    }

}