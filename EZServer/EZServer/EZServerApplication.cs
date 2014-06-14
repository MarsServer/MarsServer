using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using LogManager = ExitGames.Logging.LogManager;
using System.IO;
using EZProtocol;

namespace EZServer
{
    public class EZServerApplication : ApplicationBase
    {
        public ActorCollection Actors;
        public RoomCollection Rooms;
        public Lobby lobbyActors;

        private bool broadcastActive = false;
        private int lobbyBoradcastCounter;

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            //Link photon server
            return new EZServerPeer(initRequest.Protocol, initRequest.PhotonPeer, this);
        }

        protected override void Setup()
        {
          LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
          GlobalContext.Properties["LogFileName"] = ApplicationName + System.DateTime.Now.ToString("yyyy-MM-dd");
          XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(BinaryPath, "log4net.config")));
            // 初始化GameServer

            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");

            // log4net
            string path = Path.Combine(this.BinaryPath, "log4net.config");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }

            Debug.Log("EZServer is running...");

            Actors = new ActorCollection();
            Rooms = new RoomCollection();
            lobbyActors = new Lobby();

            lobbyBoradcastCounter = 0;

            // create broadcast threading
            broadcastActive = true;
            ThreadPool.QueueUserWorkItem(this.LobbyBroadcast);
          
        }

        protected override void TearDown()
        {
            broadcastActive = false;
        }
        private void LobbyBroadcast(object state)
        {
            /*while (broadcastActive)
            {
                try
                {
                    RoomInfo[] rInfos = Rooms.GetAllRoomInfo();

                    var parameter = new Dictionary<byte, object>();

                    for (int i = 0; i < rInfos.Length; i++)
                    {
                        var roomparameter = new Dictionary<byte, object> { 
                                              { (byte)GetRoomInfoEventCode.RoomIndex, rInfos[i].id }, {(byte)GetRoomInfoEventCode.RoomName, rInfos[i].RoomName}, {(byte)GetRoomInfoEventCode.Limit, rInfos[i].Limit}, {(byte)GetRoomInfoEventCode.ActorCount, rInfos[i].ActorCount}
                                           };

                        parameter.Add((byte)i, roomparameter);

                    }

                    var eventData = new EventData(
                        (byte)EZProtocol.EventCommand.LobbyBroadcast, parameter);


                    for (int i = 0; i < 20; i++) // 每一回合廣播20個玩家
                    {
                        if (lobbyBoradcastCounter < lobbyActors.ActorUniqueIDs.Count)
                        {
                            Actor actor = Actors.GetActor(lobbyActors.ActorUniqueIDs[lobbyBoradcastCounter]);

                            if (actor != null)
                            {
                                EZServerPeer peer = Actors.TryGetPeer(actor.guid);

                                if (peer != null)
                                {
                                    peer.SendEvent(eventData, new SendParameters());
                                }
                            }
                            lobbyBoradcastCounter++;
                        }
                        else  // 若廣播完一輪後計數器歸0後結束這一回合
                        {
                            lobbyBoradcastCounter = 0;
                            break;
                        }

                    }
                }
                catch (Exception EX)
                {
                    Log.Error("Lobby Broadcast Exception : " + EX.Message);
                }

                Thread.Sleep(500);
            }*/
        }

        // 將所有玩家資料廣播給一個玩家
        public void BroadcastRoomActorAllToOne(int RoomIndex, int ActorUniqueID)
        {
            // get peer fo actor
            Actor actor = Actors.GetActor(ActorUniqueID);

            if (actor != null)
            {
                EZServerPeer peer = Actors.TryGetPeer(actor.guid);

                if (peer != null)
                {
                    for (int i = 0; i < Rooms.RoomList[RoomIndex].ActorList.Count; i++)
                    {
                        var parameter = new Dictionary<byte, object> {
                        { (byte)RoomActorActionInfo.MemberUniqueID, Rooms.RoomList[RoomIndex].ActorList[i].uniqueID },
                        { (byte)RoomActorActionInfo.NickName, Rooms.RoomList[RoomIndex].ActorList[i].nickname },
                        { (byte)RoomActorActionInfo.PosX, Rooms.RoomList[RoomIndex].ActorList[i].PosX },
                        { (byte)RoomActorActionInfo.PosY, Rooms.RoomList[RoomIndex].ActorList[i].PosY },
                       
                        { (byte)RoomActorActionInfo.ActionNum, Rooms.RoomList[RoomIndex].ActorList[i].ActionNum }
                    };

                        var eventData = new EventData(
                        (byte)EZProtocol.EventCommand.RoomBroadcastActorAction, parameter);
                        peer.SendEvent(eventData, new SendParameters());
                    }
                }
            }
        }

        // 將一個玩家資料廣播給所有玩家
        public void BroadcastRoomActorOneToAll(int RoomIndex, int ActorUniqueID, string Nickname, float PosX, float PosY, float PosZ, short Direct, short ActionNum)
        {
            var parameter = new Dictionary<byte, object> {
                { (byte)RoomActorActionInfo.MemberUniqueID, ActorUniqueID },
                { (byte)RoomActorActionInfo.NickName, Nickname },
                { (byte)RoomActorActionInfo.PosX, PosX },
                { (byte)RoomActorActionInfo.PosY, PosY },
                { (byte)RoomActorActionInfo.PosZ, PosZ },
                { (byte)RoomActorActionInfo.Direct, Direct },
                { (byte)RoomActorActionInfo.ActionNum, ActionNum }
            };

            //Log.Info("BBBBBBBBBBB   nick = " + Nickname);
            var eventData = new EventData(
            (byte)EZProtocol.EventCommand.RoomBroadcastActorAction, parameter);

            for (int i = 0; i < Rooms.RoomList[RoomIndex].ActorList.Count; i++)
            {
                // get peer
                Actor actor = Actors.GetActor(Rooms.RoomList[RoomIndex].ActorList[i].uniqueID);
                if (actor != null)
                {
                    EZServerPeer peer = Actors.TryGetPeer(actor.guid);

                    if (peer != null)
                    {
                        peer.SendEvent(eventData, new SendParameters());
                    }
                }
            }
        }

        // 將玩家離開的事件廣播給房間所有人
        public void BroadcastRoomActorQuit(int RoomIndex, Actor _actor)
        {
            var parameter = new Dictionary<byte, object> {
                {(byte)RoomActorQuit.MemberUniqueName, _actor.nickname}, 
                {(byte)RoomActorQuit.MemberUniqueID, _actor.uniqueID}
            };
            var eventData = new EventData((byte)EZProtocol.EventCommand.RoomBroadcastActorQuit, parameter);
            for (int i = 0; i < Rooms.RoomList[RoomIndex].ActorList.Count; i++)
            {
                // get peer
                Actor actor = Actors.GetActor(Rooms.RoomList[RoomIndex].ActorList[i].uniqueID);

                if (actor != null)
                {
                    EZServerPeer peer = Actors.TryGetPeer(actor.guid);

                    if (peer != null)
                    {
                        peer.SendEvent(eventData, new SendParameters());
                    }
                }
            }
        }

        // 將某玩家講話的內容廣播給房間所有人
        public void BroadcastRoomSpeak(int RoomIndex, string ActorUniqueID, string TalkString)
        {
            var parameter = new Dictionary<byte, object> {
                { (byte)RoomActorSpeak.MemberUniqueName, ActorUniqueID },
                { (byte)RoomActorSpeak.TalkString, TalkString },
            };

            var eventData = new EventData((byte)EZProtocol.EventCommand.RoomBroadcastActorSpeak, parameter);

            for (int i = 0; i < Rooms.RoomList[RoomIndex].ActorList.Count; i++)
            {
                // get peer
                Actor actor = Actors.GetActor(Rooms.RoomList[RoomIndex].ActorList[i].uniqueID);
                EZServerPeer peer = Actors.TryGetPeer(actor.guid);

                if (peer != null)
                {
                    peer.SendEvent(eventData, new SendParameters());
                }
            }
        }


        public void initAllPlayers(Account account)
        {
            /*if (lobbyActors.ActorUniqueIDs == null && lobbyActors.ActorUniqueIDs.Count == 1)
            {
                return;
            }*/
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            Bundle bundle = new Bundle();
            bundle.players = new List<Player>();
            bundle.eventCmd = EventCommand.InitAllPlayer;
            for (int i = 0; i < lobbyActors.ActorUniqueIDs.Count; i++)
            {
                long uniqueID = lobbyActors.ActorUniqueIDs[i];
                if (account.uniqueId == uniqueID)
                {
                    continue;
                }
                //Account a = Main.instance.getAccountByUniqueID(uniqueID);

                /*Player p = new Player();
                p.uniqueId = uniqueID;
                p.x = 0;
                p.z = 0;
                p.roleName = a.roleName; ;*/
                bundle.players.Add(PlayersCollection.instance.getPlayerById (uniqueID));

            }
            string json = JsonConvert.SerializeObject(bundle);
            parameter.Add((byte)EventCommand.InitAllPlayer, json);
            EventData eventData = new EventData((byte)EventCommand.InitAllPlayer, parameter);
            Actor _actor = Actors.GetActor(account.uniqueId);
            EZServerPeer peer = Actors.TryGetPeer(_actor.guid);
            if (peer != null)
            {
                peer.SendEvent(eventData, new SendParameters());
            }
        }

        public void BroadcastLobby(Account account)
        {
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();

            Bundle bundle = new Bundle();
            bundle.account = account;
            bundle.eventCmd = EventCommand.LobbyBroadcast;
            string json = JsonConvert.SerializeObject (bundle);
           
            parameter.Add((byte)EventCommand.LobbyBroadcast,json );
            EventData eventData = new EventData((byte) EventCommand.LobbyBroadcast, parameter);
            for (int i = 0; i < lobbyActors.ActorUniqueIDs.Count; i++)
            {
                long uniqueID = lobbyActors.ActorUniqueIDs[i];
                if (account.uniqueId == uniqueID)
                {
                    continue;
                }
                Actor _actor = Actors.GetActor(uniqueID);
                if (_actor != null)
                {
                    EZServerPeer peer = Actors.TryGetPeer(_actor.guid);
                    // bundle.players.Add(EZServerPeer.getAccountByDict(uniqueID));
                    if (peer != null)
                    {
                        peer.SendEvent(eventData, new SendParameters());
                    }
                }
            }
            
        }

        public void BroadcastUpdatePlayer(Player p)
        {
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();

            Bundle bundle = new Bundle();
            bundle.player = p;
            bundle.eventCmd = EventCommand.UpdatePlayer;
            string json = JsonConvert.SerializeObject(bundle);

            parameter.Add((byte)EventCommand.UpdatePlayer, json);
            EventData eventData = new EventData((byte)EventCommand.UpdatePlayer, parameter);
            for (int i = 0; i < lobbyActors.ActorUniqueIDs.Count; i++)
            {
                long uniqueID = lobbyActors.ActorUniqueIDs[i];
                if (p.uniqueId == uniqueID)
                {
                    continue;
                }
                Actor _actor = Actors.GetActor(uniqueID);
                EZServerPeer peer = Actors.TryGetPeer(_actor.guid);
                // bundle.players.Add(EZServerPeer.getAccountByDict(uniqueID));
                if (peer != null)
                {
                    peer.SendEvent(eventData, new SendParameters());
                }
            }
        }

        public void BroadcastDisconnect(long uniqueID)
        {
           Dictionary<byte, object> parameter = new Dictionary<byte, object>();

           Bundle bundle = new Bundle();
           bundle.player = new Player ();
            bundle.player.uniqueId = uniqueID;
           bundle.eventCmd = EventCommand.PlayerDisConnect;
           string json = JsonConvert.SerializeObject(bundle);
            parameter.Add((byte)EventCommand.PlayerDisConnect, json);
            EventData eventData = new EventData((byte)EventCommand.PlayerDisConnect, parameter);

            for (int i = 0; i < lobbyActors.ActorUniqueIDs.Count; i++)
            {
                long _uniqueID = lobbyActors.ActorUniqueIDs[i];
                if (uniqueID == _uniqueID)
                {
                    continue;
                }
                Actor _actor = Actors.GetActor(_uniqueID);
                EZServerPeer peer = Actors.TryGetPeer(_actor.guid);
                // bundle.players.Add(EZServerPeer.getAccountByDict(uniqueID));
                if (peer != null)
                {
                    peer.SendEvent(eventData, new SendParameters());
                }
            }
        }

        public void BroadcastJoinRoom(int RoomIndex, Actor actor)
        {
            Dictionary<byte, object> parameter = new Dictionary<byte, object>();
            parameter.Add((byte)RoomActorActionInfo.ActionNum, 0);
            parameter.Add((byte)RoomActorActionInfo.MemberUniqueID, actor.uniqueID);
            parameter.Add((byte)RoomActorActionInfo.PosX, 100);
            parameter.Add((byte)RoomActorActionInfo.PosY, 100);
            parameter.Add((byte)RoomActorActionInfo.NickName, actor.nickname);
            var eventData = new EventData((byte)EZProtocol.EventCommand.JoinRoomNotify, parameter);
            for (int i = 0; i < Rooms.RoomList[RoomIndex].ActorList.Count; i++)
            {
                // get peer
                
                Actor _actor = Actors.GetActor(Rooms.RoomList[RoomIndex].ActorList[i].uniqueID);
                if (actor.uniqueID == _actor.uniqueID)
                {
                    //continue;
                }
                EZServerPeer peer = Actors.TryGetPeer(_actor.guid);

                if (peer != null)
                {
                    peer.SendEvent(eventData, new SendParameters());
                }
            }
        }
    }
}