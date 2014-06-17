using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MarsServer
{
    public class MarsPeer : PeerBase
    {
        public MarsPeer(IRpcProtocol rpc, IPhotonPeer peer)
            : base(rpc, peer)
        {
            LinkServerSuccess();
        }

        private void LinkServerSuccess()
        {
            Bundle bundle = new Bundle();
            bundle.cmd = Command.LinkServer;
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)Command.LinkServer, json);
            OperationResponse response = new OperationResponse((byte)Command.LinkServer, parameter) { ReturnCode = 1, DebugMessage = "" };

            SendOperationResponse(response, new SendParameters());
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Debug.Log("Client has Diconnected");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            byte command = operationRequest.OperationCode;
            string getJson = operationRequest.Parameters[command].ToString();
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<respone>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Debug.Log( getJson );
            Bundle bundle = null;
            //Command
            if (command == (byte)Command.ServerSelect)
            {
                Server server = JsonConvert.DeserializeObject<Server>(getJson);
                bundle = new Bundle ();
                bundle.cmd = Command.ServerSelect;
                bundle.server = server;
            }
            else if (command == (byte)Command.CreatRole)
            {
                Role role = JsonConvert.DeserializeObject<Role>(getJson);
            }
            if (bundle != null)
            {
                string json = JsonConvert.SerializeObject(bundle);
                 Dictionary<byte, object> parameter = new Dictionary<byte, object>();
                parameter.Add(operationRequest.OperationCode, json);
                OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter) { ReturnCode = 1, DebugMessage = "" };

                SendOperationResponse(response, new SendParameters());
            }
        }
    }
}
