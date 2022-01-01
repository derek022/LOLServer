using LOLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.cache.impl
{
    /// <summary>
    /// 账号缓存层
    /// </summary>
    public class AccountCache : IAccountCache
    {

        private int index = 0;
        /// <summary>
        /// 玩家连接对象与账号的映射绑定
        /// </summary>
        Dictionary<UserToken, string> onlineAccMap = new Dictionary<UserToken, string>();
        /// <summary>
        /// 账号与自身具体属性的映射绑定
        /// </summary>
        Dictionary<string, AccountModel> accMap = new Dictionary<string, AccountModel>();


        public void add(string account, string password)
        {
            // 创建账号实体并进行绑定
            AccountModel model = new AccountModel();
            model.account = account;
            model.password = password;
            model.id = index;
            accMap.Add(account, model);
            index++;
        }

        public int getId(UserToken token)
        {
            // 判断在线字典中是否有此连接的记录，没有说明此连接没有登录，无法获取到账号ID
            if(!onlineAccMap.ContainsKey(token))
            {
                return -1;
            }
            // 返回绑定账号的ID
            return accMap[onlineAccMap[token]].id;
        }

        public bool hasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool isOnline(string account)
        {
            return onlineAccMap.ContainsValue(account);
        }

        public bool match(string account, string password)
        {
            if(!hasAccount(account))
            {
                return false;
            }


            return accMap[account].password.Equals(password); ;
        }

        public void offline(UserToken token)
        {
            if(onlineAccMap.ContainsKey(token))
            {
                onlineAccMap.Remove(token);
            }
        }

        public void online(UserToken token, string account)
        {
            // 添加映射
            onlineAccMap.Add(token, account);
        }
    }
}
