/* ==============================================================================
2  * 功能描述：AccountCache  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 18:52:36
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.dao.model;
using NetFrame;

namespace LOLServer.cache.impl
{
    /// <summary>
    /// AccountCache
    /// </summary>
    public class AccountCache : IAccountCache
    {
        public int index = 0;
        /// <summary>
        /// 玩家连接对象与账号的映射绑定
        /// </summary>
        Dictionary<UserToken,string> onlineAccMap = new Dictionary<UserToken, string>();
        /// <summary>
        /// 账号与自身具体属性的映射绑定
        /// </summary>
        Dictionary<string,AccountModel> accMap = new Dictionary<string, AccountModel>();

        public void add(string account, string password)
        {
            //创建实体并绑定
            AccountModel model = new AccountModel();
            model.account = account;
            model.password = password;
            model.id = index;
            accMap.Add(account,model);
            index++;
        }

        public int getId(UserToken token)
        {
            //判断在线字典中是否有此连接的记录，没有说明此连接没有登陆，无法获取账号id
            if (onlineAccMap.ContainsKey(token))
                return -1;
            //返回绑定账号id
            return accMap[onlineAccMap[token]].id;
        }
       
        public bool hasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool isOnline(string account)
        {
            //判断当前在线字典中是否有次账号
            return onlineAccMap.ContainsValue(account);
        }

        public bool match(string account, string password)
        {
            //判断账号是否存在，不存在就不关匹配
            if (!hasAccount(account))
                return false;
            //获取账号的信息，判断密码是否匹配并返回
            return accMap[account].password.Equals(password);
        }

        public void offline(UserToken token)
        {
            //如果当前连接有登陆，进行移除
            if (onlineAccMap.ContainsKey(token))
                onlineAccMap.Remove(token);
        }

        public void online(UserToken token, string account)
        {
            //添加映射
            onlineAccMap.Add(token,account);
        }
    }
}