using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class MarsPeer : PeerBase
    {
        #region property
        public long accountId { get; private set; }
        public long roleId { get; private set; }
        public Role role { get { if (roleId == 0) return null; return RoleMySQL.instance.getRoleByRoleId(roleId); } }

        /// <summary>
        /// region is that player now in which region. 0-publiczone, roomId-other
        /// </summary>
        public int region { get; private set; }
        public float x { get; private set; }
        public float z { get; private set; }
        public float xRo { get; private set; }
        public float zRo { get; private set; }
        public int acion { get; private set; }
        #endregion

        #region constructor & HandshakeHandle & ClearData
        public MarsPeer(IRpcProtocol rpc, IPhotonPeer peer)
            : base(rpc, peer)
        {
            HandshakeHandle();
        }

         private void HandshakeHandle()
         {
             Bundle bundle = new Bundle();
             bundle.cmd = Command.LinkServer;
             SendClientEvent(bundle);
         }

         private void ClearData()//Switch role clear data
         {
             //accountId = 0;
             roleId = 0;
             region = 0;
             x = 0;
             z = 0;
             xRo = 0;
             zRo = 0;
             acion = 0;
         }

        #endregion

        #region Photon API
        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            ActorCollection.Instance.HandleDisconnect(accountId);
            Debug.Log(string.Format ("OnDisconnect: conId={0}, reason={1}, reasonDetail={2}, count = {3}", ConnectionId, reasonCode, reasonDetail, ActorCollection.Instance.Size));
            //this.Dispose();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            byte key = operationRequest.OperationCode;//client's key
            Command cmd = (Command)key;//convert to Command
            string json = null;//is client's json.
            Bundle bundle = null;//this will be send to client's data
            if (operationRequest.Parameters != null && operationRequest.Parameters.Count > 0)
            {
                json = operationRequest.Parameters[key].ToString ();
            }
            switch (cmd)
            {
                case Command.ServerSelect:
                    bundle = HandleServerSelectOnOperation(json, cmd);
                    break;
                case Command.CreatRole:
                    bundle = HandleCreatRoleOnOperation(json, cmd);
                    break;
                case Command.EnterGame:
                    bundle = HandleEnterGameOnOperation(json, cmd);
                    break;
                case Command.UpdatePlayer:
                    HandleUpdatePlayerOnOperation(json, cmd);
                    return;
                case Command.SendChat:
                    HandleSendChatOperation(json, cmd);
                    return;
            }

            if (bundle != null)
            {
                bundle.cmd = cmd;
                SendClientEvent(bundle);
            }
        }
        #endregion

        #region HandleServerSelectOnOperation
        Bundle HandleServerSelectOnOperation(string json, Command cmd)
        {
            Server server = JsonConvert.DeserializeObject<Server>(json);
            Bundle bundle = new Bundle();

            accountId = server.accountId;

            bool isLogined = ActorCollection.Instance.HandleAccountLogin(accountId, this);
            if (isLogined == false)
            {
                bundle.error = new Error();
                bundle.error.message = NetError.SAME_ACCOUNT_DISCOUNT_ERROR;
            }
            else
            {
                bundle.roles = RoleMySQL.instance.GetDataListByAccountId(accountId);
            }
            return bundle;
        }
        #endregion

        #region HandleCreatRoleOnOperation
        Bundle HandleCreatRoleOnOperation(string json, Command cmd)
        {
            Role role = JsonConvert.DeserializeObject<Role>(json);
            Bundle bundle = new Bundle();

            role.accountId = accountId;

            string msg = RoleMySQL.instance.CreatRole(role);
            long id = 0;
            bool isSuccess = long.TryParse(msg, out id);
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

            return bundle;
        }
        #endregion

        #region HandleEnterGameOnOperation
        Bundle HandleEnterGameOnOperation(string json, Command cmd)
        {
            Role role = JsonConvert.DeserializeObject<Role>(json);
            Bundle bundle = new Bundle();

            //Self's role
            roleId = role.roleId;
            Role newRole = RoleMySQL.instance.getRoleByRoleId(roleId);
            if (newRole != null)
            {
                region = Constants.PUBLICZONE;
                newRole.region = region;
                bundle.onlineRoles = ActorCollection.Instance.HandleRoleListOnline((MarsPeer peer) =>
                    {
                        return peer.accountId == accountId || peer.region != region;//account is self, or not in same pos, don't add list Peer
                    });
                bundle.role = newRole;
            }
            //get all online peers, in public region
            List<MarsPeer> peers = ActorCollection.Instance.HandleAccountListOnline((MarsPeer peer) =>
            {
                return peer.accountId == accountId || peer.region != region;////account is self, or not in same pos, don't add list Peer
            });
            //new Role
            if (peers.Count >= 0)
            {
                Role mRole = new Role();
                mRole.roleId = newRole.roleId;
                mRole.profession = newRole.profession;
                Bundle mBundle = new Bundle();
                mBundle.cmd = Command.AddNewPlayer;                   
                mBundle.role = mRole;
                BroadCastEvent(peers, mBundle);
            }

            return bundle;
        }
        #endregion

        #region HandleUpdatePlayerOnOperation
        void HandleUpdatePlayerOnOperation(string json, Command cmd)
        {
            Role mRole = JsonConvert.DeserializeObject<Role>(json);
            x = mRole.x;
            z = mRole.z;
            xRo = mRole.xRo;
            zRo = mRole.zRo;
            acion = mRole.action;
            
            //get all online peers, in public region
            List<MarsPeer> peers = ActorCollection.Instance.HandleAccountListOnline((MarsPeer peer) =>
            {
                return peer.accountId == accountId || peer.region != region;//account is self, or not in same pos, don't add list Peer
            });
            if (peers.Count >= 0)
            {
                Bundle bundle = new Bundle();
                bundle.role = mRole;
                bundle.cmd = Command.UpdatePlayer;
                BroadCastEvent(peers, bundle);
            }
        }
        #endregion

        #region HandleSendChatOperation
        void HandleSendChatOperation(string json, Command cmd)
        {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            Bundle bundle = new Bundle();
            //boastcast message
            bundle.message = message;
            MessageManager.instance.OnOperationRequest(bundle, this);

        }
        #endregion

        #region Link Client Event
        public void SendClientEvent(Bundle bundle)
        {
            OperationResponse response = new OperationResponse((byte)bundle.cmd, GetParameter (bundle)) { ReturnCode = 1, DebugMessage = "" };
            SendOperationResponse(response, new SendParameters());
        }


        /// <summary>
        /// Broad all peers's pos
        /// </summary>
        /// <param name="peers"></param>
        /// <param name="bundle"></param>
        public static void BroadCastEvent(List<MarsPeer> peers, Bundle bundle)
        {
            if (peers.Count > 0 && bundle != null)
            {
                EventData eventData = new EventData((byte)bundle.cmd, GetParameter(bundle));
                ApplicationBase.Instance.BroadCastEvent<MarsPeer>(eventData, peers, new SendParameters());
            }
        }

        private static Dictionary<byte, object> GetParameter(Bundle bundle)
        {
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)bundle.cmd, json);
            return parameter;
        }
        #endregion
    }
}
