using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic
{
    public interface HandlerInterface
    {
        void ClientClose(UserToken token, string error);

        //void ClientConnect(UserToken token);

        void MessageReceive(UserToken token, SocketModel message);
    }
}
