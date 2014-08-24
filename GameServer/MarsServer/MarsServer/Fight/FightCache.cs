using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    public void Update(GameMonster gm)
    {
 
    }
}
