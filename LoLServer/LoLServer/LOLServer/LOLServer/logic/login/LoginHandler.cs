/* ==============================================================================
2  * 功能描述：LoginHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 19:44:54
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetFrame;
using Protocol;
using Protocol.DTO;

namespace LOLServer.logic.login
{
    /// <summary>
    /// LoginHandler
    /// </summary>
    public class LoginHandler:IHandleInterface
    {

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
                case LoginProtocol.LOGIN_CREQ:
                    login(token,message.GetMessage<AccountInfoDTO>());
                    break;
                case LoginProtocol.REG_CREQ:
                    reg(token, message.GetMessage<AccountInfoDTO>());
                    break;
            }
        }

        public void login(UserToken token,AccountInfoDTO value)
        {
            AccountInfoDTO d = value;
        }

        public void reg(UserToken token, AccountInfoDTO value)
        {
            
        }
    }
}