using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class ActorCollection : List<MarsPeer>
    {
        public bool TryGetValue(long accountId, out MarsPeer peer)
        {
            peer = this.FirstOrDefault(marsPeer => marsPeer.accountId == accountId);
            return peer != null;
        }

        /// <summary>
        /// find peer list by specified peer
        /// region same, and  not self
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public List<MarsPeer> GetPeersBySamePos(MarsPeer peer)
        {
            IEnumerable<MarsPeer> myPeersList = this.Where(marsPeer => (marsPeer.region == peer.region && marsPeer.accountId != peer.accountId));
            List<MarsPeer> peers = new List<MarsPeer>();
            foreach (MarsPeer for_peer in myPeersList)
            {
                peers.Add(for_peer);
            }
            return peers;
        }

        /// <summary>
        /// find peer list by specified peer
        /// region is 0(is in select UI), and not self
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public List<MarsPeer> GetPeersByOthers(MarsPeer peer)
        {
            IEnumerable<MarsPeer> myPeersList = this.Where(marsPeer => (marsPeer.region != Constants.SelectRole && marsPeer.accountId != peer.accountId));

            List<MarsPeer> peers = new List<MarsPeer>();
            foreach (MarsPeer for_peer in myPeersList)
            {
                peers.Add(for_peer);
            }
            return peers;
        }

        /// <summary>
        /// find role list by specified peer
        /// region same, and  not self
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public List<Role> GetRolesBySamePos(MarsPeer peer)
        {
            IEnumerable<MarsPeer> myPeersList = this.Where(marsPeer => (marsPeer.region == peer.region && marsPeer.accountId != peer.accountId));
            List<Role> roles = new List<Role>();
            foreach (MarsPeer for_peer in myPeersList)
            {
                Role role = for_peer.Role;
                if (role == null) continue;
                role.x = for_peer.x;
                role.z = for_peer.z;
                role.xRo = for_peer.xRo;
                role.zRo = for_peer.zRo;
                roles.Add(role);
            }
            return roles;
        }
    }
}
