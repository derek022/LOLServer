using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz.account
{
    public interface IAccountBiz
    {
        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>返回创建结果， 0 成功; 1 账号重复; 2 账号不合法; 3 密码不合法</returns>
        int Register(UserToken token, string account, string password);


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>登录结果：0 成功；1 密码错误；2 账号不存在</returns>
        int Login(UserToken token, string account, string password);


        /// <summary>
        /// 客户端断开连接（下线）
        /// </summary>
        /// <param name="token"></param>
        void Close(UserToken token);


        /// <summary>
        /// 返回账户ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回用户的账户ID</returns>
        int Get(UserToken token);

    }
}
