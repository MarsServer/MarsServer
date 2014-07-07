using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class PlayersManager
    {
        public delegate bool BroadcastPlayerInfo(MarsPeer peer);

        public readonly static PlayersManager instance = new PlayersManager();

        private Dictionary<Guid, MarsPeer> users = new Dictionary<Guid, MarsPeer>();
       // private Dictionary<long, MarsPeer> userIdDict = new Dictionary<long, MarsPeer>();
        private List<MarsPeer> allusers = new List<MarsPeer>();

        public int size { get { return users.Count; } }

        public string AddUser(long accountId, Guid guidPeer, MarsPeer marsPeer)
        {
            bool isLogined = false;//users.ContainsKey(accountId);
            MarsPeer _marsPeer = null;
            //isLogined = userIdDict.TryGetValue(accountId, out _marsPeer);
            foreach (MarsPeer peer in allusers)
            {
                if (peer.accountId == accountId)
                {
                    isLogined = true;
                    _marsPeer = peer;
                    break;
                }
            }
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<" + users.Count + ">>>>>>>>>>>>>>>>>>>>>>>>");
            if (isLogined == false)//not login
            {
                users.Add(guidPeer, marsPeer);
                //userIdDict.Add(accountId, marsPeer);
                allusers.Add(marsPeer);
            }
            else//has logined
            {
                Bundle bundle = new Bundle();
                bundle.cmd = Command.NetError;
                bundle.error = new Error();
                bundle.error.message = NetError.SAME_ACCOUNT_DISCOUNT_ERROR;
                _marsPeer.SendToClient(bundle);
                _marsPeer.StopLinked();
                return NetError.SAME_ACCOUNT_ERROR;
            }
            return null;
        }

        public void RemoveUser(MarsPeer peer)
        {
            users.Remove(peer.peerGuid);
            //userIdDict.Remove(peer.accountId);
            allusers.Remove(peer);
        }

        public List<MarsPeer> BroastPlayerSomething(long accountId, BroadcastPlayerInfo broadcastPlayerInfo)//dont send to myself
        {
            return BroastPlayerSomething(accountId, false, broadcastPlayerInfo);
        }
        public List<MarsPeer> BroastPlayerSomething(long accountId, bool isContain, BroadcastPlayerInfo broadcastPlayerInfo)
        {
            List<MarsPeer> peers = new List<MarsPeer>();
            foreach (MarsPeer peer in allusers)
            {
                if (peer.accountId == accountId && isContain == false) continue;
                if (broadcastPlayerInfo != null)
                {
                    if (broadcastPlayerInfo(peer))
                    {
                        peers.Add(peer);
                    }
                }
            }
            return peers;
        }

        public List<Role> GetAllListRoleBeSideMe (long accountId)
        {
            List<Role> roles = new List<Role>();
            foreach (MarsPeer peer in allusers)
            {
                if (peer.accountId == accountId) continue;
                Role r = peer.role;
                if (peer != null)
                {
                    roles.Add(r);
                }
            }
            return roles;
        }

        public void UpdateRoleInfo(Guid peerGuid, Role role)
        {
            users[peerGuid].role = role;
        }
    }
}
