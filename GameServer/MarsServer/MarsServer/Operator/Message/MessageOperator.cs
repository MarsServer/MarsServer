using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class MessageOperator : Game
    {
        public MessageOperator(MarsPeer peer)
            : base(peer)
        { 
        }

        public override void ExecuteOperation(Command cmd, params object[] objs)
        {
            base.ExecuteOperation(cmd, objs);

            Message message = (Message)objs[0];

            bundle.message = message;
            List<MarsPeer> peers = null;// new List<MarsPeer>();
            switch (message.chatType)
            {
                case ChatType.World:
                    peers = HandleWorldChat();
                    break;
                case ChatType.Team:
                    HandleTeamChat();
                    return;
            }
            if (peers != null && peers.Count > 0)
            {
                MarsPeer.BroadCastEvent(peers, bundle);
            }

        }

        List<MarsPeer>  HandleWorldChat()
        {

            //world chat.....
            //get all online peers, in Game beside select role
            List<MarsPeer> peers = Actor.Instance.HandleAccountListOnlineByOthers(this.marsPeer);
            return peers;
        }

        void HandleTeamChat()
        {
            if (this.marsPeer.team != null)
            {
                RoomInstance.instance.BroadcastEvent(this.marsPeer, bundle, Room.BroadcastType.Notice);
            }
        }

        /*public void OnOperationRequest(Bundle bundle, MarsPeer p)
        {
            Message message = bundle.message;

            List<MarsPeer> peers = new List<MarsPeer>();
            if (message.chatType == ChatType.World)
            {
                //world chat.....
                //get all online peers, in Game beside select role
                peers = Actor.Instance.HandleAccountListOnlineByOthers(p);
            }
            else if (message.chatType == ChatType.Team)
            {
                if (p.team != null)
                {
                    RoomInstance.instance.BroadcastEvent(p, bundle, Room.BroadcastType.Notice);
                }
                return;
            }
            if (peers.Count > 0)
            {
                MarsPeer.BroadCastEvent(peers, bundle);
            }
        }*/
    }
}
