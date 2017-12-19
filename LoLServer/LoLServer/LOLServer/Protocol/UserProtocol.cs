/* ==============================================================================
2  * 功能描述：UserProtocol  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:41:28
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    /// <summary>
    /// UserProtocol
    /// </summary>
    public class UserProtocol
    {
        public const int INFO_CREQ = 0;//获取自身数据
        public const int INFO_SRES = 1;//返回自身数据
        public const int CREATE_CREQ = 2;//申请创建角色
        public const int CREATE_SRES = 3;//返回创捷结果

        public const int ONLINE_CREQ = 4;//用户上仙
        public const int ONLINE_SRES = 5;//返回用户上线
    }
}