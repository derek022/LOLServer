using LOLServer.biz;
using LOLServer.biz.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz
{
   public class BizFactory
    {
       public readonly static IAccountBiz accountBiz;
       public readonly static IUserBiz userBiz;
       static BizFactory() {
           accountBiz = new AccountBiz();
           userBiz = new UserBiz();
       }
    }
}
