/* ==============================================================================
2  * 功能描述：FightRoom  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/28 21:02:45
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using GameProtocol.Constant;
using GameProtocol.DTO;
using GameProtocol.DTO.fight;
using NetFrame;

namespace LOLServer.logic.fight
{
    /// <summary>
    /// FightRoom
    /// </summary>
    public class FightRoom:AbsMultiHandler,IHandleInterface
    {
        public Dictionary<int,AbsFightModel> teamOne = new Dictionary<int, AbsFightModel>();
        public Dictionary<int,AbsFightModel> teamTwo = new Dictionary<int, AbsFightModel>();

        public void ClientClose(UserToken token, string error)
        {
            throw new NotImplementedException();
        }

        public void ClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            throw new NotImplementedException();
        }

        public void Init(SelectModel[] teamOne, SelectModel[] teamTwo)
        {
            //初始化英雄数据
            foreach (var item  in teamOne)
            {
              this.teamOne.Add(item.userId, Create(item));
            }
            foreach (var item in teamTwo)
            {
                this.teamTwo.Add(item.userId, Create(item));
            }
            //实例化队伍一的建筑
            //预留id段-1到-10为队伍一建筑
            for (int i = -1; i >=-3 ; --i)
            {
                
                this.teamOne.Add(i, CreateBuild(i, Math.Abs(i)));
            }
            //实例化队伍二的建筑
            //预留id段-11到-20为队伍二建筑
            for (int i = -11; i >= -13; --i)
            {
                
                this.teamTwo.Add(i, CreateBuild(i, Math.Abs(i + 10)));
            }


        }

        private FightBuildModel CreateBuild(int id,int code)
        {
            BuildDataModel data = BuildData.buildMap[code];
            FightBuildModel model = new FightBuildModel(id, code, data.hp, data.hp, data.atk, data.def, data.reborn, data.rebornTime, data.initiative, data.infrared, data.name);
            model.type = ModelType.BUILD;
            //model.team = team;
            return model;
        }

        private FightPlayerModel Create(SelectModel model)
        {
            FightPlayerModel player = new FightPlayerModel();
            player.code = model.hero;
            player.name = getUser(model.userId).name;
            player.exp = 0;
            player.level = 1;
            player.free = 1;
            player.money = 0;
            //从配置表里取出对应的英雄数据
            HeroDataModel data = HeroData.heroMap[model.hero];
            player.hp = data.hpBase;
            player.maxHp = data.hpBase;
            player.atk = data.atkBase;
            player.def = data.defBase;
            player.atkSpeed = data.atkSpeed;
            player.speed = data.speed;
            player.atkRange = data.atkRange;
            player.eyeRange = data.eyeRange;
            player.skills = InitSkill(data.skills);
            player.equs = new int[3];

            return player;
        }

        private FightSkill[] InitSkill(int[] value)
        {
            FightSkill[] skills = new FightSkill[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                int skillCode = value[i];
                SkillDataModel data = SkillData.skillMap[skillCode];
                SkillLevelData levelData = data.levels[0];
                FightSkill skill = new FightSkill(skillCode, 0, levelData.level, levelData.time, data.name, levelData.range, data.info, data.target, data.type);
                skills[i] = skill;
            }
            return skills;
        }
    }
}