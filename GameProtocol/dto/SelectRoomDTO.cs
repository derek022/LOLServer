using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class SelectRoomDTO
    {
        // 两个队伍的信息
        public SelectModel[] teamOne;
        public SelectModel[] teamTwo;
    }
}
