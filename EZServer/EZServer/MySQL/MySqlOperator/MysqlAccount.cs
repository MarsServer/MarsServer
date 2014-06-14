using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace EZServer
{
    public class MysqlAccount
    {
        private static MysqlAccount _instance;
        public static MysqlAccount instance { get { if (_instance == null) { _instance = new MysqlAccount(); } return _instance; } }

        #region Register
        public string Register(Account a)
        {
            bool isIdExist = CheckExist(SQLConstants.MySQL_CHECK_ID_ACCOUNT, a.id);
            if (isIdExist == true)
            {
                return NetError.REGISTER_USER_ERROR;
            }
            /*else
            {
                bool isNameExist = CheckExist(SQLConstants.MySQL_CHECK_ROLENAME_ACCOUNT, a.roleName);
                if (isNameExist == true)
                {
                    return NetError.REGISTER_NAME_ERROR;
                }
            }*/
            StringBuilder insert_sql = new StringBuilder();
            insert_sql.Append(SQLConstants.MySQL_INSERTINTO_ACCOUNT);
            insert_sql.AppendFormat(SQLConstants.MySQL_INSERTINTO_ACCOUNT_VALUE, a.uniqueId, a.id, a.pw, a.roleName, DateTime.Now.ToString());
            DBUtility.RunSQL(insert_sql.ToString());
            return GetQueryValue(SQLConstants.MySQL_CHECK_ID_ACCOUNT, a.id, "uniqueid");
        }
        #endregion

        #region login
        public string Login(Account a)
        {
            string pw = GetQueryValue(SQLConstants.MySQL_CHECK_ID_ACCOUNT, a.id, "pw");
            if (a.pw == pw)
            {
                return GetQueryValue(SQLConstants.MySQL_CHECK_ID_ACCOUNT, a.id, "uniqueid");
            }
            else if (pw == null)
            {
                return NetError.LOGIN_USER_ERROR;
            }
            else
            {
                return NetError.LOGIN_PW_ERROR;
            }
        }
        #endregion
        public bool CheckExist(string key, string value)//true to be exist, false to be not exist
        {
            StringBuilder sb_sql = new StringBuilder();
            DataTable dt = null;
            sb_sql.AppendFormat(key, value);
            dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
            return dt.Rows.Count != 0;
        }

        public string GetQueryValue(string key, string value, string index)
        {
            StringBuilder sb_sql = new StringBuilder();
            DataTable dt = null;
            sb_sql.AppendFormat(key, value);
            dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return dt.Rows[0][index].ToString ();
        }
    }
}
