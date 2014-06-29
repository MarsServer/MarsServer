using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Photon.SocketServer;

namespace LoginServer
{
    public class LoginApplication : ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new LoginPeer(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            Debug.instance.Setup(this);
            ServerController.instance.GetServerList();
            Debug.Log("Login Server Running.....");
        }

        protected override void TearDown()
        {
            
        }
    }
}
