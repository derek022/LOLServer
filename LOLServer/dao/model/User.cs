using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.dao.model
{
    public class User
    {
        public int id; // ID，唯一主键
        public string name; // 召唤师姓名
        public int level; // 等级
        public int exp;  // 经验
        public int winCount;  // 胜利场次
        public int loseCount; // 失败场次
        public int ranCount; // 逃跑场次
    }
}
