using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerStart ss = new ServerStart(9000);
            ss.encode = MessageEncoding.Encode;
            ss.center = new HandlerCenter();
            ss.decode = MessageEncoding.Decode;
            ss.LD = LengthEncoding.decode;
            ss.LE = LengthEncoding.encode;
            ss.Start(6650);
            Console.WriteLine("服务器启动成功");
            while (true)
            {
                
            }

        }
    }
}
