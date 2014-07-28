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

        public TeamInfo GetTeamInfo(long teamId)
        {
            TeamInfo teamInfo = null;
            if (infos.TryGetValue(teamId, out teamInfo))
            {
                Debug.Log("Cann't find " + teamId);
            }
            return teamInfo;
        }

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
                peer.teamId = fbInfo.teamId;
                return fbInfo;
            }
            catch (System.Exception e)
            {
                //Debug.Log(e);
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
                    Role role = new Role();
                    role.accountId = peer.role.accountId;
                    role.roleId = peer.role.roleId;
                    role.roleName = peer.role.roleName;
                    role.profession = peer.role.profession;
                    role.level = peer.role.level;
                    fbInfo.team.roles.Add(role);
                    return fbInfo;
                }
            }
            return null;
        }

        public void ModifyTeamInfo(TeamInfo info)
        {
            if (info != null)
            {
                infos[info.teamId] = info;
            }
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
                        fbInfo.team.roles.Remove(peer.role);
                        fbInfo.peers.Remove(peer);
                        Debug.Log(fbInfo.team.roles.Count + "_____" + fbInfo.peers);
                        return peer.role;
                    }
                }
            }
            return null;
        }

        public TeamInfo SwapTeamLeader(Role role, MarsPeer peer)
        {
            TeamInfo teaminfo = null;
            if (infos.TryGetValue (peer.role.roleId, out teaminfo))
            {
                infos.Remove(teaminfo.teamId);
                teaminfo.teamId = role.roleId;
                teaminfo.team.teamId = role.roleId;
                infos.Add(teaminfo.teamId, teaminfo);

                foreach (MarsPeer p in teaminfo.peers)
                {
                    p.teamId = teaminfo.teamId;
                }
            }
            return teaminfo;
        }

        public List<MarsPeer> DismissTeam(Role role)
        {
            List<MarsPeer> peers = null;
            TeamInfo fbInfo;
            if (role != null)
            {
                if (infos.TryGetValue(role.roleId, out fbInfo) == true)
                {
                    peers = fbInfo.peers;
                    infos.Remove(role.roleId);
                }
            }
            return peers;
        }

        public List<MarsPeer> GetListBesideMe (long teamId, Role role)
        {
            TeamInfo teamInfo = GetTeamInfo(teamId);
            List<MarsPeer> peers = new List<MarsPeer> ();
            if (teamInfo == null)
            {
                return null;
            }
            foreach (MarsPeer peer in teamInfo.peers)
            {
                if (role.roleId == peer.role.roleId)
                {
                    continue;
                }
                peers.Add(peer);
            }
            return peers;
        }
    }
}
