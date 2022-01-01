using LOLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz
{
    public interface IUserBiz
    {
        /// <summary>
        /// 创建召唤师
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Create(UserToken token, string name);

        /// <summary>
        /// 获取连接对应的用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User getInfo(UserToken token);

        /// <summary>
        /// 通过ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetInfo(int id);

        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User online(UserToken token);

        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="token"></param>
        void offline(UserToken token);

        /// <summary>
        /// 通过ID获取对象连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken getToken(int id);
    }
}
