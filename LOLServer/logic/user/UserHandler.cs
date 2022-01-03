using GameProtocol;
using GameProtocol.dto;
using LOLServer.biz;
using LOLServer.dao.model;
using LOLServer.tool;
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
            userBiz.offline(token);
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
            ExecutorPool.Instance.Execute(() =>
            {
                Write(token, UserProtocol.CREATE_SRES, userBiz.Create(token, message));
            });
        }

        private void info(UserToken token)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                Write(token, UserProtocol.INFO_SRES, convert(userBiz.GetInfoByAccount(token)));
            });
        }

        private void online(UserToken token)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                Write(token, UserProtocol.ONLINE_SRES, convert(userBiz.online(token)));
            });
        }


        private UserDTO convert(User user)
        {
            if (user == null) return null;

            return new UserDTO(user.id, user.name, user.level, user.exp, user.winCount, user.loseCount, user.ranCount);

        }

        public override byte getType()
        {
            return Protocol.TYPE_USER;
        }
    }
}
