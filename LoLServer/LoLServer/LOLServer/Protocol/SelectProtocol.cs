/* ==============================================================================
2  * 功能描述：SelectProtocol  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/21 23:28:42
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    /// <summary>
    /// SelectProtocol
    /// </summary>
    public class SelectProtocol
    {
        public const int ENTER_CREQ = 0;
        public const int ENTER_SRES = 1;
        public const int ENTER_EXBRO = 2;
        public const int SELECT_CREQ = 3;
        public const int SELECT_SRES = 4;
        public const int SELECT_BRO = 5;
        public const int TALK_CREQ = 6;
        public const int TALK_BRO = 7;
        public const int READY_CREQ = 8;
        public const int READY_BRO = 9;
        public const int DESTROY_BRO = 10;
        public const int FIGHT_BRO = 11;
    }
}