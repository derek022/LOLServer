using GameProtocol;
using GameProtocol.dto;
using LOLServer.dao.model;
using LOLServer.tool;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.logic.select
{
    public class SelectRoom : AbsMultiHandler, HandlerInterface
    {
        public ConcurrentDictionary<int, SelectModel> teamOne = new ConcurrentDictionary<int, SelectModel>();
        public ConcurrentDictionary<int, SelectModel> teamTwo = new ConcurrentDictionary<int, SelectModel>();

        public List<int> readyList = new List<int>();

        // 当前进入房间的人数
        int enterCount = 0;

        int missionId = -1;

        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            // 房间重复利用，先清空历史数据。
            teamOne.Clear();
            teamTwo.Clear();
            enterCount = 0;

            foreach(int item in teamOne)
            {
                SelectModel select = new SelectModel();
                select.userId = item;
                select.name = getUser(item).name;
                select.hero = -1;
                select.enter = false;
                select.ready = false;
                this.teamOne.TryAdd(item, select);
            }
            foreach (int item in teamTwo)
            {
                SelectModel select = new SelectModel();
                select.userId = item;
                select.name = getUser(item).name;
                select.hero = -1;
                select.enter = false;
                select.ready = false;
                this.teamTwo.TryAdd(item, select);
            }

            // 初始化完毕，开始定时任务，设定 30秒后没有进入选择界面的时候 直接解散此次匹配
            ScheduleUtil.Instance.schedule(() =>
            {
                //30s后，如果没有玩家全部进入房间，解散房间
                if (enterCount < teamOne.Count + teamTwo.Count)
                {
                    destory();
                }
                else
                {
                    missionId = ScheduleUtil.Instance.schedule(() =>
                    {
                        bool isAllSelect = true;
                        foreach(var item in this.teamOne.Values)
                        {
                            if(item.hero == -1)
                            {
                                isAllSelect = false;
                                break;
                            }
                        }
                        if(isAllSelect)
                        {
                            foreach (var item in this.teamTwo.Values)
                            {
                                if (item.hero == -1)
                                {
                                    isAllSelect = false;
                                    break;
                                }
                            }
                        }

                        if(isAllSelect)
                        {
                            // 全部都选择了，开始战斗
                            startFight();

                        }
                        else
                        {
                            destory();
                        }

                        missionId = -1;

                    }, 30 * 1000);
                }

            }, 30 * 1000);
        }

        private void destory()
        {
            
            // 没有全部进入,通知所有人
            broadcast(SelectProtocol.DESTORY_BRO, null);
            EventUtil.destorySelect(getArea());

            //当前有定时任务 则进行关闭
            if (missionId != -1)
            {
                ScheduleUtil.Instance.removeMission(missionId);
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            //调用离开方法 让此连接不再接收网络消息
            leave(token);

            destory();
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch(message.command)
            {
                case SelectProtocol.ENTER_CREQ:
                    enterRoom(token);
                    break;

                case SelectProtocol.SELECT_CREQ:
                    select(token, message.GetMessage<int>());
                    break;

                case SelectProtocol.TALK_CREQ:
                    talk(token, message.GetMessage<string>());
                    break;

                case SelectProtocol.READY_CREQ:
                    ready(token);
                    break;
            }
        }



        public override byte getType()
        {
            return Protocol.TYPE_SELECT;
        }


        #region 消息方法
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="token"></param>
        private void enterRoom(UserToken token)
        {
            int userId = getUserId(token);
            if (teamOne.ContainsKey(userId))
            {
                teamOne[userId].enter = true;
            }
            else if (teamTwo.ContainsKey(userId))
            {
                teamTwo[userId].enter = true;
            }
            else
            {
                return;
            }

            if (base.enter(token))
            {
                enterCount++;
            }
            SelectRoomDTO dto = new SelectRoomDTO();
            dto.teamOne = this.teamOne.Values.ToArray();
            dto.teamTwo = this.teamTwo.Values.ToArray();

            Write(token, SelectProtocol.ENTER_SRES, dto);
            broadcast(SelectProtocol.ENTER_EXBRO, userId, token);

        }

        /// <summary>
        /// 选择英雄
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        private void select(UserToken token, int value)
        {
            // 判断玩家是否在房间中
            if (!base.isEntered(token))
            {
                return;
            }

            // 判断玩家是否拥有此英雄
            User user = getUser(token);
            if (!user.heroList.Contains(value))
            {
                Write(token, SelectProtocol.SELECT_SRES, null);
                return;
            }

            SelectModel selectModel = null;
            // 判断该英雄是否已经被队友选中
            if (teamOne.ContainsKey(user.id))
            {
                foreach (SelectModel item in teamOne.Values)
                {
                    if (item.hero == value)
                    {
                        Write(token, SelectProtocol.SELECT_SRES, null);
                        return;
                    }
                    selectModel = teamOne[user.id];
                }
            }
            else
            {
                foreach (SelectModel item in teamTwo.Values)
                {
                    if (item.hero == value)
                    {
                        Write(token, SelectProtocol.SELECT_SRES, null);
                        return;
                    }
                    selectModel = teamTwo[user.id];
                }
            }

            //选择成功 通知房间所有人变更数据
            selectModel.hero = value;
            broadcast(SelectProtocol.SELECT_BRO, selectModel);

        }

        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="token"></param>
        /// <param name="content">内容</param>
        private void talk(UserToken token,string content)
        {
            if(!base.isEntered(token))
            {
                return;
            }
            User user = getUser(token);
            broadcast(SelectProtocol.TALK_BRO, user.name + ":" + content);
        }

        /// <summary>
        ///  准备开始战斗,玩家选好英雄，准备状态
        /// </summary>
        /// <param name="token"></param>
        private void ready(UserToken token)
        {
            if(!base.isEntered(token))
            {
                return;
            }

            int userId = getUserId(token);
            // 无需重复准备
            if(readyList.Contains(userId))
            {
                return;
            }

            SelectModel sm = null;
            if (teamOne.ContainsKey(userId))
            {
                sm = teamOne[userId];
            }
            else
            {
                sm = teamTwo[userId];
            }

            if(sm.hero != -1)
            {
                sm.ready = true;
                broadcast(SelectProtocol.READY_BRO, sm);

                readyList.Add(userId);
                if(readyList.Count >= teamOne.Count + teamTwo.Count)
                {
                    // 所有人都准备完成，开始战斗
                    startFight();
                }

            }
        }
        
        /// <summary>
        /// 开始战斗
        /// </summary>
        private void startFight()
        {
            if (missionId != -1)
            {
                ScheduleUtil.Instance.removeMission(missionId);
                missionId = -1;
            }

            //通知战斗模块 创建战斗房间
            //EventUtil.createFight(teamOne.Values.ToArray(), teamTwo.Values.ToArray());
            //broadcast(SelectProtocol.FIGHT_BRO, null);

            //通知选择房间管理器 销毁当前房间
            EventUtil.destorySelect(getArea());
        }

        #endregion

    }
}
