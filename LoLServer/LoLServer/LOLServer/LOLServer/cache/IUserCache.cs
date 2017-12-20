/* ==============================================================================
2  * 功能描述：IUserCache  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:58:08
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.dao.model;
using NetFrame;

namespace LOLServer.cache
{
    /// <summary>
    /// IUserCache
    /// </summary>
    public interface IUserCache
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool create(UserToken token,string name);
        /// <summary>
        /// 是否拥有角色
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool has(UserToken token);
        /// <summary>
        /// 判断对应账号id是否拥有角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        bool hasByAccountId(int accountId);
        /// <summary>
        /// 根据连接获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel get(UserToken token);
        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel get(int id);
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        UserModel online(UserToken token,int id);
        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="token"></param>
        void offline(UserToken token);
        /// <summary>
        /// 
        /// 通过id获取连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken getToken(int id);
        /// <summary>
        /// 通过张宏ID获取角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        UserModel getByAccountId(int accountId);
        /// <summary>
        /// 角色是否已经在在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool isOnline(int id);
    }
}