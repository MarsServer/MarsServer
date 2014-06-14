using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZServer
{
    public class SQLConstants
    {
        public const string MySQL_CONNECTION = "server=localhost;user id=root;Password='';database=users;persist security info=False";

        public const string MySQL_INSERTINTO_ACCOUNT = "insert into account(uniqueid,id,pw,name,creatTime)";
        public const string MySQL_INSERTINTO_ACCOUNT_VALUE = " values('{0}','{1}','{2}','{3}','{4}')";
        public const string MySQL_CHECK_ID_ACCOUNT = "select * from account where id='{0}'";
        public const string MySQL_CHECK_ROLENAME_ACCOUNT = "select * from account where name='{0}'";
    }
}
