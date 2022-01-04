using System;
using System.Collections.Generic;
using System.Text;

namespace LOLServer.tool
{
    public class TimeTaskModel
    {
        // 任务逻辑
        public TimeEvent execute { private set; get; }
        // 任务执行的时间
        public long time { private set; get; }
        // 任务ID
        public int id { private set; get; }

        public TimeTaskModel( int id,TimeEvent execute, long time)
        {
            this.execute = execute;
            this.time = time;
            this.id = id;
        }
        // 执行任务
        public void run()
        {
            execute();
        }
    }
}
