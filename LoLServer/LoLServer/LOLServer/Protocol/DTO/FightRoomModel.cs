/* ==============================================================================
2  * 功能描述：FightRoomModel  
3  * 创 建 者：seven_words
4  * 创建日期：2018/1/2 22:57:46
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;
using GameProtocol.DTO.fight;

namespace GameProtocol.DTO
{
    /// <summary>
    /// FightRoomModel
    /// </summary>
    [Serializable]
    public class FightRoomModel
    {
        public AbsFightModel[] teamOne;
        public AbsFightModel[] teamTwo;
    }
}