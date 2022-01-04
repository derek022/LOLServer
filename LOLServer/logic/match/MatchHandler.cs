using GameProtocol;
using LOLServer.biz;
using LOLServer.dao.model;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using LOLServer.tool;

namespace LOLServer.logic.match
{
    public class MatchHandler : AbsOnceHandler, HandlerInterface
    {
        /// <summary>
        /// 多线程处理，防止数据竞争导致脏数据，使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoomDict = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间ID与模型映射
        /// </summary>
        ConcurrentDictionary<int, MatchRoom> roomMapDict = new ConcurrentDictionary<int, MatchRoom>();

        /// <summary>
        /// 回收利用过得房间对象再次使用，减少gc性能开销
        /// </summary>
        ConcurrentStack<MatchRoom> cache = new ConcurrentStack<MatchRoom>();

        ConcurrentInteger index = new ConcurrentInteger(0);

        public void ClientClose(UserToken token, string error)
        {
            leave(token);
        }


        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch(message.command)
            {
                case MatchProtocol.ENTER_CREQ:
                    enter(token);
                    break;

                case MatchProtocol.LEAVE_CREQ:
                    leave(token);
                    break;
            }
        }


        private void enter(UserToken token)
        {
            int userID = getUserId(token);
            Console.WriteLine("用户开始匹配 " + userID);
            // 判断玩家当前是否正在匹配队列中
            if (!userRoomDict.ContainsKey(userID))
            {

                MatchRoom room = null;
                bool isEnter = false;
                // 当前是否有等待中的房间
                if (roomMapDict.Count > 0)
                {
                    // 遍历当前所有等待中的房间
                    foreach(MatchRoom item in roomMapDict.Values)
                    {
                        // 如果没有满员
                        if (item.teamMax * 2 > item.teamOne.Count + item.teamTwo.Count)
                        {
                            room = item;
                            // 如果队伍1没有满员，进入队伍1 否者进入队伍2
                            if(item.teamOne.Count<item.teamMax)
                            {
                                room.teamOne.Add(userID);
                            }
                            else
                            {
                                room.teamTwo.Add(userID);
                            }
                            // 添加玩家与房间的映射关系
                            userRoomDict.TryAdd(userID, room.id);
                            isEnter = true;
                            break;
                        }
                    }

                    if(!isEnter)
                    {
                        if(cache.Count>0)
                        {
                            cache.TryPop(out room);
                            room.teamOne.Add(userID);
                            roomMapDict.TryAdd(room.id, room);
                            userRoomDict.TryAdd(userID, room.id);
                        }
                        else
                        {
                            room = new MatchRoom();
                            room.id = index.GetAndAdd();
                            room.teamOne.Add(userID);
                            roomMapDict.TryAdd(room.id, room);
                            userRoomDict.TryAdd(userID, room.id);
                        }

                    }

                }
                else
                {
                    // 没有等待中的房间

                    if (cache.Count > 0)
                    {
                        cache.TryPop(out room);
                        room.teamOne.Add(userID);
                        roomMapDict.TryAdd(room.id, room);
                        userRoomDict.TryAdd(userID, room.id);
                    }
                    else
                    {
                        room = new MatchRoom();
                        room.id = index.GetAndAdd();
                        room.teamOne.Add(userID);
                        roomMapDict.TryAdd(room.id, room);
                        userRoomDict.TryAdd(userID, room.id);
                    }

                }

                // 不管什么方式进入房间，判断房间是否满员，满了就开始选人并将当前房间丢进缓存队列
                if (room.teamOne.Count == room.teamTwo.Count && room.teamOne.Count == room.teamMax)
                {
                    // 通知选人模块，开始选人了
                    EventUtil.createSelect(room.teamOne, room.teamTwo);


                    WriteToUsers(room.teamOne.ToArray(), Protocol.TYPE_SELECT, getArea(), MatchProtocol.ENTER_SELECT_BRO, null);
                    WriteToUsers(room.teamTwo.ToArray(), Protocol.TYPE_SELECT, getArea(), MatchProtocol.ENTER_SELECT_BRO, null);


                    // 移除玩家与房间的映射
                    foreach (int item in room.teamOne)
                    {
                        int i;
                        userRoomDict.TryRemove(item, out i);
                    }
                    foreach (int item in room.teamTwo)
                    {
                        int i;
                        userRoomDict.TryRemove(item, out i);
                    }
                    room.teamOne.Clear();
                    room.teamTwo.Clear();

                    roomMapDict.TryRemove(room.id, out room);
                    cache.Push(room);

                }

            }

        }


        private void leave(UserToken token)
        {
            int userId = getUserId(token);
            Console.WriteLine("用户取消匹配 " + userId);
            if(!userRoomDict.ContainsKey(userId))
            {
                return;
            }
            // 获取用户所在房间ID
            int roomId = userRoomDict[userId];
            if (roomMapDict.ContainsKey(roomId))
            {
                // 根据用户所在的队伍 进行移除
                MatchRoom room = roomMapDict[roomId];
                if (room.teamOne.Contains(userId))
                {
                    room.teamOne.Remove(userId);
                }
                else if (room.teamTwo.Contains(userId))
                {
                    room.teamTwo.Remove(userId);
                }
                //移除用户与房间之间的映射关系
                userRoomDict.TryRemove(userId,out roomId);
                //如果当前用户为此房间最后一人 则移除房间 并丢进缓存队列
                if (room.teamOne.Count + room.teamTwo.Count == 0)
                {
                    roomMapDict.TryRemove(roomId, out room);
                    cache.Push(room);
                }
            }
        }

        public override byte getType()
        {
            return Protocol.TYPE_MATCH;
        }
    }
}
