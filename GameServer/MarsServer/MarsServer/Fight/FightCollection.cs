using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class FightCollection : List<FightCache>
{
    public bool TryGetValue(string id, out FightCache fightCache)
    {
        fightCache = this.FirstOrDefault(fc => fc.id == id);
        return fightCache != null;
    }
}
