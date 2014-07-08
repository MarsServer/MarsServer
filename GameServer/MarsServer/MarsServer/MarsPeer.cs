﻿using System;
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
        public TeamInfo teamInfo;


        public MarsPeer(IRpcProtocol rpc, IPhotonPeer peer)
            : base(rpc, peer)
        {
            initialization();
            LinkServerSuccess();
        }

        void initialization()
        {
            peerGuid = Guid.NewGuid();
        }

        private void LinkServerSuccess()
        {
            Bundle bundle = new Bundle();
            bundle.cmd = Command.LinkServer;
            SendToClient(bundle);
        }

        public void StopLinked()
        {
            FightManager.instance.DismissTeam(role);
            DestoryFromRoom();
            PlayersManager.instance.RemoveUser(this);            
            Debug.Log("Client has Diconnected" + accountId + "____" + PlayersManager.instance.size);
            this.Dispose();
        }

        private void DestoryFromRoom()
        {
            if (role != null)
            {
                Bundle bundle = new Bundle();
                bundle.cmd = Command.DestroyPlayer;
                bundle.role = role;
                List<MarsPeer> peers = PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                {
                    if (role != null && role.region == 0)
                    {
                        return true;
                    }
                    return false;
                });
                BroadCastEvent(peers, bundle);
            }
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
                PropertyValue pv = Property.instance.GetValueByK(role.profession);
                if (pv != null)
                {
                    role.strength = pv.strength + (pv.strength / 10) * (role.level - 1);
                    role.agility = pv.agility + (pv.agility / 10) * (role.level - 1);
                    role.stamina = pv.stamina + (pv.stamina / 10) * (role.level - 1);
                    role.wit = pv.wit + (pv.wit / 10) * (role.level - 1);
                }
                role.expMax = ExpMySQL.instance.GetValueByK(role.level + 1);
                bundle = new Bundle();
                bundle.cmd = Command.EnterGame;
                bundle.role = role;
                bundle.role.region = 0;

                Bundle newbundle = new Bundle();
                newbundle.cmd = Command.AddNewPlayer;
                newbundle.role = role;
                newbundle.role.region = 0;
                List<MarsPeer> peers = PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                {
                    if (role != null && role.region == 0)
                    {
                        //
                        //peer.SendToClient(newbundle);
                        return true;
                    }
                    return false;
                });
                BroadCastEvent(peers, newbundle);
                bundle.onlineRoles = PlayersManager.instance.GetAllListRoleBeSideMe(accountId);
                
            }
            else if (cmd == (byte)Command.SendChat)
            {
                Message message = JsonConvert.DeserializeObject<Message>(getJson);
                bundle = new Bundle();
                bundle.cmd = Command.SendChat;
                bundle.message = message;
                List<MarsPeer> peers = null;
                if (message.chatType == ChatType.World)
                {
                    peers = PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                    {
                        return true;
                        //peer.SendToClient(bundle);
                    });
                }
                else if (message.chatType == ChatType.Team)
                {
                    peers = FightManager.instance.GetListBesideMe(teamInfo, role);
                }
                if (peers != null)
                {
                    BroadCastEvent(peers, bundle);
                }
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
                List<MarsPeer> peers = PlayersManager.instance.BroastPlayerSomething(accountId, (MarsPeer peer) =>
                {
                    if (role != null && role.region == 0)
                    {
                        return true;
                        //peer.SendToClient(bundle);
                    }
                    return false;
                });
                BroadCastEvent(peers, bundle);
                return;
            }
            #region About Team
            else if (cmd == (byte) Command.CreatTeam)
            {
                bundle = new Bundle();
                bundle.cmd = Command.CreatTeam;
                TeamInfo info =  FightManager.instance.CreatTeam(this);
                if (info != null)
                {
                    bundle.team = info.team;
                }
                else
                {
                    bundle.error = new Error();
                    bundle.error.message = NetError.CREAT_TEAM_FIALURE;
                }

            }
            else if (cmd == (byte)Command.JoinTeam)
            {
                Role r = JsonConvert.DeserializeObject<Role>(getJson);
                bundle = new Bundle();
                bundle.cmd = Command.JoinTeam;
                TeamInfo info = FightManager.instance.AddTeamMember(r.roleId, this);
                if (info != null)
                {
                    teamInfo = info;
                    bundle.team = info.team;
                    BroadCastEvent(info.peers, bundle);
                    return;
                }
            }
            else if (cmd == (byte)Command.LeftTeam)
            {
                Role r = JsonConvert.DeserializeObject<Role>(getJson);
                bundle.cmd = Command.LeftTeam;
                bundle.role = FightManager.instance.RemoveTeamMember(r.roleId, this);
            }
            else if (cmd == (byte)Command.SwapTeamLeader)
            {
            }
            else if (cmd == (byte)Command.DismissTeam)
            {
            }
            #endregion
            else if (cmd == (byte)Command.EnterFight)
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

        public void BroadCastEvent(List<MarsPeer> peers, Bundle bundle)
        {
            if (peers.Count > 0 && bundle != null)
            {
                byte cmd = (byte)bundle.cmd;
                string json = JsonConvert.SerializeObject(bundle);
                Dictionary<byte, object> paramter = new Dictionary<byte, object>();
                paramter.Add(cmd, json);
                EventData eventData = new EventData(cmd, paramter);
                ApplicationBase.Instance.BroadCastEvent<MarsPeer>(eventData, peers, new SendParameters());
            }
        }
    }
}
