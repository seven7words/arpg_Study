/* ==============================================================================
2  * 功能描述：SelectHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 23:22:03
5  * ==============================================================================*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.logic.match;
using LOLServer.tool;
using NetFrame;

namespace LOLServer.logic.select
{
    /// <summary>
    /// SelectHandler
    /// </summary>
    public class SelectHandler : AbsOnceHandler, IHandleInterface
    {
        /// <summary>
        /// 多线程处理类中，防止数据竞争导致脏数据 使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();
        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<SelectRoom> cache = new ConcurrentStack<SelectRoom>();
        /// <summary>
        /// 房间id自增器
        /// </summary>
        private ConcurrentInteger index = new ConcurrentInteger();

        public SelectHandler()
        {
            EventUtil.createSelect = Create;
            EventUtil.destroySelect = Destroy;
        }

        public void Create(List<int> teamOne, List<int> teamTwo)
        {
            SelectRoom room;
            if (!cache.TryPop(out room))
            {
                room = new SelectRoom();
                //添加唯一id(区域码)
                room.SetArea(index.GetAndAdd());
            }
            //房间数据初始化
            room.Init(teamOne,teamTwo);
            //绑定映射关系
            foreach (int item in teamOne)
            {
                userRoom.TryAdd(item, room.GetArea());
            }
            foreach (int item in teamTwo)
            {
                userRoom.TryAdd(item, room.GetArea());
            }
            roomMap.TryAdd(room.GetArea(), room);
        }

        public void Destroy(int roomId)
        {
            SelectRoom room;
            if (roomMap.TryRemove(roomId, out room))
            {
               //移除橘色和房间之间的绑定关系
               //将房间丢进缓存队列，供下次选择使用
               cache.Push(room);
            }
        }
        public void ClientClose(UserToken token, string error)
        {
            int userId = getUserId(token);
            if (userRoom.ContainsKey(userId))
            {
                int roomId;
                userRoom.TryRemove(userId, out roomId);
                if (roomMap.ContainsKey(roomId))
                {
                    roomMap[roomId].ClientClose(token,error);
                }
            }
        }

        public void ClientConnect(UserToken token)
        {
            
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            //客户端判断
            //if (roomMap.ContainsKey(message.area))
            //{
            //    roomMap[message.area].MessageReceive(token,message);
            //}
            //服务器判断

            int userId = getUserId(token);
            if (userRoom.ContainsKey(userId))
            {
                int roomId = userRoom[userId];
                if (roomMap.ContainsKey(roomId))
                {
                    roomMap[roomId].MessageReceive(token,message);
                }
            }
        }
    }
}