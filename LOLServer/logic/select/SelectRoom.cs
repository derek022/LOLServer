using GameProtocol;
using GameProtocol.dto;
using LOLServer.tool;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.logic.select
{
    public class SelectRoom : AbsMultiHandler, HandlerInterface
    {
        public ConcurrentDictionary<int, SelectModel> teamOne = new ConcurrentDictionary<int, SelectModel>();
        public ConcurrentDictionary<int, SelectModel> teamTwo = new ConcurrentDictionary<int, SelectModel>();


        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            // 房间重复利用，先清空历史数据。
            teamOne.Clear();
            teamTwo.Clear();

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

            }, 30 * 1000);
        }


        public void ClientClose(UserToken token, string error)
        {
            
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            
        }

        public override byte getType()
        {
            return Protocol.TYPE_SELECT;
        }
    }
}
