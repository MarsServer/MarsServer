using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsServer;

public class FightInstance
{
    public readonly static FightInstance instance = new FightInstance();

    private static int MinFightCacheId = 100;
    private FightCollection Fights = new FightCollection();

    /// <summary>
    /// Get a fightcache, if exist, get,  else new......
    /// fightcache's id is teamid...
    /// </summary>
    /// <param name="id">is team id</param>
    /// <param name="gameMonsters"></param>
    /// <returns></returns>
    public FightCache GetFightCache(string id, Dictionary<string, GameMonster> gameMonsters)
    {
        FightCache fightCache = null;
        if (Fights.TryGetValue(id, out fightCache) == false)
        {
            fightCache = new FightCache(id, gameMonsters);
            Fights.Add(fightCache);
        }
        else if (gameMonsters != null)
        {
            fightCache.AddRandge(gameMonsters);
        }
        return fightCache;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">is fightCache's id</param>
    /// <param name="msId">is gameMonster,s id</param>
    public void RemoveMS(string id, string msId)
    {
        FightCache fightCache = GetFightCache(id, null);
        if (fightCache != null)
        {
            fightCache.Remove(msId);
        }
    }

    /// <summary>
    /// Remove this fightCahe
    /// </summary>
    /// <param name="fightCache"></param>
    public void Remove(FightCache fightCache)
    {
        Debug.Log("**********" + Fights.Count);
        Fights.Remove(fightCache);
        Debug.Log("**********" + Fights.Count);
    }

    public void SaveMonster()
    {
       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gm"></param>
    public void UpdateMonsters(string id, Dictionary<string, GameMonster> gameMonsters)
    {
        FightCache fightCache = GetFightCache(id, null);
        foreach (GameMonster gm in gameMonsters.Values)
        {
            if (fightCache != null)
            {
                GameMonster g_gm = fightCache.gameMonsters[gm.id];
                g_gm.x = gm.x;
                g_gm.z = gm.z;
                g_gm.xRo = gm.xRo;
                g_gm.zRo = gm.zRo;
                g_gm.action = gm.action;
                g_gm.state = gm.state;
                if (g_gm.state == 1)
                {
                    g_gm.target_x = gm.target_x;
                    g_gm.target_y = gm.target_y;
                    g_gm.target_z = gm.target_z;
                }
            }
        }
    }
}
