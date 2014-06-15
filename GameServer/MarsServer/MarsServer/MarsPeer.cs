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
            LoginServerSuccess();
        }

        void LoginServerSuccess()
        {
            Bundle bundle = new Bundle();
            bundle.cmd = Command.LoginSuccess;
            bundle.sqliteVer = new SQLiteVer();
            bundle.sqliteVer.ver = SQLiteVer.SQLITE_VER;
            bundle.sqliteVer.url = SQLiteVer.SQLITE_PATH_URL;
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)Command.Handshake, json);
            OperationResponse response = new OperationResponse((byte)Command.Handshake, parameter) { ReturnCode = 1, DebugMessage = "" };

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
