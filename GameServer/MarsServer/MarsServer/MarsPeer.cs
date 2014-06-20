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
        //public Guid peerGuid { get; protected set; }
        public long accountId;
        public Role role;


        public MarsPeer(IRpcProtocol rpc, IPhotonPeer peer)
            : base(rpc, peer)
        {
            initialization();
            LinkServerSuccess();
        }

        void initialization()
        {
            //peerGuid = Guid.NewGuid();
            //PlayersManager.instance.AddUser(peerGuid, this);
        }

        private void LinkServerSuccess()
        {
            Bundle bundle = new Bundle();
            bundle.cmd = Command.LinkServer;
            SendToClient(bundle);
            /*string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)Command.LinkServer, json);
            OperationResponse response = new OperationResponse((byte)Command.LinkServer, parameter) { ReturnCode = 1, DebugMessage = "" };

            SendOperationResponse(response, new SendParameters());*/
        }

        public void StopLinked()
        {
            PlayersManager.instance.RemoveUser(accountId);
            Debug.Log("Client has Diconnected" + accountId);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            StopLinked();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            byte command = operationRequest.OperationCode;
            string getJson = "";
            if (operationRequest.Parameters != null && operationRequest.Parameters.Count > 0)
            {
                getJson = operationRequest.Parameters[command].ToString();
                Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<respone>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Debug.Log(getJson);
            }
            Bundle bundle = null;
            //Command
            if (command == (byte)Command.ServerSelect)
            {
                Server server = JsonConvert.DeserializeObject<Server>(getJson);
                bundle = new Bundle ();
                bundle.cmd = Command.ServerSelect;

                /**/
                accountId = server.accountId;
                string message = PlayersManager.instance.AddUser(accountId, this);

                if (message == null)
                {
                    bundle.server = server;
                    bundle.roles = RoleMySQL.instance.GetDataList(server.accountId);
                }
                else
                {
                    bundle.error = new Error();
                    bundle.error.message = message;
                }
            }
            else if (command == (byte)Command.CreatRole)
            {
                Role role = JsonConvert.DeserializeObject<Role>(getJson);
                bundle = new Bundle();
                bundle.cmd = Command.CreatRole;
                string message = RoleMySQL.instance.CreatRole(role);
                long id = 0;
                bool isSuccess = long.TryParse(message, out id);
                if (isSuccess)
                {
                    role.roleId = id;
                    bundle.role = role;
                }
                else
                {
                    bundle.error = new Error();
                    bundle.error.message = NetError.ROLR_CREAT_ERROR;
                }
            }
            else if (command == (byte) Command.AbortDiscount)
            {
                AbortConnection();
            }
            else if (command == (byte)Command.EnterGame)
            {
                Role role = JsonConvert.DeserializeObject<Role>(getJson);
                this.role = role;
                bundle.cmd = Command.EnterGame;
                bundle.role = role;
            }
            if (bundle != null)
            {
                SendToClient(bundle);
            }
        }

        public void SendToClient(Bundle bundle)
        {
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)bundle.cmd, json);
            OperationResponse response = new OperationResponse((byte)bundle.cmd, parameter) { ReturnCode = 1, DebugMessage = "" };

            SendOperationResponse(response, new SendParameters());
        }

        /*void SendEventToClient(Bundle bundle)
        {
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)bundle.cmd, json);
            OperationResponse response = new OperationResponse((byte)bundle.cmd, parameter) { ReturnCode = 1, DebugMessage = "" };

            SendOperationResponse(response, new SendParameters());
        }*/
    }
}
