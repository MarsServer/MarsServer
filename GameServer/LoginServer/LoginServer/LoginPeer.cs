using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace LoginServer
{
    public class LoginPeer : PeerBase
    {
        public LoginPeer (IRpcProtocol rpc, IPhotonPeer peer) : base (rpc, peer)
        {
            Handshake();
        }

        private void Handshake()
        {
            Bundle bundle = new Bundle();
            bundle.cmd = Command.Handshake;
            bundle.sqliteVer = SqliteSQL.instance.GetValueByK (0);
            //bundle.sqliteVer.ver = SQLiteVer.SQLITE_VER;
            //bundle.sqliteVer.url = SQLiteVer.SQLITE_PATH_URL;
            /*bundle.sqliteVer.test = "test";*/
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
            if (command == (byte)Command.Register)
            {
                Account account = JsonConvert.DeserializeObject<Account>(getJson);
                string message = MysqlAccount.instance.Register(account);
                //Debug.Log("<<<<<<<<<<<"  + message);
                //string message = SQLiteManager.instance.Register(account);
                bundle = new Bundle();
                bundle.cmd = Command.Register;
                long newUniqueID = 0;
                bool isUniqueID = long.TryParse(message, out newUniqueID);
                if (isUniqueID == true)
                {
                    account.uniqueId = newUniqueID;
                    bundle.account = account;
                }
                else
                {
                    bundle = new Bundle();
                    bundle.error = new Error ();
                    bundle.error.message = message;
                }
                //
            }
            else if (command == (byte)Command.Login)
            {
                bundle = new Bundle();
                Account account = JsonConvert.DeserializeObject<Account>(getJson);
                string message = MysqlAccount.instance.Login(account);
                bundle.cmd = Command.Login;
                long loginUniqueID = 0;
                bool isSuccess = long.TryParse(message, out loginUniqueID);
                if (isSuccess)
                {
                    Account loginAccount = account;
                    account.uniqueId = loginUniqueID;
                    bundle.account = loginAccount;
                    bundle.cmd = Command.Login;
                    bundle.serverList = ServerController.instance.servers;
                    
                }
                else
                {
                    bundle.error = new Error ();
                    bundle.error.message = message;
                }
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
