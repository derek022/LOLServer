using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace LOLServer.tool
{
    /// <summary>
    /// 委托，事件方法
    /// </summary>
    public delegate void TimeEvent();

    /// <summary>
    /// 定时任务工具类
    /// </summary>
    public class ScheduleUtil
    {

        Timer timer;
        private ConcurrentInteger index = new ConcurrentInteger();
        //等待执行的任务表
        private Dictionary<int, TimeTaskModel> mission = new Dictionary<int, TimeTaskModel>();
        //等待移除的任务列表
        private List<int> removelist = new List<int>();

        #region 单例
        private static ScheduleUtil util;
        static ScheduleUtil()
        {
            util = new ScheduleUtil();
        }
        private ScheduleUtil() {
            timer = new Timer(15);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public static ScheduleUtil Instance
        {
            get
            {
                return util;
            }
        }
        #endregion


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (mission)
            {
                lock(removelist)
                {
                    foreach(int item in removelist)
                    {
                        mission.Remove(item);
                    }
                    removelist.Clear();

                    foreach(TimeTaskModel item in mission.Values)
                    {
                        // DateTime.Now.Ticks  100纳秒
                        if (item.time <= DateTime.Now.Ticks)
                        {
                            item.run();
                            removelist.Add(item.id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 延时任务 毫秒
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int schedule(TimeEvent task,long delay)
        {
            //毫秒转 100 ns
            return schedulemms(task, delay * 1000 * 10);
        }

        /// <summary>
        /// 微秒时间轴
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private int schedulemms(TimeEvent task,long delay)
        {
            lock (mission)
            {
                int id = index.GetAndAdd();

                TimeTaskModel model = new TimeTaskModel(id, task, DateTime.Now.Ticks + delay);
                mission.Add(id, model);
                return id;
            }
        }

        /// <summary>
        /// 移除任务ID
        /// </summary>
        /// <param name="id"></param>
        public void removeMission(int id)
        {
            lock(removelist)
            {
                removelist.Remove(id);
            }
        }

        /// <summary>
        /// 延时任务  时间
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int schedule(TimeEvent task, DateTime time)
        {
            long t = time.Ticks - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return schedulemms(task, t);
        }

        /// <summary>
        /// DateTime 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time">DateTime 的纳秒值</param>
        /// <returns></returns>
        public int timeSchedule(TimeEvent task, long time)
        {
            long t = time - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return schedulemms(task, t);
        }

    }
}
