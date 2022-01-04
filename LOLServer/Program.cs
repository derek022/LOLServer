using LOLServer.tool;
using NetFrame.auto;
using System;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NetFrame.ServerStart ss = new NetFrame.ServerStart(9999);

            ss.encode = MessageEncoding.encode;
            ss.decode = MessageEncoding.decode;
            ss.center = new HandlerCenter();
            ss.LD = LengthEncoding.decode;
            ss.LE = LengthEncoding.encode;
            ss.Start(55590);

            while (true) { }

        }
    }
}
