using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;

using ExitGames.Concurrency.Fibers;

using EZProtocol;


namespace EZServer
{
    
    public class EZServerPeer : PeerBase
    {
       private readonly IFiber fiber;

        public Guid peerGuid {get; protected set;}
        private EZServerApplication _server;

        
        #region
        public EZServerPeer(IRpcProtocol rpcProtocol, IPhotonPeer nativePeer, EZServerApplication ServerApplication)
            : base(rpcProtocol, nativePeer)
        {

            //Log.Debug("start");
            //DBSQLManager.instance.LoadDB();
           

            peerGuid = Guid.NewGuid ();
            _server = ServerApplication;
            this.fiber = new PoolFiber ();
            this.fiber.Start ();
            _server.Actors.AddConnectedPeer(peerGuid, this);

            Handshake();
        }

        private void Handshake()
        {
            Bundle bundle = new Bundle();
            bundle.cmd = Command.Handshake;
            bundle.sqliteVer = new SQLiteVer();
            bundle.sqliteVer.ver = SQLiteVer.SQLITE_VER;
            bundle.sqliteVer.url = SQLiteVer.SQLITE_PATH_URL;
            string json = JsonConvert.SerializeObject(bundle);
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)Command.Handshake, json);
            OperationResponse response = new OperationResponse((byte)Command.Handshake, parameter) { ReturnCode = (short)ErrorCode.Ok, DebugMessage = "" };

            SendOperationResponse(response, new SendParameters());
        }

        /*public static Account getAccountByDict(long uniqueID)
        {
            foreach (Account a in users.Values)
            {
                if (a.uniqueId == uniqueID)
                {
                    return a;
                }
            }
            return null;
        }*/
        #endregion

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
          
            Debug.Log("Client has Diconnected");
            // 失去連線時要處理的事項，例如釋放資源

            Actor actor = _server.Actors.GetActorFromGuid(this.peerGuid);
            PlayersCollection.instance.Remove(actor.uniqueID);
           // Debug.Log("Client has Diconnected" + actor);
            if (actor != null)
            {
                if (actor.roomIndex > 0)
                {
                    //Log.Debug("1");
                    // 呼叫Application廣播給聊天室所有玩家將此玩家移除
                    _server.BroadcastRoomActorQuit(actor.roomIndex, actor);

                    // 從房間移除
                    _server.Rooms.RoomList[actor.roomIndex].Quit(actor.uniqueID);
                }
                else
                {
                    //Log.Debug("2");
                    _server.BroadcastDisconnect(actor.uniqueID);
                    // 從大廳移除
                    _server.lobbyActors.Remove(actor.uniqueID);
                }
            }


            _server.Actors.ActorOffline(peerGuid);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            #region Photon Debug
            byte command = operationRequest.OperationCode;
            string getJson = operationRequest.Parameters[command].ToString();
            //Show log.....
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<" + getJson + ">>>>>>>>>>>>>>>>>>>>>>>>>>>");

            Bundle bundle = new Bundle();
            String json = "";
            if (command == (byte)Command.Register)
            {

                Account account = JsonConvert.DeserializeObject<Account>(getJson);
                string message = MysqlAccount.instance.Register(account);
                //Debug.Log("<<<<<<<<<<<"  + message);
                //string message = SQLiteManager.instance.Register(account);
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
                    bundle.error = message;
                }
                json = JsonConvert.SerializeObject(bundle);
            }
            else if (command == (byte)Command.Login)
            {
                Account account = JsonConvert.DeserializeObject<Account>(getJson);
                string message = MysqlAccount.instance.Login(account);
                bundle.cmd = Command.Login;
                long loginUniqueID = 0;
                bool isSuccess = long.TryParse(message, out loginUniqueID);
                if (isSuccess)
                {
                    Account loginAccount = account;
                    account.uniqueId = loginUniqueID;
                    ActorCollection.ActorReturn ar = _server.Actors.ActorOnline(peerGuid, loginAccount.uniqueId);
                    bundle.account = loginAccount;
                    bundle.cmd = Command.Login;
                    json = JsonConvert.SerializeObject(bundle);

                    //Join public zone : lobby
                    //Log.Debug(loginAccount.uniqueId);
                    _server.lobbyActors.Add(loginAccount.uniqueId);
                    Player p = new Player();
                    p.uniqueId = loginAccount.uniqueId;
                    p.x = 0;
                    p.z = 0;
                    p.zRo = 0;
                    p.xRo = 0;
                    p.roleName = loginAccount.roleName;
                    p.actionId = 0;
                    PlayersCollection.instance.Add(p);

                    //if you login now, it can notice all public zone's players
                    _server.BroadcastLobby(loginAccount);

                    //if you login now, it can notice all players online
                    _server.initAllPlayers(loginAccount);
                }
                else
                {
                    bundle.error = message;
                    json = JsonConvert.SerializeObject(bundle);
                }
            }
            else if (command == (byte)Command.UpdatePlayer)
            {
                Player player = JsonConvert.DeserializeObject<Player>(getJson);
                PlayersCollection.instance.ModifyState(player.uniqueId, player.x, player.z, player.xRo, player.zRo);
                _server.BroadcastUpdatePlayer(player);

            }
            if (json != "")
            {
                Dictionary<byte, object> parameter = new Dictionary<byte, object>();
                parameter.Add(operationRequest.OperationCode, json);
                OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter) { ReturnCode = (short)ErrorCode.Ok, DebugMessage = "" };

                SendOperationResponse(response, new SendParameters());
            }
           
            #endregion
       }
    }
}