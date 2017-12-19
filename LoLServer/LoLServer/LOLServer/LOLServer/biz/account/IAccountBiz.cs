/* ==============================================================================
2  * 功能描述：IUserBiz  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 23:17:11
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFrame;

namespace LOLServer.biz.account
{
    /// <summary>
    /// IUserBiz
    /// </summary>
    public interface IAccountBiz
    {
        /// <summary>
        /// 账号创建
        /// </summary>
        /// <param name="token">用户连接</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>返回创建结果 0 成功，1账号重复2账号不合法3密码不合法</returns>
        int create(UserToken token,string account,string password);
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="token">用户连接</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>登陆结果0成功，，-1账号不存在 -2 账号已经登陆 -3 密码错误  -4输入不合法</returns>
        int login(UserToken token, string account, string password);
        /// <summary>
        /// 客户端断开连接（下线）
        /// </summary>
        /// <param name="token"></param>
        void close(UserToken token);
        /// <summary>
        /// 获取账号id
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回用户登陆账号id</returns>
        int get(UserToken token);
    }
}