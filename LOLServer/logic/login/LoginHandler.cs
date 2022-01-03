using GameProtocol;
using GameProtocol.dto;
using LOLServer.biz;
using LOLServer.tool;
using NetFrame;
using NetFrame.auto;
using System;

namespace LOLServer.logic.login
{
    public class LoginHandler :AbsOnceHandler ,HandlerInterface
    {

        IAccountBiz accountBiz = BizFactory.accountBiz;

        public void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开连接 ");
            ExecutorPool.Instance.Execute(() =>
            {
                accountBiz.Close(token);
            });
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case LoginProtocol.LOGIN_CREQ:
                    Login(token, message.GetMessage<AccountInfoDTO>());
                    break;

                case LoginProtocol.REG_CREQ:
                    Reg(token, message.GetMessage<AccountInfoDTO>());
                    break;
            }
        }

        public void Login(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                int res = accountBiz.Login(token, value.account, value.password);

                Write(token, LoginProtocol.LOGIN_SRES, res);
            });
        }

        public void Reg(UserToken token,AccountInfoDTO value)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                int res = accountBiz.Register(token, value.account, value.password);

                Write(token, LoginProtocol.REG_SRES, res);
            });
        }

        public void ClientClose(UserToken token,AccountInfoDTO value)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                accountBiz.Close(token);
            });
        }


        public override byte getType()
        {
            return Protocol.TYPE_LOGIN;
        }
    }
}
