using ExitGames.Concurrency.Fibers;
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

        private readonly IFiber fiber;

        public long accountId { get; private set; }
        public long roleId { get; private set; }

        private Role role;
        public Role Role 
        {
            get
            { 
                if (roleId == 0)
                    return null; 
                if (role == null)
                    role = RoleMySQL.instance.getRoleByRoleId(roleId);
                return role; 
            }
        }

        /// <summary>
        /// region is that player now in which region. 0-publiczone, roomId-other
        /// </summary>
        public long region { get; private set; }
        public float x { get; private set; }
        public float z { get; private set; }
        public float xRo { get; private set; }
        public float zRo { get; private set; }
        public int acion { get; private set; }

        /// <summary>
        /// it's very import,is Game room
        /// </summary>
        public Team team { get; private set; }

        /// <summary>
        /// it's very import, is Game Fight
        /// </summary>
        public Fight fight { get; private set; }
        public FightCache fightCache { get; private set; }
        #endregion

        #region constructor & HandshakeHandle & ClearData
        public MarsPeer(IRpcProtocol rpc, IPhotonPeer peer)
            : base(rpc, peer)
        {
            this.fiber = new PoolFiber();
            this.fiber.Start();

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
             team = null;
             fight = null;
         }

        #endregion

        #region Photon API
        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            Debug.Log("1111111111111111111111111: " + string.Format("OnDisconnect: conId={0}, reason={1}, reasonDetail={2}, playerscount = {3}, teamscount = {4}", ConnectionId, reasonCode, reasonDetail, Actor.Instance.Size, RoomInstance.instance.Size));
            HandleDisconnectOperation();
            Debug.Log("2222222222222222222222222: " + string.Format("OnDisconnect: conId={0}, reason={1}, reasonDetail={2}, playerscount = {3}, teamscount = {4}", ConnectionId, reasonCode, reasonDetail, Actor.Instance.Size, RoomInstance.instance.Size));
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
                    HandleSendChatOnOperation(json, cmd);
                    return;
                case Command.JoinTeam:
                    bundle = HandleJoinTeamOnOperation(json, cmd);
                    break;
                case Command.LeaveTeam:
                    HandleLeaveTeamOnOperation(json, cmd);
                    return;
                case Command.EnterFight:
                    bundle = HandleEnterFightOnOperation(json, cmd);
                    break;
                case Command.TeamUpdate:
                    HandleTeamUpdateOnOperation(json, cmd);
                    return;
                case Command.MonsterRefresh:
                    bundle = HandleMonsterRefreshOnOperation(json, cmd);
                    break;
                case Command.MonsterStateUpdate:
                    bundle = HandleMonsterStateUpdateOnOperation(json, cmd);
                    break;
            }

            if (bundle != null)
            {
                bundle.cmd = cmd;
                SendClientEvent(bundle);
            }
        }
        #endregion

        #region CommonMethod
        protected void RemoveRoomFromPublicZone()
        {
            if (Role != null)
            {
                //get all online peers, in same region
                List<MarsPeer> peers = Actor.Instance.HandleAccountListOnlineBySamePos(this);/*.HandleAccountListOnline((MarsPeer peer) =>
                {
                    return peer.accountId == accountId || peer.region != region;////account is self, or not in same pos, don't add list Peer
                });*/
                if (peers.Count > 0)
                {
                    Bundle bundle = new Bundle();
                    bundle.cmd = Command.DestroyPlayer;
                    bundle.role = new Role();
                    bundle.role.roleId = roleId;
                    BroadCastEvent(peers, bundle);
                }
            }
        }

        protected void UpdateRoleState(Role mRole)
        {
            x = mRole.x;
            z = mRole.z;
            xRo = mRole.xRo;
            zRo = mRole.zRo;
            acion = mRole.action;
        }
        #endregion

        #region HandleServerSelectOnOperation
        Bundle HandleServerSelectOnOperation(string json, Command cmd)
        {
            Server server = JsonConvert.DeserializeObject<Server>(json);
            Bundle bundle = new Bundle();

            region = Constants.SelectRole;

            accountId = server.accountId;

            bool isLogined = Actor.Instance.HandleAccountLogin(accountId, this);
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
            Role newRole = new Role();//this.Role;//RoleMySQL.instance.getRoleByRoleId(roleId);
            if (newRole != null)
            {
                newRole.accountId = this.Role.accountId;
                newRole.roleId = this.Role.roleId;
                newRole.roleName = this.Role.roleName;
                newRole.sex = this.Role.sex;
                newRole.exp = this.Role.exp;
                newRole.profession = this.Role.profession;
                newRole.level = this.Role.level;

                PropertyValue pv = Property.instance.GetValueByK(newRole.profession);
                newRole.strength = pv.strength;
                newRole.stamina = pv.stamina;
                newRole.wit = pv.wit;
                newRole.agility = pv.agility;
                newRole.critical = pv.critical;
                
               

                region = Constants.PUBLICZONE;
                bundle.onlineRoles = Actor.Instance.HandleRoleListOnlineBySamePos (this);
                bundle.role = newRole;
            }
            //get all online peers, in public region
            List<MarsPeer> peers = Actor.Instance.HandleAccountListOnlineBySamePos(this);
            //new Role
            if (peers.Count >= 0)
            {
                Role mRole = new Role();
                mRole.roleId = newRole.roleId;
                mRole.roleName = newRole.roleName;
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
            UpdateRoleState(mRole);
            
            //get all online peers, in public region
            List<MarsPeer> peers = Actor.Instance.HandleAccountListOnlineBySamePos(this);/*.HandleAccountListOnline((MarsPeer peer) =>
            {
                return peer.accountId == accountId || peer.region != region;//account is self, or not in same pos, don't add list Peer
            });*/
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
        void HandleSendChatOnOperation(string json, Command cmd)
        {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            Bundle bundle = new Bundle();
            //boastcast message
            bundle.cmd = Command.SendChat;
            bundle.message = message;
            MessageManager.instance.OnOperationRequest(bundle, this);

        }
        #endregion

        #region HandleDisconnectOperation
        void HandleDisconnectOperation()
        {
            if (Role != null)
            {
                RemoveRoomFromPublicZone();
                if (team != null)
                {
                    RoomInstance.instance.LeaveTeamRole(this);
                }
                FightInstance.instance.Remove(fightCache);
            }
            Actor.Instance.HandleDisconnect(this);
            ClearData();
        }
        #endregion

        #region HandleJoinTeamOperation
        Bundle HandleJoinTeamOnOperation(string json, Command cmd)
        {
            Team g_team = JsonConvert.DeserializeObject<Team>(json);
            Bundle bundle = new Bundle();

            //Team id.......
            Team m_team = RoomInstance.instance.GetTeamById(g_team.teamId, g_team.teamName, this);
            this.team = m_team;

            Team s_team = new Team();
            s_team.teamId = team.teamId;
            s_team.teamName = team.teamName;
            s_team.roles = new List<Role>();
            foreach (MarsPeer for_p in team.peers)
            {
                Role s_role = new Role();
                s_role.roleId = for_p.roleId;
                s_role.roleName = for_p.Role.roleName;
                s_role.profession = for_p.Role.profession;
                s_role.level = for_p.Role.level;
                s_team.roles.Add(s_role);
            }
            bundle.team = s_team;

            bundle.cmd = cmd;
            //send others
            RoomInstance.instance.BroadcastEvent(this, bundle, Room.BroadcastType.Notice);

            //Send self
            return bundle;
        }
        #endregion

        #region HandleLeaveTeamOperation
        void HandleLeaveTeamOnOperation(string json, Command cmd)
        {
            Team g_team = JsonConvert.DeserializeObject<Team>(json);
            Bundle bundle = new Bundle();

            ////Team id.......
            if (team != null)
            {
                Role g_role = RoomInstance.instance.LeaveTeamRole(this);
                Role s_role = new Role();
                s_role.roleId = g_role.roleId;
                bundle.cmd = cmd;
                bundle.role = s_role;

                team = null;
                //send other
                RoomInstance.instance.BroadcastEvent (this, bundle, Room.BroadcastType.Notice);
            }            
        }
        #endregion

        #region HandleEnterFightOperation
        Bundle HandleEnterFightOnOperation(string json, Command cmd)
        {
            RemoveRoomFromPublicZone();


            Fight g_fight = JsonConvert.DeserializeObject<Fight>(json);
            Bundle bundle = new Bundle();
            bundle.fight = g_fight;

            this.fight = g_fight;

            region = fight.id;

            //if you have team, find all team's peer
            if (team != null)
            {
                bundle.cmd = Command.EnterFight;
                bundle.onlineRoles = new List<Role>();
                foreach (MarsPeer for_p in team.peers)
                {
                    if (for_p.region == region)
                    {
                        Role s_role = new Role();
                        s_role.roleId = for_p.roleId;
                        s_role.roleName = for_p.Role.roleName;
                        s_role.profession = for_p.Role.profession;
                        s_role.level = for_p.Role.level;
                        s_role.x = for_p.x;
                        s_role.z = for_p.z;
                        s_role.xRo = for_p.xRo;
                        s_role.zRo = for_p.zRo;
                        bundle.onlineRoles.Add(s_role);
                    }
                }

                //broast all peers, add new role in fight
                Role mRole = new Role();
                mRole.roleId = Role.roleId;
                mRole.roleName = Role.roleName;
                mRole.profession = Role.profession;
                Bundle mBundle = new Bundle();
                mBundle.cmd = Command.PlayerAdd;
                mBundle.role = mRole;
                RoomInstance.instance.BroadcastEvent(this, mBundle, Room.BroadcastType.Region);
            }

            //Handle fight
            return bundle;
        }
        #endregion

        #region HandleTeamUpdateOnOperation
        void HandleTeamUpdateOnOperation(string json, Command cmd)
        {
            if (team != null)
            {
                Role mRole = JsonConvert.DeserializeObject<Role>(json);
                UpdateRoleState(mRole);
                Bundle bundle = new Bundle();
                bundle.role = mRole;
                bundle.cmd = Command.TeamUpdate;
                RoomInstance.instance.BroadcastEvent(this, bundle, Room.BroadcastType.Region);
            }
        }
        #endregion

        #region HandleMonsterRefreshOnOperation
        Bundle HandleMonsterRefreshOnOperation(string json, Command cmd)
        {
            FightRegion g_fr = JsonConvert.DeserializeObject<FightRegion>(json);
            Bundle bundle = new Bundle ();

            //send fight region
            bundle.region = g_fr;

            LvInfo lvInfo = LvInfoSQL.instance.GetValueByK(g_fr.scId);
            if (lvInfo != null)
            {
                //Fight g_Fight = JsonConvert.DeserializeObject<Fight>(lvInfo.scInfoJson);
                bundle.gameMonsters = lvInfo.fight.gameMonsters[g_fr.index];
            }

            Dictionary<string, GameMonster> gmDict = new Dictionary<string, GameMonster>();
            foreach (GameMonster gm in bundle.gameMonsters)
            {
                gmDict.Add(gm.id, gm);
            }
            FightCache cache = FightInstance.instance.GetFightCache(role.roleId.ToString(), gmDict);
            this.fightCache = cache;

            return bundle;
        }
        #endregion

        #region HandleMonsterStateUpdateOnOperation
        Bundle HandleMonsterStateUpdateOnOperation(string json, Command cmd)
        {
            Bundle bundle = new Bundle();
            GameMonster gm = JsonConvert.DeserializeObject<GameMonster>(json);

            GameMonster gameMonster = new GameMonster();
            gameMonster.id = gm.id;
            GameMonster g_gm = fightCache.UpdateHp(gm);
            if (g_gm != null)
            {
                gameMonster.hp = g_gm.hp;
            }
            bundle.gameMonster = gameMonster;

            if (team != null)
            {
                bundle.cmd = cmd;
                RoomInstance.instance.BroadcastEvent(this, bundle, Room.BroadcastType.Region);
            }

            return bundle;
        }
        #endregion

        #region Link Client Event
        public void SendClientEvent(Bundle bundle)
        {
            OperationResponse response = new OperationResponse((byte)bundle.cmd, GetParameter (bundle)) { ReturnCode = 1, DebugMessage = "" };
            //SendOperationResponse(response, new SendParameters());
            this.fiber.Enqueue(()=>SendOperationResponse (response, new SendParameters()));
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
