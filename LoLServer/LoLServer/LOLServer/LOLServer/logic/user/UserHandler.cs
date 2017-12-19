/* ==============================================================================
2  * 功能描述：UserHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:38:15
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameProtocol;
using NetFrame;

namespace LOLServer.logic.user
{
    /// <summary>
    /// UserHandler
    /// </summary>
    public class UserHandler : AbsOnceHandler, IHandleInterface
    {
        public void ClientClose(UserToken token, string error)
        {
            
        }

        public void ClientConnect(UserToken token)
        {
            
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case UserProtocol.CREATE_CREQ:
                    create(token,message.GetMessage<string>());
                    break;
                case UserProtocol.INFO_CREQ:
                    info(token);
                    break;
                case UserProtocol.ONLINE_CREQ:
                    online(token);
                    break;
            }
        }

        private void create(UserToken token, string message)
        {
            
        }

        private void info(UserToken token)
        {
            
        }

        private void online(UserToken token)
        {
            
        }
    }
}