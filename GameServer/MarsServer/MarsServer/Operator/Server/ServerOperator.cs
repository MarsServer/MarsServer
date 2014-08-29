using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class ServerOperator : Game
    {
        public ServerOperator (MarsPeer peer) : base (peer)
        {
        }

        public override void ExecuteOperation(Command cmd, params object[] objs)
        {
            base.ExecuteOperation(cmd, objs);

            long accountId = (long)objs[0];
            bool isLogined = Actor.Instance.HandleAccountLogin(accountId, marsPeer);
            if (isLogined == false)
            {
                bundle.error = new Error();
                bundle.error.message = NetError.SAME_ACCOUNT_DISCOUNT_ERROR;
            }
            else
            {
                bundle.roles = RoleMySQL.instance.GetDataListByAccountId(accountId);
            }
            this.marsPeer.SendClientResponse(bundle);
        }
    }
}
