/* ==============================================================================
2  * 功能描述：FightPlayerModel  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/28 21:18:00
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO.fight
{
    /// <summary>
    /// FightPlayerModel
    /// </summary>
    public class FightPlayerModel:AbsFightModel
    {
        public int level;//等级
        public int exp;//经验
        public int free;//剩余潜能点
        public int money;//玩家经济
        public int[] equs;//装备表
        public int mp;//当前能量
        public int maxMp;//最大能量         
        public FightSkill[] skills;//英雄拥有技能（玩家）
    }
}