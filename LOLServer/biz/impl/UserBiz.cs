using LOLServer.cache;
using LOLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz.impl
{
    /// <summary>
    /// 用户事物处理
    /// </summary>
    public class UserBiz : IUserBiz
    {
        IAccountBiz accBiz = BizFactory.accountBiz;
        IUserCache userCache = CacheFactory.userCache;

        public bool Create(UserToken token, string name)
        {
            // 判断是否登录，获取账号ID
            // 的判断当前战虎是否存在

            int accountId = accBiz.Get(token);
            if (accountId == -1) return false;
            // 判断当前账号是否已经拥有角色
            if (userCache.hasByAccountId(accountId)) return false;

            userCache.Create(token, name,accountId);
            return true;
        }

        public User getInfo(UserToken token)
        {
            return userCache.get(token);
        }

        public User GetInfo(int id)
        {
            return userCache.get(id);
        }

        public UserToken getToken(int id)
        {
            return userCache.getToken(id);
        }

        public void offline(UserToken token)
        {
            userCache.offline(token);
        }

        public User online(UserToken token)
        {
            int accountId = accBiz.Get(token);
            if (accountId == -1) return null;

            User user = userCache.getByAccountId(accountId);
            // 已经在线
            if (userCache.isOnline(user.id)) return null;

            return userCache.online(token, user.id);
        }
    }
}
