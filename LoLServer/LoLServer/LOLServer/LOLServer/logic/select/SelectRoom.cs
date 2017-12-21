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
using GameProtocol;
using GameProtocol.DTO;
using LOLServer.dao.model;
using LOLServer.tool;
using NetFrame;

namespace LOLServer.logic.select
{
    /// <summary>
    /// SelectRoom
    /// </summary>
    public class SelectRoom:AbsMultiHandler,IHandleInterface
    {
        public  ConcurrentDictionary<int,SelectModel> teamOne = new ConcurrentDictionary<int, SelectModel>();
        public ConcurrentDictionary<int, SelectModel> teamTwo = new ConcurrentDictionary<int, SelectModel>();
        //当前进入房间的人数
        private int enterCount = 0;
        //当前定时任务id
        private int missionId = -1;
        public void ClientClose(UserToken token, string error)
        {
            leave(token);
            Destroy();
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
            //初始化完毕 开始定时任务，设定30秒后没有进入到选择界面的时候，直接解散此次匹配
            missionId = ScheduleUtil.Instance.schedule(delegate
            {
                //30秒后判断进入情况，如果不是全员进入，解散房间
                if (enterCount < teamOne.Count + teamTwo.Count)
                {
                    Destroy();
                }
                else
                {
                    missionId = ScheduleUtil.Instance.schedule(delegate
                    {
                        bool selectAll = true;
                        foreach (SelectModel item in this.teamOne.Values)
                        {
                            if (item.hero == -1)
                            {
                                selectAll = false;
                                break;
                            }
                        }
                        if (selectAll)
                        {
                            foreach (SelectModel item in this.teamTwo.Values)
                            {
                                if (item.hero == -1)
                                {
                                    selectAll = false;
                                    break;
                                }
                            }
                        }
                        if (selectAll)
                        {
                            //全部选了，只是有人没有开始按准备按钮，开支战斗
                        }else
                        {
                           Destroy();
                        }
                        missionId = -1;
                    }, 30 * 1000);
                }

            }, 30 * 1000);
        }

        private void Destroy()
        {
            
            brocast(SelectProtocol.DESTROY_BRO, null);
            EventUtil.destroySelect(GetArea());
            if (missionId != -1)
            {
                ScheduleUtil.Instance.RemoveMission(missionId);
            }
            enterCount = 0;

        }
        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case SelectProtocol.ENTER_CREQ:
                    Enter(token);
                    break;
                case SelectProtocol.SELECT_CREQ:
                    Select(token,message.GetMessage<int>());
                    break;
                case SelectProtocol.TALK_CREQ:
                    break;
                case SelectProtocol.READY_CREQ:
                    break;
                    
            }
        }

        private void Select(UserToken token, int value)
        {
            //判断玩家是否在房间里
            if (base.isEntered(token))
            {
                return;
            }
            //判断玩家是否拥有此英雄
            UserModel user = getUser(token);
            if (!user.heroList.Contains(value))
            {
                write(token,SelectProtocol.SELECT_SRES,null);
                return;
            }

            SelectModel selectModel = null;
            //判断队友是否已经选择
            if (teamOne.ContainsKey(user.id))
            {
                foreach (var item in teamOne.Values)
                {
                    if (item.hero == value)
                        return;
                }
                selectModel = teamOne[user.id];

            }
            else
            {

                foreach (var item in teamTwo.Values)
                {
                    if (item.hero == value)
                        return;
                }
                selectModel = teamTwo[user.id];
            }
            //选择成功，通知房间所有人变更数据
            selectModel.hero = value;
            brocast(SelectProtocol.SELECT_BRO,selectModel);
        }
        private void Enter(UserToken token)
        {
            //判断用户所在的房间并对其进入状态进行修改
            int userId = getUserId(token);
            if (teamOne.ContainsKey(userId))
            {
                teamOne[userId].enter = true;
            }
            else if (teamTwo.ContainsKey(userId))
            {
                teamTwo[userId].enter = true;
            }
            else
            {
                return;
            }
            //判断用户是否已经在房间 不在则计算累加，否则无视
            if (base.enter(token))
            {
                enterCount++;
            }
           //进入成功 发送房间信息给进入的玩家，并通知在房间内的其他玩家
            SelectRoomDTO dto = new SelectRoomDTO();
            dto.teamOne = teamOne.Values.ToArray();
            dto.teamTwo = teamTwo.Values.ToArray();
            write(token,SelectProtocol.ENTER_SRES,dto);
            brocast(SelectProtocol.ENTER_EXBRO,userId,token);
        }
        public override byte GetType()
        {
            return GameProtocol.Protocol.TYPE_SELECT;
        }
    }
}