using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    struct FBInfo
    {
        public long fightId;
        public long teamId;//team boss role id
        public List<MarsPeer> peers;
    }
    //isFB
    public class FightManager
    {
        public static readonly FightManager instance = new FightManager();


        private Dictionary<long, FBInfo> infos = new Dictionary<long, FBInfo>();
        public void EnterFb(Fight fight, MarsPeer peer)
        {
            FBInfo fbInfo = new FBInfo();
            fbInfo.fightId = fight.id;
            fbInfo.teamId = peer.role.roleId;
            fbInfo.peers = new List<MarsPeer>();
            fbInfo.peers.Add(peer);
            infos.Add(fbInfo.teamId, fbInfo);
        }

        public void AddTeamMember(long roleId, MarsPeer peer)
        {
            FBInfo fbInfo;
            if (infos.TryGetValue (roleId, out fbInfo) == true)
            {
                if (fbInfo.peers.Count <= FightConstants.TEAM_MAX_NUM)
                {
                    fbInfo.peers.Add(peer);
                }
            }
        }
    }
}
