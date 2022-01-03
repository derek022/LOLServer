using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic.match
{
    /// <summary>
    /// 战斗匹配房间模型
    /// </summary>
    public class MatchRoom
    {
        public int id;// 房间唯一ID
        public int teamMax = 1; // 每只队伍需要匹配到的人数
        public List<int> teamOne = new List<int>(); // 队伍1 人员ID
        public List<int> teamTwo = new List<int>(); // 队伍2 人员ID


    }
}
