using LOLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.cache
{
    public interface IUserCache
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Create(UserToken token, string name,int accountId);
        /// <summary>
        /// 是否拥有角色
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool has(UserToken token);

        /// <summary>
        /// 判断对应账号ID是否拥有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool hasByAccountId(int id);
        /// <summary>
        /// 根据连接对象获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User get(UserToken token);

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User get(int id);
        /// <summary>
        /// 通过ID获取token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken getToken(int id);
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="token"></param>
        void offline(UserToken token);
        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User online(UserToken token,int id);

        /// <summary>
        /// 通过ID创建角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User getByAccountId(int id);

        /// <summary>
        /// 角色是否已经在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool isOnline(int id);
    }
}
