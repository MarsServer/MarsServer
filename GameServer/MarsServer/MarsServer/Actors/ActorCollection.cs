using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class ActorCollection
    {
        public readonly static ActorCollection Instance = new ActorCollection();


        private Dictionary<long, MarsPeer> allUserByAccountId = new Dictionary<long, MarsPeer>();//key is accountId;
        private List<long> accountIds = new List<long>();//all accountIds;

        public delegate bool HandlePeerListLimit(MarsPeer peer);
        public delegate bool HandleRoleListLimit(Role peer);

        /// <summary>
        /// Size for debug
        /// </summary>
        public string Size
        {
            get 
            {
                return allUserByAccountId.Count + "/" + accountIds.Count;
            }
        }
        /*private Dictionary<long, MarsPeer> allUserByRoleId = new Dictionary<long, MarsPeer>();//key is roleId;
        private Dictionary<string, MarsPeer> allUserByRoleName = new Dictionary<string, MarsPeer>();//key is roleName*/

        
        /// <summary>
        /// add a peer by accountid
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="peer"></param>
        /// <returns></returns>
        public bool HandleAccountLogin(long accountId, MarsPeer peer)
        {
            MarsPeer getPeer = null;
            if (allUserByAccountId.TryGetValue(accountId, out getPeer) == false)
            {
                allUserByAccountId.Add(accountId, peer);
                accountIds.Add(accountId);
                return true;
            }
            else
            {
                Debug.Log("getPeer.Connected = " + getPeer.Connected);

                getPeer.Dispose();
                return false;
            }
        }

        /// <summary>
        /// Remove a peer by accountid
        /// </summary>
        /// <param name="accountId"></param>
        public void HandleDisconnect(long accountId)
        {
            allUserByAccountId.Remove(accountId);
            accountIds.Remove(accountId);
        }

        /// <summary>
        /// return is BOOL by handlePeerListLimit
        /// BOOL is true, not add List, e.g it is self, or diff region
        /// else is add the list
        /// </summary>
        /// <param name="handlePeerListLimit"></param>
        /// <returns></returns>
        public List<MarsPeer> HandleAccountListOnline(HandlePeerListLimit handlePeerListLimit)
        {
            bool isAllAdd = (handlePeerListLimit == null);
            List<MarsPeer> peers = new List<MarsPeer>();
            for (int i = 0; i < accountIds.Count; i++ )
            {
                MarsPeer peer = allUserByAccountId[accountIds[i]];
                if (peer == null) continue;
                if (isAllAdd == false)
                {
                    if (handlePeerListLimit(peer) == true)
                    {
                        continue;
                    }
                }
                peers.Add(peer);
            }
            return peers;
        }

        /// <summary>
        /// Get beside me all peers
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<MarsPeer> HandleAccountListOnlineByOthers(MarsPeer p)
        {
            List<MarsPeer> peers = ActorCollection.Instance.HandleAccountListOnline((MarsPeer peer) =>
            {
                return peer.accountId == p.accountId || peer.region == 0;////account is self, or region not be zero, don't add list Peer
            });
            return peers;
        }

        /// <summary>
        /// Get the same region all peers beside self
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<MarsPeer> HandleAccountListOnlineBySamePos(MarsPeer p)
        {
            //get all online peers, in public region
            List<MarsPeer> peers = ActorCollection.Instance.HandleAccountListOnline((MarsPeer peer) =>
            {
                return peer.accountId == p.accountId || peer.region != p.region;////account is self, or not in same pos, don't add list Peer
            });
            return peers;
        }

        /// <summary>
        /// return is BOOL by handleRoleListLimit
        /// BOOL is true, not add List, e.g it is self, or diff region
        /// else is add the list
        /// </summary>
        /// <param name="handleRoleListLimit"></param>
        /// <returns></returns>
        public List<Role> HandleRoleListOnline(HandlePeerListLimit handlePeerListLimit)
        {
            bool isAllAdd = (handlePeerListLimit == null);
            List<Role> roles = new List<Role>();
            for (int i = 0; i < accountIds.Count; i++)
            {
                MarsPeer peer = allUserByAccountId[accountIds[i]];
                if (peer == null) continue;
                Role role = peer.role;
                if (role == null) continue;
                if (isAllAdd == false)
                {
                    if (handlePeerListLimit(peer) == true)
                    {
                        continue;
                    }
                }
                role.x = peer.x;
                role.z = peer.z;
                role.xRo = peer.xRo;
                role.zRo = peer.zRo;
                roles.Add(role);
            }
            return roles;
        }

        /// <summary>
        /// get same region's roles
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<Role> HandleRoleListOnlineBySamePos(MarsPeer p)
        {
            List<Role> roles = ActorCollection.Instance.HandleRoleListOnline((MarsPeer peer) =>
            {
                return peer.accountId == p.accountId || peer.region != p.region;//account is self, or not in same pos, don't add list Peer
            }); ;
            
            return roles;
        }
    }
}
