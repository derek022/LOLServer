using LOLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.cache.impl
{
    public class UserCache : IUserCache
    {
        private int index = 0;
        /// <summary>
        /// 用户ID和模型的映射表
        /// </summary>
        Dictionary<int, User> idToModel = new Dictionary<int, User>();

        /// <summary>
        /// 账号ID和角色ID之间的绑定，一个账户可以有多个角色
        /// </summary>
        Dictionary<int, int> accToUid = new Dictionary<int, int>();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, UserToken> idToToken = new Dictionary<int, UserToken>();

        Dictionary<UserToken, int> tokenToId = new Dictionary<UserToken, int>();

        public bool Create(UserToken token, string name,int accountId)
        {
            User user = new User(index++,name);
            user.accountId = accountId;
            // 创建成功，进行账号ID和用户ID的绑定
            accToUid.Add(accountId, user.id);
            // 用户ID和模型的绑定
            idToModel.Add(user.id, user);

            return true;
        }

        public User get(UserToken token)
        {
            if (!has(token)) return null;

            return idToModel[tokenToId[token]];
        }

        public User get(int id)
        {
            return idToModel[id];
        }

        public User getByAccountId(int id)
        {
            if(!accToUid.ContainsKey(id))
            {
                return null;
            }

            return idToModel[accToUid[id]];
        }

        public UserToken getToken(int id)
        {
            return idToToken[id];
        }

        public bool has(UserToken token)
        {
            return tokenToId.ContainsKey(token);
        }

        public bool hasByAccountId(int id)
        {
            return accToUid.ContainsKey(id);
        }

        public bool isOnline(int id)
        {
            return idToToken.ContainsKey(id);
        }

        public void offline(UserToken token)
        {
            if(tokenToId.ContainsKey(token))
            {
                if(idToToken.ContainsKey(tokenToId[token]))
                {
                    idToToken.Remove(tokenToId[token]);
                }

                tokenToId.Remove(token);
            }
        }

        public User online(UserToken token,int id)
        {
            idToToken.Add(id, token);
            tokenToId.Add(token, id);

            return idToModel[id];
        }
    }
}
