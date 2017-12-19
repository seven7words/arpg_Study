/* ==============================================================================
2  * 功能描述：AccountInfoDTO  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 20:39:09
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO
{
    /// <summary>
    /// AccountInfoDTO
    /// </summary>
    [Serializable]
    public class AccountInfoDTO
    {
        public string account;
        public  string password;
    }
}