/* ==============================================================================
2  * 功能描述：UserBiz  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 23:23:45
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFrame;

namespace LOLServer.biz.account.impl
{
    /// <summary>
    /// UserBiz
    /// </summary>
    public class AccountBiz:IAccountBiz
    {
        public int create(UserToken token, string account, string password)
        {
            return 0;
        }

        public int login(UserToken token, string account, string password)
        {
            return 0;
        }

        public void close(UserToken token)
        {
            
        }

        public int get(UserToken token)
        {
            return 0;
        }
    }
}