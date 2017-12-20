/* ==============================================================================
2  * 功能描述：UserDTO  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 18:47:52
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO
{
    /// <summary>
    /// UserDTO
    /// </summary>
    [Serializable]
    public class UserDTO
    {
        public int id;//玩家id 唯一主键
        public string name;//玩家昵称
        public int level;//玩家等级
        public int exp;//玩家经验
        public int winCount;//胜利场次
        public int loseCount;//失败场次
        public int ranCount;//逃跑场次

        public UserDTO()
        {
            
        }

        public UserDTO(string name,int id,int level,int win,int lose,int ran)
        {
            this.id = id;
            this.name = name;
            this.winCount = win;
            this.loseCount = lose;
            this.ranCount = ran;
        }
    }
}