using LOLServer.biz;
using LOLServer.biz.impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz
{
    public class BizFactory
    {
        public static readonly IAccountBiz accountBiz;
        public static readonly IUserBiz userBiz;

        static BizFactory()
        {
            accountBiz = new AccountBiz();
            userBiz = new UserBiz();
        }

    }
}
