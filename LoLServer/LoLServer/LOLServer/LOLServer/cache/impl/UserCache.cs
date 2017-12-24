/* ==============================================================================
2  * 功能描述：UserCache  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 9:25:49
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
    /// UserCache
    /// </summary>
    public class UserCache:IUserCache
    {
        /// <summary>
        /// 账号id和模型的映射表
        /// </summary>
        Dictionary<int,UserModel> idToModel = new Dictionary<int, UserModel>();
        /// <summary>
        /// 账号id和角色id之间的绑定
        /// </summary>
        Dictionary<int,int> accToUid = new Dictionary<int, int>();
        /// <summary>
        /// 用户id和token连接的映射表
        /// </summary>
        Dictionary<int,UserToken> idToToken = new Dictionary<int, UserToken>();
        /// <summary>
        /// 用户连接和用户id的映射表
        /// </summary>
        Dictionary<UserToken,int> tokenToId = new Dictionary<UserToken, int>();
        /// <summary>
        /// 角色id
        /// </summary>
        private int index = 0;
        public bool create(UserToken token, string name,int accountId)
        {
            UserModel user = new UserModel();
            user.name = name;
            user.id = index++;
            user.accountId = accountId;
            List<int> list = new List<int>();
            for (int i = 1; i < 8; i++)
            {
                list.Add(i);
            }
            user.heroList = list;
            //创建成功，进行账号id和用户id的绑定
            accToUid.Add(accountId,user.id);
            //创建成功进行用户id和用户模型的绑定

            idToModel.Add(user.id,user);
            return true;
        }

        public bool has(UserToken token)
        {
            return tokenToId.ContainsKey(token);
        }

        public bool hasByAccountId(int accountId)
        {
            return accToUid.ContainsKey(accountId);
        }

        public UserModel get(UserToken token)
        {
            if (!has(token)) return null;
            return idToModel[tokenToId[token]];
        }

        public UserModel get(int id)
        {
            return idToModel[id];
        }

        public UserModel online(UserToken token,int id)
        {
            idToToken.Add(id,token);
            tokenToId.Add(token,id);
            return idToModel[id];
        }

        public void offline(UserToken token)
        {
            if (tokenToId.ContainsKey(token))
            {
                if (idToToken.ContainsKey(tokenToId[token]))
                {
                    idToToken.Remove(tokenToId[token]);
                }
                tokenToId.Remove(token);
            }
        }

        public UserToken getToken(int id)
        {
            return idToToken[id];
        }

        public UserModel getByAccountId(int accountId)
        {
            if (!accToUid.ContainsKey(accountId)) return null;
            return idToModel[accToUid[accountId]];
        }

        public bool isOnline(int id)
        {
            return idToToken.ContainsKey(id);
        }
    }
}