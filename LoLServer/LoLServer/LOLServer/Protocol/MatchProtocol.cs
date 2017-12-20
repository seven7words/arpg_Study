/* ==============================================================================
2  * 功能描述：MatchProtocol  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 21:51:09
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    /// <summary>
    /// MatchProtocol
    /// </summary>
    public class MatchProtocol
    {
        public const int ENTER_CREQ = 0;//申请进入匹配
        public const int ENTER_SRES = 1;//是返回申请结果
        public const int LEAVE_CREQ = 2;//申请离开匹配
        public const int LEAVE_SRES = 3;//返回离开结果 


    }
}