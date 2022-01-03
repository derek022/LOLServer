using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// 创建选人模块事件
/// </summary>
/// <param name="teamOne"></param>
/// <param name="teamTwo"></param>
public delegate void CreateSelect(List<int> teamOne, List<int> teamTwo);

/// <summary>
/// 移除选人模块事件
/// </summary>
/// <param name="roomId"></param>
public delegate void DestorySelect(int roomId);

namespace LOLServer.tool
{
    public class EventUtil
    {
        public static CreateSelect createSelect;
        public static DestorySelect destorySelect;

    }
}
