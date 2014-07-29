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
            return new MarsPeerOld (initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            Debug.instance.Setup(this);
            Debug.Log("Mars Server Running.....");

            RoleMySQL.instance.Init();
            Debug.Log("Role is Ok...............................................");

            Property.instance.Init();
            Debug.Log("Property is Ok...............................................");

            ExpMySQL.instance.Init();
            Debug.Log("ExpMySQL is Ok...............................................");
        }

        protected override void TearDown()
        {

        }
    }
}
