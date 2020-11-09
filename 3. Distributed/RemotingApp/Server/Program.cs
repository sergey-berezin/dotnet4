using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Hello : MarshalByRefObject, IHello
    {
        public string SayHello(string name)
        {
            Console.WriteLine("name = " + name);
            return "Hello, " + name + "!";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var h = new Hello();
            ChannelServices.RegisterChannel(new TcpChannel(8081), false);
            RemotingServices.Marshal(h, "hello");
            Console.ReadLine();
        }
    }
}
