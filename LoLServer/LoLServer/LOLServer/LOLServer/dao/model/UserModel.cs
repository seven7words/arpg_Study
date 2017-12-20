/* ==============================================================================
2  * 功能描述：UserModel  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 23:51:07
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.dao.model
{
    /// <summary>
    /// UserModel
    /// </summary>
    public class UserModel
    {
        public int id;//玩家id 唯一主键
        public string name;//玩家昵称
        public int level;//玩家等级
        public int exp;//玩家经验
        public int winCount;//胜利场次
        public int loseCount;//失败场次
        public int ranCount;//逃跑场次
        public int accountId;//角色所属的账号id
        public UserModel()
        {
            level = 0;
            exp = 0;
            winCount = 0;
            loseCount = 0;
            ranCount = 0;

        }
    }
}