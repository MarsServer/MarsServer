using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class TeamInfo
    {
        public long fightId;
        public long teamId;//team boss role id
        public List<MarsPeer> peers;
        public Team team;
    }
    //isFB
    public class FightManager
    {
        public static readonly FightManager instance = new FightManager();


        private Dictionary<long, TeamInfo> infos = new Dictionary<long, TeamInfo>();

        public TeamInfo CreatTeam(MarsPeer peer)
        {
            TeamInfo fbInfo = new TeamInfo();
            fbInfo.fightId = 0;
            fbInfo.teamId = peer.role.roleId;
            fbInfo.peers = new List<MarsPeer>();
            fbInfo.peers.Add(peer);
            try
            {
                fbInfo.team = new Team();
                fbInfo.team.teamId = fbInfo.teamId;
                fbInfo.team.roles = new List<Role>();
                fbInfo.team.roles.Add(peer.role);
                

                infos.Add(fbInfo.teamId, fbInfo);
                peer.teamInfo = fbInfo;
                return fbInfo;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            return null;
        }

        public TeamInfo AddTeamMember(long roleId, MarsPeer peer)
        {
            TeamInfo fbInfo;
            if (infos.TryGetValue (roleId, out fbInfo) == true)
            {
                if (fbInfo.peers.Count <= FightConstants.TEAM_MAX_NUM)
                {
                    fbInfo.peers.Add(peer);
                    fbInfo.team.roles.Add(peer.role);
                    return fbInfo;
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
        public void DismissTeam(Role role)
        {
            TeamInfo fbInfo;
            if (infos.TryGetValue(role.roleId, out fbInfo) == true)
            {
                infos.Remove(role.roleId);
            }
        }
    }
}
