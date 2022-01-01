using LOLServer.biz;
using LOLServer.biz.impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz
{
    public class BizFactory
    {
        public static IAccountBiz accountBiz;

        static BizFactory()
        {
            accountBiz = new AccountBiz();
        }

    }
}
