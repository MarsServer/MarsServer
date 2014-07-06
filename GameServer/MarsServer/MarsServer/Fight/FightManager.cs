using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    struct TeamInfo
    {
        public long fightId;
        public long teamId;//team boss role id
        public List<MarsPeer> peers;
    }
    //isFB
    public class FightManager
    {
        public static readonly FightManager instance = new FightManager();


        private Dictionary<long, TeamInfo> infos = new Dictionary<long, TeamInfo>();
        public string CreatTeam(MarsPeer peer)
        {
            TeamInfo fbInfo = new TeamInfo();
            fbInfo.fightId = 0;
            fbInfo.teamId = peer.role.roleId;
            fbInfo.peers = new List<MarsPeer>();
            fbInfo.peers.Add(peer);
            try
            {
                infos.Add(fbInfo.teamId, fbInfo);
                return NetSuccess.CREAT_TEAM_SUCCESS;
            }
            catch (System.Exception e)
            {
            }
            return NetError.CREAT_TEAM_FIALURE;
        }

        public Role AddTeamMember(long roleId, MarsPeer peer)
        {
            TeamInfo fbInfo;
            if (infos.TryGetValue (roleId, out fbInfo) == true)
            {
                if (fbInfo.peers.Count <= FightConstants.TEAM_MAX_NUM)
                {
                    fbInfo.peers.Add(peer);
                    return peer.role;
                }
            }
            return null;
        }

        public Role RemoveTeamMember(long roleId, MarsPeer peer)
        {
            TeamInfo fbInfo;
            if (infos.TryGetValue(roleId, out fbInfo) == true)
            {
                for (int i = 0; i < fbInfo.peers.Count; i++)
                {
                    if (fbInfo.peers[i].role.roleId == peer.role.roleId)
                    {
                        return peer.role;
                    }
                }
            }
            return null;
        }
    }
}
