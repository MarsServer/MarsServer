using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class FightOperator : Game
    {
        public FightOperator(MarsPeer peer)
            : base(peer)
        { }

        public override void ExecuteOperation(Command cmd, params object[] objs)
        {
            base.ExecuteOperation(cmd, objs);

            Fight fight = (Fight)objs[0];
            switch (cmd)
            {
                case Command.EnterFight:
                    HandleEnterFight(fight);
                    marsPeer.SendClientResponse(bundle);
                    break;
            }
        }

        private void HandleEnterFight(Fight fight)
        {
            bundle.fight = fight;

           //if you have team, find all team's peer
            if (marsPeer.team != null)
            {
                bundle.onlineRoles = new List<Role>();
                foreach (MarsPeer for_p in marsPeer.team.peers)
                {
                    if (for_p.region == marsPeer.region)
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
                mRole.roleId = marsPeer.Role.roleId;
                mRole.roleName = marsPeer.Role.roleName;
                mRole.profession = marsPeer.Role.profession;
                Bundle mBundle = new Bundle();
                mBundle.cmd = Command.PlayerAdd;
                mBundle.role = mRole;
                RoomInstance.instance.BroadcastEvent(marsPeer, mBundle, Room.BroadcastType.Region);
            }
        }
    }
}
