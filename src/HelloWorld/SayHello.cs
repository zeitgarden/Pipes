using System.IO;

namespace HelloWorld
{
    public class SayHello
    {
        public SayHello(string name, TextWriter output)
        {
            Name = name;
            Output = output;
        }

        public string Name { get; private set; }
        public TextWriter Output { get; private set; }
    }
}
