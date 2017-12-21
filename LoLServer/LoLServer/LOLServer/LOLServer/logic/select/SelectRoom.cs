/* ==============================================================================
2  * 功能描述：SelectRoom  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 23:30:02
5  * ==============================================================================*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameProtocol.DTO;
using LOLServer.tool;
using NetFrame;

namespace LOLServer.logic.select
{
    /// <summary>
    /// SelectRoom
    /// </summary>
    public class SelectRoom:AbsMultiHandler,IHandleInterface
    {
        public  ConcurrentDictionary<int,SelectMode> teamOne = new ConcurrentDictionary<int, SelectMode>();
        public ConcurrentDictionary<int, SelectMode> teamTwo = new ConcurrentDictionary<int, SelectMode>();


        public void ClientClose(UserToken token, string error)
        {
            
        }

        public void ClientConnect(UserToken token)
        {
           
        }

        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            this.teamOne.Clear();
            this.teamTwo.Clear();
            foreach (int item in teamOne)
            {
                SelectMode select = new SelectMode();
                select.userId = item;
                select.name = getUser(item).name;
                select.hero = -1;
                select.enter = false;
                select.ready = false;
                this.teamOne.TryAdd(item, select);
            }
            foreach (int item in teamTwo)
            {
                SelectMode select = new SelectMode();
                select.userId = item;
                select.name = getUser(item).name;
                select.hero = -1;
                select.enter = false;
                select.ready = false;
                this.teamTwo.TryAdd(item, select);
            }
            //初始化完毕 开始定时任务，设定30秒后没有进入到选择界面的时候，直接解散此次匹配
            ScheduleUtil.Instance.schedule(delegate
            {
                //30秒后判断进入情况，如果不是全员进入，解散房间

            }, 30 * 1000);
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            throw new NotImplementedException();
        }

        public override byte GetType()
        {
            return GameProtocol.Protocol.TYPE_SELECT;
        }
    }
}