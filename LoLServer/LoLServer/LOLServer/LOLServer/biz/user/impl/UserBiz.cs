/* ==============================================================================
2  * 功能描述：UserBiz  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:57:00
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.biz.account;
using LOLServer.cache;
using LOLServer.dao.model;
using NetFrame;

namespace LOLServer.biz.user.impl
{
    /// <summary>
    /// 用户事物处理
    /// </summary>
    public class UserBiz:IUserBiz
    {
        private IAccountBiz accBiz = BizFactory.accountBiz;
        private IUserCache userCache = CacheFactory.userCache;
        public bool Create(UserToken token, string name)
        {
           //账号是否登陆 获取账号id
            int accountId =   accBiz.get(token);
            if (accountId == -1)
                return false;
           //判断当前账号是否已经拥有角色
            if (userCache.hasByAccountId(accountId))
                return false;
            userCache.create(token, name);
            return true;
        }

        public UserModel get(UserToken token)
        {
            return userCache.get(token);
        }

        public UserModel get(int id)
        {
            return userCache.get(id);
        }

        public UserModel online(UserToken token)
        {
            //账号是否登陆 获取账号id
            int accountId = accBiz.get(token);
            if (accountId == -1)
                return null;
            UserModel user = userCache.getByAccountId(accountId);
            if (userCache.isOnline(user.id))
                return null;
            userCache.online(token, user.id);
            return user;
        }

        public void offline(UserToken token)
        {
            userCache.offline(token);
        }

        public UserToken getToken(int id)
        {
            return userCache.getToken(id);
        }
    }
}