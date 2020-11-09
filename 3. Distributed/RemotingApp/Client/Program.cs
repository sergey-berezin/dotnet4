using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpChannel(0), false);
            var h = (IHello)Activator.GetObject(typeof(IHello), "tcp://localhost:8081/hello");

            Console.WriteLine(h.SayHello("World"));
            Console.ReadLine();
        }
    }
}
