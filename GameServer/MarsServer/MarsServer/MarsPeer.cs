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
        public long accountId;
        public long roleId;


        #region constructor And HandshakeHandle
        public MarsPeer(IRpcProtocol rpc, IPhotonPeer peer)
            : base(rpc, peer)
        {
            HandshakeHandle();
        }

         public void HandshakeHandle()
         {
             Bundle bundle = new Bundle();
             bundle.cmd = Command.LinkServer;
             SendClientEvent(bundle);
         }
        #endregion

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
            string getClientJson = null;//is client's json.
            Bundle bundle = null;//this will be send to client's data
            if (operationRequest.Parameters != null && operationRequest.Parameters.Count > 0)
            {
                getClientJson = operationRequest.Parameters[key].ToString ();
            }
            switch (cmd)
            {
                case Command.ServerSelect:
                    bundle = HandleServerSelectOnOperation(getClientJson, cmd);
                    break;
                case Command.CreatRole:
                    break;
            }

            if (bundle != null)
            {
                bundle.cmd = cmd;
                SendClientEvent(bundle);
            }
        }

        #region HandleServerSelectOnOperation
        Bundle HandleServerSelectOnOperation(string json, Command cmd)
        {
            Server server = JsonConvert.DeserializeObject<Server>(json);
            Bundle bundle = new Bundle();

            accountId = server.accountId;

            long id = 0;
            bool isLogined = ActorCollection.Instance.HandleRoleLogin(accountId, this);
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


        #region Link Client Event
        public void SendClientEvent(Bundle bundle)
        {
            OperationResponse response = new OperationResponse((byte)bundle.cmd, GetParameter (bundle)) { ReturnCode = 1, DebugMessage = "" };
            SendOperationResponse(response, new SendParameters());
        }

        public void BroadCastEvent(List<MarsPeerOld> peers, Bundle bundle)
        {
            if (peers.Count > 0 && bundle != null)
            {
                EventData eventData = new EventData((byte)bundle.cmd, GetParameter(bundle));
                ApplicationBase.Instance.BroadCastEvent<MarsPeerOld>(eventData, peers, new SendParameters());
            }
        }

        private Dictionary<byte, object> GetParameter(Bundle bundle)
        {
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)bundle.cmd, json);
            return parameter;
        }
        #endregion
    }
}
