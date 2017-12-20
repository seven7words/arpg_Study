/* ==============================================================================
2  * 功能描述：MatchHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 21:49:05
5  * ==============================================================================*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameProtocol;
using LOLServer.biz;
using LOLServer.biz.user;
using LOLServer.dao.model;
using LOLServer.tool;
using NetFrame;

namespace LOLServer.logic.match
{
    /// <summary>
    ///战斗匹配逻辑处理类
    /// </summary>
    public class MatchHandler: AbsOnceHandler,IHandleInterface
    {
        /// <summary>
        /// 多线程处理类中，防止数据竞争导致脏数据 使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int,int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int,MatchRoom> roomMap = new ConcurrentDictionary<int, MatchRoom>();
        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<MatchRoom> cache = new ConcurrentStack<MatchRoom>();
        /// <summary>
        /// 房间id自增器
        /// </summary>
        private ConcurrentInteger index = new ConcurrentInteger();
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
            switch (message.command)
            {
                case MatchProtocol.ENTER_CREQ:
                    enter(token);
                    break;
                case MatchProtocol.LEAVE_CREQ:
                    break;
            }
        }

        private void enter(UserToken token)
        {
            int userId = getUserId(token);
            //判断玩家当前是否在匹配队列中
            if (!userRoom.ContainsKey(userId))
            {
                MatchRoom room = null;
                //当前是否有等待中的房间
                if (roomMap.Count > 0)
                {
                    //便利当前所有等待中的房间
                    foreach (MatchRoom item in roomMap.Values)
                    {
                        //如果没满员
                        if (item.teamMax * 2 > item.teamOne.Count + item.teamTwo.Count)
                        {
                            room = item;
                            //如果队伍一没有埋怨，进入
                            if (room.teamOne.Count < room.teamMax)
                            {
                                room.teamOne.Add(userId);
                            }
                            else
                            {
                                room.teamTwo.Add(userId);
                            }
                            //添加玩家与房间的映射关系
                            userRoom.TryAdd(userId, room.id);
                            break;
                        }
                    }
                    //走到这里说明等待中的房间全部满员了
                    if (cache.Count > 0)
                    {
                        cache.TryPop(out room);
                        room.teamOne.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);

                    }
                    else
                    {
                        room = new MatchRoom();
                        room.id = index.GetAndAdd();
                        room.teamOne.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);
                    }
                }
                else
                {
                    //没有等待中的房间
                    //直接从缓存列表中找到可用房间，或者创建新房间
                    if (cache.Count > 0)
                    {
                        cache.TryPop(out room);
                        room.teamOne.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);

                    }
                    else
                    {
                        room = new MatchRoom();
                        room.id = index.GetAndAdd();
                        room.teamOne.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);
                    }

                }
                //不管森么方式进入房间，判断房间是否满员
                //满了就开始选人，并将当前房间丢入缓存队列
                if (room.teamOne.Count == room.teamTwo.Count && room.teamOne.Count == room.teamMax)
                {
                    //这里通知选人模块，开始选人了
                    //TODO:
                    //移除玩家与房间映射
                    foreach (int item in room.teamOne)
                    {
                        int i;
                        userRoom.TryRemove(item, out i);

                    }
                    foreach (int item in room.teamTwo)
                    {
                        int i;
                        userRoom.TryRemove(item, out i);

                    }
                    //重置房间数据，供下次使用
                    room.teamOne.Clear();
                    room.teamTwo.Clear();
                    //将房间从等待房间列表中移除
                    roomMap.TryRemove(room.id, out room);
                    //将房间丢进缓存表 供下次使用
                    cache.Push(room);

                }
            }
        }
    }
}