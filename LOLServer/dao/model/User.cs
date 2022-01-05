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

        public int accountId; // 角色所属的账号ID

        public int[] heroList;

        public User(int id,string name)
        {
            this.id = id;
            this.name = name;
            level = 0;
            exp = 0;
            winCount = 0;
            loseCount = 0;
            ranCount = 0;
        }
    }
}
