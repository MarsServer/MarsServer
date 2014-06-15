using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Photon.SocketServer;

namespace MarsServer
{
    public class MarsApplication : ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new MarsPeer (initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            Debug.instance.Setup(this);
            Debug.Log("Mars Server Running.....");
        }

        protected override void TearDown()
        {

        }
    }
}
