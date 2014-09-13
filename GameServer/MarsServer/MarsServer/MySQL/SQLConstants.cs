using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class SQLConstants
    {
        public const string MySQL_CONNECTION = "server=localhost;user id=root;Password='';database=gameserver;persist security info=False";

        /*Accout*/
        public const string MySQL_INSERTINTO_ACCOUNT = "insert into account(uniqueid,id,pw,creatTime)";
        public const string MySQL_INSERTINTO_ACCOUNT_VALUE = " values('{0}','{1}','{2}','{3}')";
        public const string MySQL_CHECK_ID_ACCOUNT = "select * from account where id='{0}'";

        /*Server List*/
        public const string MySQL_SERVER_REGION_LIST = "select * from serverregion";
        public const string MySQL_SERVER_LIST = "select * from serverlist_{0}";

        /*Role*/
        public const string MySQL_ROLE_LIST = "select * from role";
        // follow is older
        public const string MySQL_INSERTINTO_ROLE = "insert into role(roleid,accountid,rolename, sex, exp, pro,level,time)";
        public const string MySQL_INSERTINTO_ROLE_VALUE = " values('{0}','{1}','{2}','{3}', '{4}','{5}', '{6}', '{7}')";
        public const string MySQL_CHECK_ROLE_NAME = "select * from role where rolename='{0}'";
        public const string MySQL_CHECK_ACCOUNT_ID_ROLE = "select * from role where accountid='{0}'";

        /*property*/
        public const string MySQL_PROPERTY_LIST = "select * from property";
        /*exp*/
        public const string MySQL_EXP_LIST = "select * from exp";
        /*LV_INFO*/
        public const string MySQL_LV_INF_LIST = "select * from LV_INFO";
        /*GameItem*/
        public const string MySQL_GAME_ITEM_LIST = "select * from GameItem";
    }
}
