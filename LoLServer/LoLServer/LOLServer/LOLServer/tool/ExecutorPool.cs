/* ==============================================================================
2  * 功能描述：ExecutorPool  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 22:57:11
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LOLServer.tool
{
    /// <summary>
    /// 单线程处理对象 将所有事物处理调用 通过此处调用
    /// </summary>
    public class ExecutorPool
    {
        /// <summary>
        /// 单线程事件委托
        /// </summary>
        public delegate void ExecutorDelegate();
        /// <summary>
        /// 线程同步锁
        /// </summary>
        Mutex tex = new Mutex();
        private static ExecutorPool instance;
        /// <summary>
        /// 单例对象
        /// </summary>
        public static ExecutorPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExecutorPool();
                }
                return instance;
            }
        }
        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="d"></param>
        public void Execute(ExecutorDelegate d)
        {
            lock (this)
            {
                tex.WaitOne();
                d();
                tex.ReleaseMutex();
            }
        }
    }
}