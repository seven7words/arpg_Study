/* ==============================================================================
2  * 功能描述：ScheduleUtil  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/21 9:30:47
5  * ==============================================================================*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LOLServer.tool
{
    public delegate void TimeEvent();
    /// <summary>
    /// ScheduleUtil
    /// </summary>
    public class ScheduleUtil
    {
        private static ScheduleUtil util;

        public static ScheduleUtil Instance
        {
            get
            {
                if (util == null)
                {
                    util = new ScheduleUtil();
                }
                return util;
            }
        }

        private Timer timer;
        private ConcurrentInteger index = new ConcurrentInteger();
        //等待执行的任务列表
        private ConcurrentDictionary<int,TimeTaskModel> mission = new ConcurrentDictionary<int, TimeTaskModel>();
        //等待移除的任务
        private List<int> removeList = new List<int>();
        private ScheduleUtil()
        {
            timer = new Timer(200);
            timer.Elapsed += CallBack;
            timer.Start();
            
        }

        void CallBack(object sender, ElapsedEventArgs e)
        {
            lock (mission)
            {
                lock (removeList)
                {
                    foreach (int item in removeList)
                    {
                        TimeTaskModel model = null;
                        mission.TryRemove(item,out model);
                    }
                    removeList.Clear();
                    foreach (TimeTaskModel taskModel in mission.Values)
                    {
                        //微秒
                        if (taskModel.time <= DateTime.Now.Ticks)
                        {
                            taskModel.Run();
                            removeList.Add(taskModel.id);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 任务调用毫秒
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int schedule(TimeEvent task,long delay)
        {
            
            //毫秒转微秒
            return schedulemms(task,delay*1000);
        }
        /// <summary>
        /// 任务调用 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"> 系统时间</param>
        /// <returns></returns>
        public int schedule(TimeEvent task, DateTime time)
        {
            long t = time.Ticks - DateTime.Now.Ticks;
            t = Math.Abs(t);

            return schedulemms(task, t);
        }

        public int timeSchedule(TimeEvent task, long time)
        {
            long t = time - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return schedulemms(task, t);
        }
        /// <summary>
        /// 微秒调用内部处理
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private int schedulemms(TimeEvent task, long delay)
        {
            lock (mission)
            {
                int id = index.GetAndAdd();
                TimeTaskModel model = new TimeTaskModel(id, task, DateTime.Now.Ticks + delay);
                mission.TryAdd(id, model);
                return id;
            }
            
        }
        public void RemoveMission(int id)
        {
            lock (removeList)
            {
                removeList.Add(id);
            }
            
        }
    }
}