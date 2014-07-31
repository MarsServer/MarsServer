using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class MessageManager
    {
        public static readonly MessageManager instance = new MessageManager();

        public void OnOperationRequest(Bundle bundle, MarsPeer p)
        {
            Message message = bundle.message;

            List<MarsPeer> peers = new List<MarsPeer>();
            if (message.chatType == ChatType.World)
            {
                //world chat.....
                //get all online peers, in Game beside select role
                peers = ActorCollection.Instance.HandleAccountListOnlineByOthers(p);
            }
            if (peers.Count > 0)
            {
                MarsPeer.BroadCastEvent(peers, bundle);
            }
        }
    }
}
