/* ==============================================================================
2  * 功能描述：FightSkill  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/28 21:22:39
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO.fight
{
    /// <summary>
    /// FightSkill
    /// </summary>
    [Serializable]
    public class FightSkill
    {
        public int code;//策划编码
        public int level;//等级
        public int nextLevel;//下一次学习的玩家等级
        public int time;//冷却时间 ms
        public string name;//技能名称
        public float range;//释放距离
        public string info;//技能描述
        public SkillTarget target;//技能伤害目标类型
        public SkillType type;//技能释放类型

        public FightSkill() { }
        public FightSkill(
            int code,
            int level,
            int nextLevel,
            int time,
            string name,
            float range,
            string info,
            SkillTarget target,
            SkillType type
        )
        {
            this.code = code;
            this.level = level;
            this.nextLevel = nextLevel;
            this.time = time;
            this.name = name;
            this.range = range;
            this.info = info;
            this.target = target;
            this.type = type;
        }
    }
    /// <summary>
    /// 能够造成效果的单位类型
    /// </summary>
    public enum SkillTarget
    {
        SELF,//自身释放
        F_H,//友方英雄
        F_N_B,//友方非建筑单位
        F_ALL,//友方全体
        E_H,//敌方英雄
        E_N_B,//敌方非建筑
        E_S_N,//敌方小兵和中立单位
        N_F_ALL,//非友方单位

    }
    /// <summary>
    /// 技能释放方式
    /// </summary>
    public enum SkillType
    {
        SELF,//以自身为中心释放
        TARGET,//以目标为中心进行释放
        POSITION,//以鼠标点击位置进行释放
    }
    /// <summary>
    /// 战斗模型类型
    /// </summary>
    public enum ModelType
    {
        BUILD,//建筑
        HUMAN,//生物
    }
}