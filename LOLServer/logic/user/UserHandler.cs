using GameProtocol;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic.user
{
    public class UserHandler : AbsOnceHandler, HandlerInterface
    {
        public void ClientClose(UserToken token, string error)
        {

        }

        public void ClientConnect(UserToken token)
        {

        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch(message.command)
            {
                case UserProtocol.CREATE_CREQ:
                    create(token, message.GetMessage<string>());
                    break;

                case UserProtocol.INFO_CREQ:
                    info(token);
                    break;

                case UserProtocol.ONLINE_CREQ:
                    online(token);
                    break;
            }
        }


        private void create(UserToken token,string message)
        {

        }

        private void info(UserToken token)
        {

        }

        private void online(UserToken token)
        {

        }
    }
}
