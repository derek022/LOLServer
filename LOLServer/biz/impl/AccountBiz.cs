using LOLServer.cache;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz.impl
{
    public class AccountBiz : IAccountBiz
    {

        IAccountCache accountCache = CacheFactory.accountCache;

        public void Close(UserToken token)
        {
            accountCache.offline(token);
        }

        public int Get(UserToken token)
        {
            return accountCache.getId(token);
        }

        public int Login(UserToken token, string account, string password)
        {
            // 登录条件判断
            if (account == null || password == null) return -4;
            if (!accountCache.hasAccount(account)) return -1;
            if (accountCache.isOnline(account)) return -2;
            if (!accountCache.match(account, password)) return -3;

            // 符合登录条件，登录
            accountCache.online(token, account);

            return 0;
        }

        public int Register(UserToken token, string account, string password)
        {
            if (accountCache.hasAccount(account)) return 1;

            accountCache.add(account, password);

            return 0;
        }
    }
}
