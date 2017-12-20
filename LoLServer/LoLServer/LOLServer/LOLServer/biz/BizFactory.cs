/* ==============================================================================
2  * 功能描述：BizFactory  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 23:26:06
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.biz.account;
using LOLServer.biz.account.impl;
using LOLServer.biz.user;
using LOLServer.biz.user.impl;

namespace LOLServer.biz
{
    /// <summary>
    /// BizFactory
    /// </summary>
    public class BizFactory
    {
        public readonly static IAccountBiz accountBiz;
        public readonly static IUserBiz userBiz;
        static BizFactory()
        {
            accountBiz = new AccountBiz();
            userBiz = new UserBiz();
        }
    }
}