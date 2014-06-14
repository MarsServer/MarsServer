using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZServer
{
    public class PlayersCollection
    {
        private static PlayersCollection _instance;
        public static PlayersCollection instance { get { if (_instance == null) { _instance = new PlayersCollection(); } return _instance; } }

        private Dictionary<long, Player> players = new Dictionary<long, Player>();

        public void Add(Player p)
        {
            if (players.ContainsKey(p.uniqueId) == false)
            {
                players.Add(p.uniqueId, p);
            }
        }

        public void Remove(long uniqueId)
        {
            players.Remove(uniqueId);
        }

        public void ModifyState(long uniqueID,float x, float z, float xRo, float zRo)
        {
            Player p = players[uniqueID];
            if (p != null)
            {
                p.x = x;
                p.z = z;
                p.xRo = xRo;
                p.zRo = zRo;
                players[uniqueID] = p;
            }
        }

        public Player getPlayerById (long uniqueID)
        {
            return players[uniqueID];
        }
    }
}
