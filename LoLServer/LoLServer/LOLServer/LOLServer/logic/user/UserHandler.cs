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
using GameProtocol.DTO;
using LOLServer.biz;
using LOLServer.biz.user;
using LOLServer.dao.model;
using LOLServer.tool;
using NetFrame;

namespace LOLServer.logic.user
{
    /// <summary>
    /// UserHandler
    /// </summary>
    public class UserHandler : AbsOnceHandler, IHandleInterface
    {
        private IUserBiz userBiz = BizFactory.userBiz;
        public void ClientClose(UserToken token, string error)
        {
            userBiz.offline(token);
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

            ExecutorPool.Instance.Execute(delegate ()
            {
                write(token,UserProtocol.CREATE_SRES, userBiz.Create(token, message));
              
            });  
        }

        private void info(UserToken token)
        {

            ExecutorPool.Instance.Execute(delegate ()
            {
                write(token, UserProtocol.INFO_SRES, convert(userBiz.getByAccount(token)));

            });  
        }

        private void online(UserToken token)
        {

            ExecutorPool.Instance.Execute(delegate ()
            {
            write(token, UserProtocol.ONLINE_SRES, convert(userBiz.online(token)));

            });  
        }

        private UserDTO convert(UserModel user)
        {
            if (user == null) return null;
            return new UserDTO(user.name,user.id,user.level,user.winCount,user.loseCount,user.ranCount);
        }

        public override byte GetType()
        {
            return Protocol.TYPE_USER;
        }
    }
}