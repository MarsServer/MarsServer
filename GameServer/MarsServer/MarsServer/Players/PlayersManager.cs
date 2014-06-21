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

        private Dictionary<long, MarsPeer> users = new Dictionary<long, MarsPeer>();

        public string AddUser(long accountId, MarsPeer marsPeer)
        {
            bool isLogined = users.ContainsKey(accountId);
            if (isLogined == false)//not login
            {
                users.Add(accountId, marsPeer);
            }
            else//has logined
            {
                Bundle bundle = new Bundle();
                bundle.cmd = Command.NetError;
                bundle.error = new Error();
                bundle.error.message = NetError.SAME_ACCOUNT_DISCOUNT_ERROR;
                users[accountId].SendToClient(bundle);
                users[accountId].StopLinked();
                return NetError.SAME_ACCOUNT_ERROR;
            }
            return null;
        }

        public void RemoveUser(long accountId)
        {
            users.Remove(accountId);
        }

        public void BroastPlayerSomething(long accountId, BroadcastPlayerInfo broadcastPlayerInfo)//dont send to myself
        {
            BroastPlayerSomething(accountId, false, broadcastPlayerInfo);
        }
        public void BroastPlayerSomething(long accountId, bool isContain, BroadcastPlayerInfo broadcastPlayerInfo)
        {
            foreach (KeyValuePair<long, MarsPeer> kvp in users)
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
