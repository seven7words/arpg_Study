/* ==============================================================================
2  * 功能描述：UserBiz  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:57:00
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.dao.model;
using NetFrame;

namespace LOLServer.biz.user.impl
{
    /// <summary>
    /// UserBiz
    /// </summary>
    public class UserBiz:IUserBiz
    {
        public bool Create(UserToken token, string name)
        {
           //账号是否登陆 获取账号id
           //判断当前账号是否已经拥有角色

            return true;
        }

        public UserModel get(UserToken token)
        {
            throw new NotImplementedException();
        }

        public UserModel get(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel online(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void offline(UserToken token)
        {
            throw new NotImplementedException();
        }

        public UserToken getToken(int id)
        {
            throw new NotImplementedException();
        }
    }
}