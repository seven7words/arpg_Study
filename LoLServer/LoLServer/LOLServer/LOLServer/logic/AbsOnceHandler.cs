/* ==============================================================================
2  * 功能描述：AbsOnceHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 23:29:16
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFrame;

namespace LOLServer.logic
{
    /// <summary>
    /// AbsOnceHandler
    /// </summary>
    public class AbsOnceHandler
    {
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

        }
        public void write(int id, int command, object message)
        {

        }
        public void write(int id, int area, int command, object message)
        {

        }
        public void write(int id, byte type, int area, int command, object message)
        {

        }

        #endregion

        public SocketModel CreateSocketModel(byte type, int area, int command, object message)
        {
            return new SocketModel( type,  area,  command,  message);
        }
    }
}