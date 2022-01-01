using LOLServer.cache.impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.cache
{
    public class CacheFactory
    {
        public static readonly IAccountCache accountCache;

        static CacheFactory()
        {
            accountCache = new AccountCache();
        }
    }
}
