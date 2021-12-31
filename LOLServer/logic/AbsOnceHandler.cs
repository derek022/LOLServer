using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic
{
    public class AbsOnceHandler
    {

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

        }

        public void Write(int id, int command, object message)
        {

        }

        public void Write(int id, int area, int command, object message)
        {

        }

        public void Write(int id, byte type, int area, int command, object message)
        {

        }
        #endregion


        public SocketModel CreateSocketModel(byte type,int area,int command,object message)
        {
            return new SocketModel(type, area, command, message);
        }

    }
}
