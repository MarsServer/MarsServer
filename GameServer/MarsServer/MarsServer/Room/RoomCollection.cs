using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServe
{
    public class RoomCollection : List<Team>
    {

        /// <summary>
        /// get one team by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Team GetTeamById(long id)
        {
            return this.FirstOrDefault(team => team.teamId == id);
        }
    }
}
