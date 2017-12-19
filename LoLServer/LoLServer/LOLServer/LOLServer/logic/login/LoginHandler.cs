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
using GameProtocol;
using GameProtocol.DTO;
using LOLServer.biz;
using LOLServer.biz.account;
using LOLServer.tool;

namespace LOLServer.logic.login
{
    /// <summary>
    /// LoginHandler
    /// </summary>
    public class LoginHandler: AbsOnceHandler,IHandleInterface
    {
        private IAccountBiz accountBiz = BizFactory.accountBiz;

        public void ClientClose(UserToken token, string error)
        {
            throw new NotImplementedException();
        }

        public void ClientConnect(UserToken token)
        {

            ExecutorPool.Instance.Execute(delegate ()
           {
               accountBiz.close(token);
           });  
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
            ExecutorPool.Instance.Execute(delegate()
            {
                int result = accountBiz.login(token, value.account, value.password);
                write(token, LoginProtocol.LOGIN_SRES, result);
            });
        }

        public void reg(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {
                int result = accountBiz.create(token, value.account, value.password);
                write(token, LoginProtocol.REG_SRES, result);
            });
        }

        public override byte GetType()
        {
            return Protocol.TYPE_LOGIN;
        }
    }
}