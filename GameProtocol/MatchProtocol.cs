using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class MatchProtocol
    {
        public const int ENTER_CREQ = 0;
        public const int ENTER_SRES = 1;

        public const int LEAVE_CREQ = 2;
        public const int LEAVE_SRES = 3;// 返回离开

        public const int ENTER_SELECT_BRO = 4; // 匹配完毕，通知进入选择界面广告
    }
}
