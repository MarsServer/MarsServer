using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsServer;
public class FightCache
{
    public string id;
    public Dictionary<string, GameMonster> gameMonsters;

    public FightCache(string id,Dictionary<string, GameMonster> gameMonsters)
    {
        this.id = id;
        this.gameMonsters = gameMonsters;
    }

    public void Add(GameMonster gameMonster)
    {
        gameMonsters.Add(gameMonster.id, gameMonster);
    }
    public void AddRandge(Dictionary<string, GameMonster> gameMonsters)
    {
        foreach (KeyValuePair<string, GameMonster> gm in gameMonsters)
        {
           this.gameMonsters.Add(gm.Key, gm.Value);
        }
    }

    public void Remove(string id)//when monster boss.....
    {
        gameMonsters.Remove(id);

        /*if (gameMonsters.Count == 0)
        {
            FightInstance.instance.Remove(this);
        }*/
    }

    public GameMonster UpdateHp(GameMonster gm, MarsPeer peer)
    {
        GameMonster gameMonster = null;
        this.gameMonsters.TryGetValue(gm.id, out gameMonster);

        if (gameMonster != null)
        {
            gameMonster.hp -= gm.deductHp;
            gameMonster.hp = Math.Max(gameMonster.hp, 0);

            if (gameMonster.hp <= 0)
            {
                //TODO: when monster die give some reward...
                gameMonster.gameReward = new GameReward();
                gameMonster.gameReward.exp = 100;
                gameMonster.gameReward.item = new Item();
                gameMonster.gameReward.item.id = "100001";
                gameMonster.gameReward.item.num = 10;

                RoleMySQL.instance.AddExp(peer.roleId, gameMonster.gameReward.exp);
            }
        }
        return gameMonster;
    }

    public void UpdateAttId(GameMonster gm)
    {
        GameMonster gameMonster = null;
        this.gameMonsters.TryGetValue(gm.id, out gameMonster);

        if (gameMonster != null)
        {
            gameMonster.attId -= gm.attId;
        }
    }
}
