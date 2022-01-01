using LOLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.biz.impl
{
    public class UserBiz : IUserBiz
    {
        public bool Create(UserToken token, string name)
        {
            throw new NotImplementedException();
        }

        public User getInfo(UserToken token)
        {
            throw new NotImplementedException();
        }

        public User GetInfo(int id)
        {
            throw new NotImplementedException();
        }

        public UserToken getToken(int id)
        {
            throw new NotImplementedException();
        }

        public void offline(UserToken token)
        {
            throw new NotImplementedException();
        }

        public User online(UserToken token)
        {
            throw new NotImplementedException();
        }
    }
}
