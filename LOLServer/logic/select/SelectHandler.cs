using LOLServer.logic.match;
using LOLServer.logic.user;
using LOLServer.tool;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic.select
{
    public class SelectHandler : AbsOnceHandler, HandlerInterface
    {

        /// <summary>
        /// 多线程处理，防止数据竞争导致脏数据，使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoomDict = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间ID与模型映射
        /// </summary>
        ConcurrentDictionary<int, SelectRoom> roomMapDict = new ConcurrentDictionary<int, SelectRoom>();

        /// <summary>
        /// 回收利用过得房间对象再次使用，减少gc性能开销
        /// </summary>
        ConcurrentStack<SelectRoom> cache = new ConcurrentStack<SelectRoom>();

        ConcurrentInteger index = new ConcurrentInteger();


        public SelectHandler()
        {
            EventUtil.createSelect = Create;
            EventUtil.destorySelect = Destory;
        }

        public void Create(List<int> teamOne, List<int> teamTwo)
        {
            SelectRoom room;
            if(cache.TryPop(out room))
            {

            }
            else
            {
                room = new SelectRoom();



            }
        }

        public void Destory(int roomId)
        {

        }

        public void ClientClose(UserToken token, string error)
        {
            
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
         
        }
    }
}
