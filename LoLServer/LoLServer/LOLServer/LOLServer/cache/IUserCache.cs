/* ==============================================================================
2  * 功能描述：IUserCache  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:58:08
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFrame;

namespace LOLServer.cache
{
    /// <summary>
    /// IUserCache
    /// </summary>
    public interface IUserCache
    {
        bool create(UserToken token,string name);
        bool has(UserToken token);

    }
}