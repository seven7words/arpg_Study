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

namespace LOLServer.biz
{
    /// <summary>
    /// BizFactory
    /// </summary>
    public class BizFactory
    {
        public readonly static IAccountBiz accountBiz;

        static BizFactory()
        {
            accountBiz = new AccountBiz();
        }
    }
}