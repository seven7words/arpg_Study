/* ==============================================================================
2  * 功能描述：FightHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/28 20:58:46
5  * ==============================================================================*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using GameProtocol.DTO;
using LOLServer.logic.@select;
using LOLServer.tool;
using NetFrame;

namespace LOLServer.logic.fight
{
    /// <summary>
    /// FightHandler
    /// </summary>
    public class FightHandler:AbsMultiHandler,IHandleInterface
    { /// <summary>
        /// 多线程处理类中，防止数据竞争导致脏数据 使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();
        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<FightRoom> cache = new ConcurrentStack<FightRoom>();
        /// <summary>
        /// 房间id自增器
        /// </summary>
        private ConcurrentInteger index = new ConcurrentInteger();

        public  FightHandler()
        {
            EventUtil.createFight = Create;
            EventUtil.destroyFight = Destroy;
        }
        /// <summary>
        /// 战斗结束，房间移除
        /// </summary>
        /// <param name="roomId"></param>
        public void Destroy(int roomId)
        {
            FightRoom room;
            if (roomMap.TryRemove(roomId, out room))
            {

                int temp = 0;
                ////移除橘色和房间之间的绑定关系
                //foreach (int item in room.teamOne.Keys)
                //{
                //    int temp = 0;
                //    userRoom.TryRemove(item, out temp);

                //}
                //foreach (int item in room.teamTwo.Keys)
                //{
                //    int temp = 0;
                //    userRoom.TryRemove(item, out temp);

                //}
                //room.list.Clear();
                //room.teamOne.Clear();
                //room.teamTwo.Clear();
                //将房间丢进缓存队列，供下次选择使用
                cache.Push(room);
            }

        }
        /// <summary>
        /// 创建战场
        /// </summary>
        /// <param name="teamOne"></param>
        /// <param name="teamTwo"></param>
        public void Create(SelectModel[] teamOne, SelectModel[] teamTwo)
        {
            FightRoom room;
            if (!cache.TryPop(out room))
            {
                room = new FightRoom();
                //添加唯一id(区域码)
                room.SetArea(index.GetAndAdd());
            }
            //房间数据初始化
            room.Init(teamOne, teamTwo);
            //绑定映射关系
            foreach (SelectModel item in teamOne)
            {
                userRoom.TryAdd(item.userId, room.GetArea());
            }
            foreach (SelectModel item in teamTwo)
            {
                userRoom.TryAdd(item.userId, room.GetArea());
            }
            roomMap.TryAdd(room.GetArea(), room);
        }

      
        public void ClientClose(UserToken token, string error)
        {
            throw new NotImplementedException();
        }

        public void ClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            throw new NotImplementedException();
        }
    }
}