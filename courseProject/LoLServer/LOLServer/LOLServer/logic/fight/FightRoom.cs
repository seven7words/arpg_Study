using GameProtocol.constans;
using GameProtocol;
using GameProtocol.dto.fight;
using LOLServer.tool;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProtocol.dto;

namespace LOLServer.logic.fight
{
    class FightRoom:AbsMulitHandler, HandlerInterface
    {
        public Dictionary<int, AbsFightModel> teamOne = new Dictionary<int, AbsFightModel>();
        public Dictionary<int, AbsFightModel> teamTwo = new Dictionary<int, AbsFightModel>();

        private ConcurrentInteger enterCount;

        public void init(SelectModel[] teamOne, SelectModel[] teamTwo) {
            enterCount = new ConcurrentInteger(teamOne.Length+teamTwo.Length);
            //初始化英雄数据
            foreach (var item in teamOne)
            {
               this.teamOne.Add(item.userId, create(item));
            }
            foreach (var item in teamTwo)
            {
                this.teamTwo.Add(item.userId, create(item));
            }
            ///实例化队伍一的建筑
            ///预留 ID段 -1到-10为队伍1建筑
            for (int i = -1; i >= -3; i--) { 
               this.teamOne.Add(i, createBuild(i,Math.Abs(i)));
            }
            ///实例化队伍二的建筑
            ///预留 ID段 -11到-20为队伍2建筑
            for (int i = -11; i >= -13; i--)
            {
               this.teamTwo.Add(i, createBuild(i, Math.Abs(i)-10));
            }

        }

        private FightBuildModel createBuild(int id,int code) {
            BuildDataModel data = BuildData.buildMap[code];
            FightBuildModel model = new FightBuildModel(id, code, data.hp, data.hp, data.atk, data.def, data.reborn, data.rebornTime, data.initiative, data.infrared, data.name);
            model.type = ModelType.BUILD;
            return model;
        }

        private FightPlayerModel create(SelectModel model) {
            FightPlayerModel player = new FightPlayerModel();
            player.id = model.userId;
            player.code = model.hero;
            player.type = ModelType.HUMAN;
            player.name = getUser(model.userId).name;
            player.exp = 0;
            player.level = 1;
            player.free = 1;
            player.money = 0;

            //从配置表里 去出对应的英雄数据
           HeroDataModel data = HeroData.heroMap[model.hero];
           player.hp = data.hpBase;
           player.maxHp = data.hpBase;
           player.atk = data.atkBase;
           player.def = data.defBase;
           player.aSpeed = data.aSpeed;
           player.speed = data.speed;
           player.aRange = data.range;
           player.eyeRange = data.eyeRange;
           player.skills = initSkill(data.skills);
           player.equs = new int[3];
            return player;
        }

        private FightSkill[] initSkill(int[] value) {
            FightSkill[] skills=new FightSkill[value.Length];

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

        public void ClientClose(NetFrame.UserToken token, string error)
        {
            leave(token);
        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            switch (message.command) { 
                case FightProtocol.ENTER_CREQ:
                    enter(token);
                    break;

            }
        }


        private new void enter(UserToken token) {
            if (isEntered(token)) return;
            base.enter(token);
            enterCount.GetAndReduce();
            //所有人准备了 发送房间信息
            if (enterCount.get() == 0) {
                FightRoomModel room = new FightRoomModel();
                room.teamOne = teamOne.Values.ToArray();
                room.teamTwo = teamTwo.Values.ToArray();
                brocast(FightProtocol.START_BRO, room);
            }
        }

        public override byte GetType()
        {
            return Protocol.TYPE_FIGHT;
        }
    }
}
