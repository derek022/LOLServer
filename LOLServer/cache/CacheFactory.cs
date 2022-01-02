using LOLServer.cache.impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.cache
{
    public class CacheFactory
    {
        public static readonly IAccountCache accountCache;
        public static readonly IUserCache userCache;
        static CacheFactory()
        {
            accountCache = new AccountCache();
            userCache = new UserCache();
        }
    }
}
