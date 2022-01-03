using GameProtocol;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic.select
{
    public class SelectRoom : AbsMultiHandler, HandlerInterface
    {
        public int id;


        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            
        }


        public void ClientClose(UserToken token, string error)
        {
            throw new NotImplementedException();
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            throw new NotImplementedException();
        }

        public override byte getType()
        {
            return Protocol.TYPE_SELECT;
        }
    }
}
