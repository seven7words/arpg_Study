/* ==============================================================================
2  * 功能描述：UserBiz  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 23:23:45
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.cache;
using NetFrame;

namespace LOLServer.biz.account.impl
{
    /// <summary>
    /// UserBiz
    /// </summary>
    public class AccountBiz:IAccountBiz
    {
        private IAccountCache accountCache = CacheFactory.accountCache;
        public int create(UserToken token, string account, string password)
        {
            if (accountCache.hasAccount(account))
                return 1;
            accountCache.add(account,password);
            return 0;
        }

        public int login(UserToken token, string account, string password)
        {
            //账号密码为空，输入不合法
            if (account == null || password == null)
                return -4;
            //账号是否存在
            if (!accountCache.hasAccount(account))
                return -1;
            //账号是否在线
            if (accountCache.isOnline(account))
                return -2;
            //账号密码是否匹配
            if (!accountCache.match(account, password))
                return -3;
            //验证都通过，可以登陆，调用上仙并返回成功
            accountCache.online(token,account);
            return 0;
        }

        public void close(UserToken token)
        {
            accountCache.offline(token);
        }

        public int get(UserToken token)
        {
            return accountCache.getId(token);
        }
    }
}