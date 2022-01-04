using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LOLServer.tool
{
    public class ConcurrentInteger
    {
        int value;

        public ConcurrentInteger()
        {
            value = 0;
        }

        public ConcurrentInteger(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// 自增并返回
        /// </summary>
        /// <returns></returns>
        public int GetAndAdd()
        {
            lock(this)
            {
                value++;
            }
            return value;
        }

        /// <summary>
        /// 自减并返回
        /// </summary>
        /// <returns></returns>
        public int GetAndReduce()
        {
            lock (this)
            {
                value--;
            }
            return value;
        }

        public void reset()
        {
            lock(this)
            {
                value = 0;
            }
        }

        public int get()
        {
            return value;
        }
    }
}
