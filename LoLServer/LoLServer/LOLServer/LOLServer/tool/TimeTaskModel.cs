/* ==============================================================================
2  * 功能描述：TimeTaskModel  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/21 9:37:42
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.tool
{
    /// <summary>
    /// TimeTaskModel
    /// </summary>
    public class TimeTaskModel
    {
        //任务逻辑
        public TimeEvent execute;
        //任务执行的时间
        public long time;
        //任务id
        public int id;

        public TimeTaskModel(int id,TimeEvent execute, long time)
        {
            this.execute = execute;
            this.id = id;
            this.time = time;
        }

        public void Run()
        {
            execute();
        }
    }
}