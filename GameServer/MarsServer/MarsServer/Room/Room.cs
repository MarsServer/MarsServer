using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    /// <summary>
    /// A base class
    /// to do manager room
    /// </summary>
    public class Room
    {
        public enum BroadcastType
        {
            Chat = 0,
            Region = 1,
        }


        private RoomCollection Rooms = new RoomCollection();

        protected object SyncRoot = new object();

        public Team GetTeamById(string id, MarsPeer peer)//get key
        {
            return GetTeamById(id, null, peer);
        }
        /// <summary>
        /// !!!!!!!!!
        /// id is team's id
        /// name is team's. if not null, creat, else get a specified team
        /// MarsPeer is one peer;
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="peer"></param>
        /// <returns></returns>
        public virtual Team GetTeamById(string id, string name, MarsPeer peer)//creat new......
        {
            Team team = null;
            lock (this.SyncRoot)
            {
                if (Rooms.TryGetValue(id, out team) == false)
                {
                    if (name != null)
                    {
                        team = new Team();
                        team.teamId = Guid.NewGuid().ToString();
                        team.teamName = name;
                        team.peers = new List<MarsPeer>();
                    }
                }
                if (team.peers.Contains(peer) == false)
                {
                    team.peers.Add(peer);
                }
            }
            return team;
        }

        /// <summary>
        /// Remove one team
        /// </summary>
        /// <param name="teamId"></param>
        public void RemoveTeam(string teamId)
        {
            Rooms.Remove(teamId);
        }

        /// <summary>
        /// LeaveTeamRole;Return a role
        /// </summary>
        /// <param name="peer"></param>
        public Role LeaveTeamRole(MarsPeer peer)
        {
            Role role = null;
            lock (this.SyncRoot)
            {
                Team team = null;
                if (Rooms.TryGetValue(peer.team.teamId, out team) == true)
                {
                    role = peer.role;
                    team.peers.Remove(peer);
                }
            }
            return role;
        }

        /// <summary>
        /// Get all team beside public zone;
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Team> getAllTeams()
        {
            return Rooms;
        }

        /// <summary>
        /// Broast all peers in team, beside self
        /// e.g BroadcastType is Chat
        /// e.g BroadcastType is Region,that will send same region action
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="bundle"></param>
        public void BroadcastEvent(MarsPeer peer, Bundle bundle, BroadcastType broadcastType = BroadcastType.Chat)
        {
            Team team = null;
            List<MarsPeer> peerList = new List<MarsPeer>();
            if (Rooms.TryGetValue(peer.team.teamId, out team) == true)
            {
                foreach (MarsPeer for_p in team.peers)
                {
                    if (for_p != peer)
                    {
                        if (broadcastType == BroadcastType.Region)
                        {
                            if (peer.region != for_p.region)
                            {
                                continue;
                            }
                        }
                        peerList.Add(for_p);
                    }
                }
            }
            if (peerList.Count > 0)
            {
                MarsPeer.BroadCastEvent(peerList, bundle);
            }
        }
    }
}
