/* ==============================================================================
2  * 功能描述：AbsMultiHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 23:36:31
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFrame;

namespace LOLServer.logic
{
    /// <summary>
    /// AbsMultiHandler
    /// </summary>
    public class AbsMultiHandler:AbsOnceHandler
    {
        public List<UserToken> list = new List<UserToken>();
        /// <summary>
        /// 用户进入当前子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool enter(UserToken token)
        {
            if (list.Contains(token))
            {
                return false;
            }
            list.Add(token);
            return true;
        }
        /// <summary>
        /// 用户是否在此模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool isEntered(UserToken token)
        {
            return list.Contains(token);
        }
        /// <summary>
        /// 用户离开当前子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool leave(UserToken token)
        {
            if (list.Contains(token))
            {
                list.Remove(token);
                return true;
            }
            return false;
        }

        #region 消息群发api


        public void brocast(int command, object message,UserToken exToken =null)
        {
            brocast(GetArea(), command, message,exToken);
        }
        public void brocast(int area, int command, object message, UserToken exToken = null)
        {
            brocast(GetType(), area, command, message,exToken);
        }
        public void brocast(byte type, int area, int command, object message, UserToken exToken = null)
        {
            byte[] value = MessageEncoding.Encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.encode(value);
            foreach (UserToken token in list)
            {
                if (token != exToken)
                {
                    byte[] bs = new byte[value.Length];
                    Array.Copy(value, 0, bs, 0, value.Length);
                    token.write(bs);
                }
               
            }
        }

        #endregion

    }
}