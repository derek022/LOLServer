using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    /// <summary>
    /// 账户信息序列化
    /// </summary>
    [Serializable]
    public class AccountInfoDTO
    {
        public string account;
        public string password;
    }
}
