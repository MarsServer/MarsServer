using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class PlayersManager
    {
        public delegate void BroadcastPlayerInfo(MarsPeer peer);

        public readonly static PlayersManager instance = new PlayersManager();

        private Dictionary<Guid, MarsPeer> users = new Dictionary<Guid, MarsPeer>();

        public int size { get { return users.Count; } }

        public string AddUser(long accountId, Guid guidPeer, MarsPeer marsPeer)
        {
            bool isLogined = false;//users.ContainsKey(accountId);
            MarsPeer _marsPeer = null;
            foreach (KeyValuePair<Guid, MarsPeer> kvp in users)
            {
                if (kvp.Value.accountId == accountId)
                {
                    isLogined = true;
                    _marsPeer = kvp.Value;
                    break;
                }
            }
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<" + users.Count + ">>>>>>>>>>>>>>>>>>>>>>>>");
            if (isLogined == false)//not login
            {
                users.Add(guidPeer, marsPeer);
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

        public void RemoveUser(Guid accountId)
        {
            users.Remove(accountId);
        }

        public void BroastPlayerSomething(Guid accountId, BroadcastPlayerInfo broadcastPlayerInfo)//dont send to myself
        {
            BroastPlayerSomething(accountId, false, broadcastPlayerInfo);
        }
        public void BroastPlayerSomething(Guid accountId, bool isContain, BroadcastPlayerInfo broadcastPlayerInfo)
        {
            foreach (KeyValuePair<Guid, MarsPeer> kvp in users)
            {
                if (kvp.Key == accountId && isContain == false)
                {
                    continue;
                }
                if (broadcastPlayerInfo != null)
                {
                    broadcastPlayerInfo(kvp.Value);
                }
            }
        }
    }
}
