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
        public Guid peerGuid { get; protected set; }
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
            peerGuid = Guid.NewGuid();
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
            if (role != null)
            {
                Bundle bundle = new Bundle();
                bundle.cmd = Command.DestroyPlayer;
                bundle.role = role;
                PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                {
                    if (role != null && role.region == 0)
                    {
                       peer.SendToClient(bundle);
                    }
                });
            }

            PlayersManager.instance.RemoveUser(this);
            Debug.Log("Client has Diconnected" + accountId + "____" + PlayersManager.instance.size);
            this.Dispose();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            StopLinked();
         //   StopLinked();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            byte cmd = operationRequest.OperationCode;
            string getJson = "";
            if (operationRequest.Parameters != null && operationRequest.Parameters.Count > 0)
            {
                getJson = operationRequest.Parameters[cmd].ToString();
                Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<respone>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Debug.Log(getJson);
            }
            Bundle bundle = null;
            //Command
            if (cmd == (byte)Command.ServerSelect)
            {
                Server server = JsonConvert.DeserializeObject<Server>(getJson);
                bundle = new Bundle ();
                bundle.cmd = Command.ServerSelect;

                /**/
                accountId = server.accountId;
                string message = PlayersManager.instance.AddUser(accountId, peerGuid, this);

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
            else if (cmd == (byte)Command.CreatRole)
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
            else if (cmd == (byte) Command.AbortDiscount)
            {
                StopLinked();
                //AbortConnection();
            }
            else if (cmd == (byte)Command.EnterGame)
            {
                Role role = JsonConvert.DeserializeObject<Role>(getJson);
                this.role = role;
                bundle = new Bundle();
                bundle.cmd = Command.EnterGame;
                bundle.role = role;
                bundle.role.region = 0;
                PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                {
                    if (role != null && role.region == 0)
                    {
                        Bundle newbundle = new Bundle();
                        newbundle.cmd = Command.AddNewPlayer;
                        newbundle.role = role;
                        newbundle.role.region = 0;
                        peer.SendToClient(newbundle);
                    }
                });
                bundle.onlineRoles = PlayersManager.instance.GetAllListRole(accountId);
                
            }
            else if (cmd == (byte)Command.SendChat)
            {
                Message message = JsonConvert.DeserializeObject<Message>(getJson);
                bundle = new Bundle();
                bundle.cmd = Command.SendChat;
                bundle.message = message;
                PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                    {
                        peer.SendToClient(bundle);
                    });
                //not send myself
                return;
            }
            else if (cmd == (byte)Command.UpdatePlayer)
            {
                Role r = JsonConvert.DeserializeObject<Role>(getJson);
                this.role.x = r.x;
                this.role.z = r.z;
                this.role.xRo = r.xRo;
                this.role.zRo = r.zRo;
                this.role.action = r.action;

                bundle = new Bundle();
                bundle.cmd = Command.UpdatePlayer;
                bundle.role = r;
                PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                {
                    if (role != null && role.region == 0)
                    {
                        peer.SendToClient(bundle);
                    }
                });
                return;
            }
            else if (cmd == (byte) Command.EnterFight)
            { 
                
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
