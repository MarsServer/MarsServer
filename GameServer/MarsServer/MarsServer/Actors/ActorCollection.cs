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
        private List<long> accountIds = new List<long>();

        public string Size
        {
            get 
            {
                return allUserByAccountId.Count + "/" + accountIds.Count;
            }
        }
        /*private Dictionary<long, MarsPeer> allUserByRoleId = new Dictionary<long, MarsPeer>();//key is roleId;
        private Dictionary<string, MarsPeer> allUserByRoleName = new Dictionary<string, MarsPeer>();//key is roleName*/

        
        public bool HandleRoleLogin(long accountId, MarsPeer peer)
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

        public void HandleDisconnect(long accountId)
        {
            allUserByAccountId.Remove(accountId);
            accountIds.Remove(accountId);
        }
    }
}
