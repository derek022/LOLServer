using GameProtocol;
using GameProtocol.dto;
using NetFrame;
using NetFrame.auto;
using System;

namespace LOLServer.logic.login
{
    class LoginHandler : HandlerInterface
    {
        public void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开连接 ");
        }

        public void ClientConnect(UserToken token)
        {
            
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case LoginProtocol.LOGIN_CREQ:
                    login(token, message.GetMessage<AccountInfoDTO>());
                    break;

                case LoginProtocol.REG_CREQ:
                    reg(token, message.GetMessage<AccountInfoDTO>());
                    break;
            }
        }

        public void login(UserToken token, AccountInfoDTO value)
        {
            Console.WriteLine("dto " + value.account);
        }

        public void reg(UserToken token,AccountInfoDTO value)
        {

        }



    }
}
