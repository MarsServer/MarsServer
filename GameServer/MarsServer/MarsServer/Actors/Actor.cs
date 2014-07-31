using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class Actor
    {
        public readonly static Actor Instance = new Actor();

        public ActorCollection Actors = new ActorCollection();

        /// <summary>
        /// Size for debug
        /// </summary>
        public string Size
        {
            get 
            {
                return "*******/" + Actors.Count;//NEW TODO:
            }
        }
        
        /// <summary>
        /// add a peer by accountid
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="peer"></param>
        /// <returns></returns>
        public bool HandleAccountLogin(long accountId, MarsPeer peer)
        {
            MarsPeer getPeer = null;
            //if ( .TryGetValue(accountId, out getPeer) == false)
            if (Actors.TryGetValue (accountId, out getPeer) == false)
            {
                Actors.Add(peer);//NEW TODO:
                Debug.Log("Start..................................." + Size);
                return true;
            }
            else
            {
                getPeer.Disconnect();
                getPeer.Dispose();
                getPeer = null;
                return false;
            }
        }

        /// <summary>
        /// Remove a peer by accountid
        /// </summary>
        /// <param name="accountId"></param>
        public void HandleDisconnect(MarsPeer peer)
        {
            Actors.Remove(peer);//NEW TODO:
        }

        /// <summary>
        /// Get beside me all peers
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public List<MarsPeer> HandleAccountListOnlineByOthers(MarsPeer peer)
        {
            return Actors.GetPeersByOthers(peer); ;
        }

        /// <summary>
        /// Get the same region all peers beside self
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public List<MarsPeer> HandleAccountListOnlineBySamePos(MarsPeer peer)
        {
            return Actors.GetPeersBySamePos (peer);
        }
        
        /// <summary>
        /// get same region's roles
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public List<Role> HandleRoleListOnlineBySamePos(MarsPeer peer)
        {
            return Actors.GetRolesBySamePos (peer);
        }
    }
}
