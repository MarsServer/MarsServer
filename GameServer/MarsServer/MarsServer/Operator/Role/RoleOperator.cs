using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class RoleOperator : Game
    {
        public RoleOperator(MarsPeer peer)
            : base(peer)
        { 
        }

        public override void ExecuteOperation(Command cmd, params object[] objs)
        {
            base.ExecuteOperation(cmd, objs);

            Role role = (Role)objs[0];

            switch (cmd)
            {
                case Command.CreatRole:
                    GetCreatRoleData(role);
                    break;
                case Command.EnterGame:
                    GetEnterGameData(role);
                    break;
                case Command.UpdatePlayer:
                    SetBroadMsg(role);
                    break;
                case Command.TeamUpdate:
                    HandleTeamUpdate(role);
                    break;
            }
            marsPeer.SendClientResponse(bundle);

            bundle = null;
        }

        private void GetCreatRoleData(Role role)
        {
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
        }

        private void GetEnterGameData(Role role)
        {
            Role newRole = new Role();//this.Role;//RoleMySQL.instance.getRoleByRoleId(roleId);
            if (newRole != null)
            {
                newRole.accountId = marsPeer.Role.accountId;
                newRole.roleId = marsPeer.Role.roleId;
                newRole.roleName = marsPeer.Role.roleName;
                newRole.sex = marsPeer.Role.sex;
                newRole.exp = marsPeer.Role.exp;
                newRole.profession = marsPeer.Role.profession;
                newRole.level = marsPeer.Role.level;

                PropertyValue pv = Property.instance.GetValueByK(newRole.profession);
                newRole.strength = pv.strength;
                newRole.stamina = pv.stamina;
                newRole.wit = pv.wit;
                newRole.agility = pv.agility;
                newRole.critical = pv.critical;
                
                bundle.onlineRoles = Actor.Instance.HandleRoleListOnlineBySamePos(this.marsPeer);
                bundle.role = newRole;
            }
            //get all online peers, in public region
            List<MarsPeer> peers = Actor.Instance.HandleAccountListOnlineBySamePos(this.marsPeer);
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
                MarsPeer.BroadCastEvent(peers, mBundle);
            }
        }

        void SetBroadMsg(Role role)
        {
            //get all online peers, in public region
            List<MarsPeer> peers = Actor.Instance.HandleAccountListOnlineBySamePos(this.marsPeer);
            if (peers.Count >= 0)
            {
                Bundle bundle = new Bundle();
                bundle.role = role;
                bundle.cmd = Command.UpdatePlayer;
                MarsPeer.BroadCastEvent(peers, bundle);
            }
        }

        void HandleTeamUpdate(Role role)
        {
            bundle.role = role;
            RoomInstance.instance.BroadcastEvent(marsPeer, bundle, Room.BroadcastType.Region);
        }

    }
}
