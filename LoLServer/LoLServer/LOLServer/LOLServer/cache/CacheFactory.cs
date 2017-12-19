/* ==============================================================================
2  * 功能描述：CacheFactory  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 18:48:01
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOLServer.cache.impl;

namespace LOLServer.cache
{
    /// <summary>
    /// CacheFactory
    /// </summary>
    public class CacheFactory
    {
        public readonly static IAccountCache accountCache; 
        static CacheFactory()
        {
            accountCache = new AccountCache();
        }

    }
}