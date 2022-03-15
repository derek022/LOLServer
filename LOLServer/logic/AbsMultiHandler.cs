using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic
{
    public class AbsMultiHandler : AbsOnceHandler
    {
        public List<UserToken> list = new List<UserToken>();

        public bool enter(UserToken token)
        {
            if(list.Contains(token))
            {
                return false;
            }
            list.Add(token);
            return true;
        }

        public bool isEntered(UserToken token)
        {
            return list.Contains(token);
        }

        public bool leave(UserToken token)
        {
            if(list.Contains(token))
            {
                list.Remove(token);
                return true;
            }
            return false;
        }


        #region 群发消息API

        public void broadcast(int command,object message,UserToken exToken = null)
        {
            broadcast(GetArea(), command, message, exToken);
        }

        public void broadcast(int area,int command, object message, UserToken exToken = null)
        {
            broadcast(GetType(),area, command, message, exToken);
        }

        public void broadcast(byte type,int area, int command, object message, UserToken exToken = null)
        {
            // 提前将数据编码，然后直接将二进制数据发送
            byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.encode(value);

            foreach(UserToken item in list)
            {
                if(item != exToken)
                {
                    byte[] bs = new byte[value.Length];
                    Array.Copy(value, 0, bs, 0, value.Length);
                    // 写入之后，二进制数据变空，复制一份出来
                    item.Write(bs);
                }
            }
        }


        #endregion
    }
}
