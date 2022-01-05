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
            if(!cache.TryPop(out room))
            { 
                room = new SelectRoom();
                // 设置唯一ID
                room.setArea(index.GetAndAdd());
            }
            // 房间数据初始化
            room.Init(teamOne, teamTwo);

            // 绑定映射关系
            foreach(int item in teamOne)
            {
                userRoomDict.TryAdd(item, room.getArea());
            }
            foreach (int item in teamTwo)
            {
                userRoomDict.TryAdd(item, room.getArea());
            }
            roomMapDict.TryAdd(room.getArea(), room);
        }

        public void Destory(int roomId)
        {
            SelectRoom room;
            if(roomMapDict.TryRemove(roomId,out room))
            {
                int temp = 0;
                // 移除角色和房间之间的绑定关系
                foreach(var item in room.teamOne.Keys)
                {
                    userRoomDict.TryRemove(item, out temp);
                }
                foreach (var item in room.teamTwo.Keys)
                {
                    userRoomDict.TryRemove(item, out temp);
                }
                room.list.Clear();
                

                // 将房间丢进缓存队列， 供下次选择使用
                cache.Push(room);

            }
        }

        public void ClientClose(UserToken token, string error)
        {
            int userId = getUserId(token);
            // 判断当前顽疾是否有房间
            if(userRoomDict.ContainsKey(userId))
            {
                int roomId;
                // 移除并获取玩家所在房间ID
                userRoomDict.TryRemove(userId, out roomId);

                if(roomMapDict.ContainsKey(roomId))
                {
                    roomMapDict[roomId].ClientClose(token, error);
                }
            }
        }


        /// <summary>
        /// 消息转发，转送给SelectRoom 转发
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void MessageReceive(UserToken token, SocketModel message)
        {
            int userId = getUserId(token);
            if(userRoomDict.ContainsKey(userId))
            {
                int roomId = userRoomDict[userId];

                if(roomMapDict.ContainsKey(roomId))
                {
                    roomMapDict[roomId].MessageReceive(token, message);
                }
            }

        }
    }
}
