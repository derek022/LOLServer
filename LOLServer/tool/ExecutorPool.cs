using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LOLServer.tool
{
    /// <summary>
    /// 单线程事件委托
    /// </summary>
    public delegate void ExecutorDelegate();
    /// <summary>
    /// 单线程处理对象，将所有事物处理调用 通过此处调用
    /// </summary>
    public class ExecutorPool
    {
        private Object thisLock = new Object();
        public static ExecutorPool Instance { get { return instance; } }

        private static ExecutorPool instance;
        static ExecutorPool()
        {
            instance = new ExecutorPool();
        }
        private ExecutorPool(){ }


        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="d"></param>
        public void Execute(ExecutorDelegate d)
        {
            lock (thisLock)  
            {
                d();
            }
        }
    }
}
