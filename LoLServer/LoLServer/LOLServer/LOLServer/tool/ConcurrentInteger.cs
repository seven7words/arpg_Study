/* ==============================================================================
2  * 功能描述：ConcurrentInteger  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 22:21:01
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LOLServer.tool
{
    /// <summary>
    /// ConcurrentInteger
    /// </summary>
    public class ConcurrentInteger
    {
        private int value;
        Mutex tex = new Mutex();

        public ConcurrentInteger()
        {
            
        }

        public ConcurrentInteger(int value)
        {
            this.value = value;
        }
        /// <summary>
        /// 自增
        /// </summary>
        /// <returns></returns>
        public int GetAndAdd()
        {
            lock (this)
            {
                tex.WaitOne();
                value++;               
                tex.ReleaseMutex();
                return value;
            }
        }
        /// <summary>
        /// 自减
        /// </summary>
        /// <returns></returns>
        public int GetAndReduce()
        {
            lock (this)
            {
                tex.WaitOne();
                value--;
                tex.ReleaseMutex();
                return value;
            }
        }
        /// <summary>
        /// 充值value为0
        /// </summary>
        public void reset()
        {
            lock (this)
            {
                tex.WaitOne();
                value = 0;
                tex.ReleaseMutex();
                
            }
        }

        public int get()
        {
            return value;
        }
    }
}