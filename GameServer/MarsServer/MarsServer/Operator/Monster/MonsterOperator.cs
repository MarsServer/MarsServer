using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class MonsterOperator : Game
    {
        public MonsterOperator(MarsPeer peer)
            : base(peer)
        { 
        }

        public override void ExecuteOperation(Command cmd, params object[] objs)
        {
            base.ExecuteOperation(cmd, objs);

            switch (cmd)
            {
                case Command.MonsterRefresh:
                    FightRegion fr = (FightRegion)objs[0];
                    HandleMonsterRefresh(fr);
                    marsPeer.SendClientResponse(bundle);
                    break;
                case Command.MonsterStateUpdate:
                    GameMonster gm = (GameMonster)objs[0];
                    HandleMonsterStateUpdate(gm);
                    marsPeer.SendClientResponse(bundle);
                    break;
            }
        }

        void HandleMonsterRefresh (FightRegion fr)
        {
            //send fight region
            bundle.region = fr;

            LvInfo lvInfo = LvInfoSQL.instance.GetValueByK(fr.scId);
            if (lvInfo != null)
            {
                //Fight g_Fight = JsonConvert.DeserializeObject<Fight>(lvInfo.scInfoJson);
                bundle.gameMonsters = lvInfo.fight.gameMonsters[fr.index];
            }

            Dictionary<string, GameMonster> gmDict = new Dictionary<string, GameMonster>();
            foreach (GameMonster gm in bundle.gameMonsters)
            {
                gmDict.Add(gm.id, gm);
            }
            FightCache cache = FightInstance.instance.GetFightCache(this.marsPeer.Role.roleId.ToString(), gmDict);
            this.marsPeer.fightCache = cache;
        }

        void HandleMonsterStateUpdate(GameMonster gm)
        {
            GameMonster gameMonster = new GameMonster();
            gameMonster.id = gm.id;
            GameMonster g_gm = marsPeer.fightCache.UpdateHp(gm);
            if (g_gm != null)
            {
                gameMonster.hp = g_gm.hp;
                if (g_gm.gameReward != null)
                {
                    gameMonster.gameReward = g_gm.gameReward;
                }
            }
            bundle.gameMonster = gameMonster;

            if (marsPeer.team != null)
            {
                //bundle.cmd = cmd;
                RoomInstance.instance.BroadcastEvent(marsPeer, bundle, Room.BroadcastType.Region);
            }
        }
    }
}
