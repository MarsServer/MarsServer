using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Client.Photon;

namespace MarsServer
{
    public class LoginServer : IPhotonPeerListener
    {
        public void InitLoginServer()
        {
            PhotonPeer peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            if (peer.Connect("192.168.1.102:5055", "LoginServerServer"))
            {
                do
                {
                    //Console.WriteLine("."); // 􄔓􁾳􆺆􁺋􅠐􄦳􂺷􆀬􃳣􅏿
                    peer.Service();
                    System.Threading.Thread.Sleep(500); //0.5s
                }
                    while (true);
            }
        }
        public void DebugReturn(DebugLevel level, string message)
        {
            //throw new NotImplementedException();
        }

        public void OnEvent(EventData eventData)
        {
            //throw new NotImplementedException();
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
           // throw new NotImplementedException();
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            //throw new NotImplementedException();
        }
    }
}
