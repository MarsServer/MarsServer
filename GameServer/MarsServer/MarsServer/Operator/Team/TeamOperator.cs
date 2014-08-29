using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class TeamOperator : Game
    {
        public TeamOperator(MarsPeer peer)
            : base(peer)
        { 
        }

        public override void ExecuteOperation(Command cmd, params object[] objs)
        {
            base.ExecuteOperation(cmd, objs);

            Team team = (Team) objs[0];
            switch (cmd)
            {
                case Command.JoinTeam:
                    HandleJoinTeam(team);
                    this.marsPeer.SendClientResponse(bundle);
                    break;
                case Command.LeaveTeam:
                    HandleLeaveTeam(team);
                    break;
            }
            bundle = null;
        }

        void HandleJoinTeam(Team team)
        {
            //Team id.......
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
            //send others
            RoomInstance.instance.BroadcastEvent(this.marsPeer, bundle, Room.BroadcastType.Notice);
        }

        void HandleLeaveTeam(Team team)
        {
            ////Team id.......
            if (team != null)
            {
                Role g_role = RoomInstance.instance.LeaveTeamRole(this.marsPeer);
                Role s_role = new Role();
                s_role.roleId = g_role.roleId;
                bundle.role = s_role;

                //send other
                RoomInstance.instance.BroadcastEvent(this.marsPeer, bundle, Room.BroadcastType.Notice);

                this.marsPeer.ClearTeam();
            }
        }
    }
}
