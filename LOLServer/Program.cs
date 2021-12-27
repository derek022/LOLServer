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
            ss.Start(55509);

            while (true) { }
            
        }
    }
}
