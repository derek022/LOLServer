using LOLServer.biz;
using LOLServer.dao.model;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic
{
    public class AbsOnceHandler
    {

        public IUserBiz userBiz = BizFactory.userBiz;

        private byte type;
        private int area;

        public void setArea(int area)
        {
            this.area = area;
        }

        public virtual int getArea()
        {
            return this.area;
        }

        public void setType(byte type)
        {
            this.type = type;
        }

        public virtual byte getType()
        {
            return this.type;
        }


        public User getUser(UserToken token)
        {
            return userBiz.getInfo(token);
        }

        public User getUser(int id)
        {
            return userBiz.getInfo(id);
        }

        /// <summary>
        /// 通过对象
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int getUserId (UserToken token)
        {
            User user = getUser(token);
            if (user == null) return -1;
            return user.id;
        }

        /// <summary>
        /// 通过ID获取连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserToken getToken(int id)
        {
            return userBiz.getToken(id);
        }

        #region 通过连接对象发送
        public void Write(UserToken token, int command)
        {
            Write(token, command, null);
        }

        public void Write(UserToken token, int command, object message)
        {
            Write(token, getArea(), command, message);
        }

        public void Write(UserToken token, int area, int command, object message)
        {
            Write(token,getType() ,area, command, message);
        }

        public void Write(UserToken token,byte type,int area,int command,object message)
        {
            // 消息体编码
            byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
            // 长度编码
            value = LengthEncoding.encode(value);

            token.Write(value);
        }
        #endregion

        #region 通过玩家ID发送
        public void Write(int id, int command)
        {
            Write(id, command, null);
        }

        public void Write(int id, int command, object message)
        {
            Write(id, getArea(), command, message);
        }

        public void Write(int id, int area, int command, object message)
        {
            Write(id, getType(), area, command, message);
        }

        public void Write(int id, byte type, int area, int command, object message)
        {
            UserToken token = getToken(id);
            if(token == null)
            {
                return;
            }
            Write(token, type, area, command, message);
        }


        public void WriteToUsers(int [] users,byte type,int area,int command,object message)
        {
            // 消息体编码
            byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
            // 长度编码
            value = LengthEncoding.encode(value);

            foreach(int userId in users)
            {
                UserToken token = userBiz.getToken(userId);
                if (token == null) continue;

                byte[] bs = new byte[value.Length];
                Array.Copy(value, 0, bs, 0, value.Length);
                // 写入之后，二进制数据变空，复制一份出来
                token.Write(bs);

            }
        }
        #endregion


        public SocketModel CreateSocketModel(byte type,int area,int command,object message)
        {
            return new SocketModel(type, area, command, message);
        }


    }
}
