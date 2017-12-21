/* ==============================================================================
2  * 功能描述：AbsOnceHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 23:29:16
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.biz;
using LOLServer.biz.user;
using LOLServer.dao.model;
using NetFrame;

namespace LOLServer.logic
{
    /// <summary>
    /// AbsOnceHandler
    /// </summary>
    public class AbsOnceHandler
    {
        public IUserBiz userBiz = BizFactory.userBiz;
        private byte type;
        private int area;

        public void SetArea(int area)
        {
            this.area = area;
        }

        public virtual int GetArea()
        {
            return area;
        }
        public void SetType(byte type)
        {
            this.type = type;
        }

        public virtual byte GetType()
        {
            return type;
        }
        /// <summary>
        /// 通过链接获取用户模型数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel getUser(UserToken token)
        {
            return userBiz.get(token);
        }
        /// <summary>
        /// 通过id获取用户模型数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel getUser(int id)
        {
            return userBiz.get(id);
        }
        /// <summary>
        /// 通过链接获取用户id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int getUserId(UserToken token)
        {
            UserModel user = getUser(token);
            if (user == null) return -1;
            return getUser(token).id;
        }
        /// <summary>
        /// 通过用户id获取链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserToken getToken(int id)
        {
            return userBiz.getToken(id);
        }
        #region 通过连接对象发送

        public void write(UserToken token, int command)
        {
            write(token,command,null);
        }
        public void write(UserToken token, int command, object message)
        {
            write(token,GetArea(),command,message);
        }
        public void write(UserToken token, int area, int command, object message)
        {
            write(token,GetType(),GetArea(),command,message);
        }
        public void write(UserToken token, byte type, int area, int command, object message)
        {
            byte[] value = MessageEncoding.Encode(CreateSocketModel(type,area,command,message));
            value = LengthEncoding.encode(value);
            token.write(value);
        }

        #endregion

        #region 通过玩家id发送

        public void write(int id, int command)
        {
            write(id,command,null);
        }
        public void write(int id, int command, object message)
        {
            write(id,GetArea(),command,message);
        }
        public void write(int id, int area, int command, object message)
        {
            write(id,GetType(),area,command,message);
        }
        public void write(int id, byte type, int area, int command, object message)
        {
            UserToken token=   getToken(id);
            if (token == null) return;
            write(token,type,area,command,message);
        }

        public void writeToUsers(int[] users, byte type, int area, int command, object message)
        {
            byte[] value = MessageEncoding.Encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.encode(value);
            foreach (int item in users)
            {
                UserToken token = userBiz.getToken(item);
                if(token==null)continue;
                byte[] bs = new byte[value.Length];
                Array.Copy(value,0,bs,0,value.Length);
                token.write(bs);
            }
        }
        #endregion


        public SocketModel CreateSocketModel(byte type, int area, int command, object message)
        {
            return new SocketModel( type,  area,  command,  message);
        }

    }
}