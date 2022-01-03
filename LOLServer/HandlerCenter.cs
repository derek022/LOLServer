using System;
using System.Collections.Generic;
using System.Text;
using GameProtocol;
using LOLServer.logic;
using LOLServer.logic.login;
using LOLServer.logic.match;
using LOLServer.logic.user;
using NetFrame;
using NetFrame.auto;

namespace LOLServer
{
    public class HandlerCenter : AbsHandlerCenter
    {
        HandlerInterface login;
        HandlerInterface user;
        HandlerInterface match;

        public HandlerCenter()
        {
            login = new LoginHandler();
            user = new UserHandler();
            match = new MatchHandler();
        }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine($"有客户端断开连接了 -- HashCode: ${ token.GetHashCode()}");
            match.ClientClose(token, error);
            // user的连接关闭方法，一定要放在逻辑处理单元后面
            // 其他逻辑单元需要通过user绑定数据来清理内存
            // 如果先清除了绑定关系，其他模块无法获取角色数据会导致无法清理
            user.ClientClose(token, error);
            login.ClientClose(token, error);
        }

        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine($"有客户端连接了 -- HashCode : ${token.GetHashCode()}");
        }

        public override void MessageReceive(UserToken token, object message)
        {
            SocketModel model = message as SocketModel;
            Console.WriteLine($"有新消息 来自 ${token.GetHashCode()}; type: ${model.type} ,area: ${model.area}, command:${model.command}, obj:${model.message}");

            switch (model.type)
            {
                case Protocol.TYPE_LOGIN:
                    login.MessageReceive(token, model);
                    break;

                case Protocol.TYPE_USER:
                    user.MessageReceive(token, model);
                    break;

                case Protocol.TYPE_MATCH:
                    match.MessageReceive(token, model);
                    break;
                default:
                    //未知模块， 可能是客户端作弊了，无视
                    break;
            }

        }
    }
}
