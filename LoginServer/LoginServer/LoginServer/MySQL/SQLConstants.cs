using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    public class SQLConstants
    {
        public const string MySQL_CONNECTION = "server=localhost;user id=root;Password='';database=loginserver;persist security info=False";

        /*Accout*/
        public const string MySQL_INSERTINTO_ACCOUNT = "insert into account(uniqueid,id,pw,creatTime)";
        public const string MySQL_INSERTINTO_ACCOUNT_VALUE = " values('{0}','{1}','{2}','{3}')";
        public const string MySQL_CHECK_ID_ACCOUNT = "select * from account where id='{0}'";

        /*Server List*/
        public const string MySQL_SERVER_REGION_LIST = "select * from serverregion";
        public const string MySQL_SERVER_LIST = "select * from serverlist_{0}";
    }
}
