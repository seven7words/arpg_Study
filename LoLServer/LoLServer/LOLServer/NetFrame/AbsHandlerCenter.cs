using System;
using System.Collections.Generic;
using NetFrame;

namespace NetFrame
{

    public abstract class AbsHandlerCenter
    {
        /// <summary>
        /// 客户端链接
        /// </summary>
        /// <param name="token">链接的客户端对象</param>
        public abstract void ClientConnect(UserToken token);
        /// <summary>
        /// 收到客户端消息
        /// </summary>
        /// <param name="token">发送消息的的客户端对象</param>
        /// <param name="message">消息内容</param>
        public abstract void MessageReceive(UserToken token, object message);
        /// <summary>
        /// 客户端断开链接
        /// </summary>
        /// <param name="token">断开的客户端对象</param>
        /// <param name="error">断开的错误信息</param>
        public abstract void ClientClose(UserToken token, string error);
    }
}
