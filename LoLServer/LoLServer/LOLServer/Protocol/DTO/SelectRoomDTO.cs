/* ==============================================================================
2  * 功能描述：SelectRoomDTO  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/21 23:46:47
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO
{
    /// <summary>
    /// SelectRoomDTO
    /// </summary>
    [Serializable]
    public class SelectRoomDTO
    {
        public SelectModel[] teamOne;
        public SelectModel[] teamTwo;
    }
}