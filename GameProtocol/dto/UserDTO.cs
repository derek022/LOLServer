using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    /// <summary>
    /// 用户数据序列化
    /// </summary>
    [Serializable]
    public class UserDTO
    {
        public int id; // ID，唯一主键
        public string name; // 召唤师姓名
        public int level; // 等级
        public int exp;  // 经验
        public int winCount;  // 胜利场次
        public int loseCount; // 失败场次
        public int ranCount; // 逃跑场次

        public UserDTO()
        {
        }

        public UserDTO(int id, string name, int level, int exp, int winCount, int loseCount, int ranCount)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.exp = exp;
            this.winCount = winCount;
            this.loseCount = loseCount;
            this.ranCount = ranCount;
        }
    }
}
