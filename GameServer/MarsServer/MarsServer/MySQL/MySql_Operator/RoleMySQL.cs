using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
     public class RoleMySQL
    {
         public readonly static RoleMySQL instance = new RoleMySQL();

         public string CreatRole(Role r)
         {
             bool isNameExist = CheckExist(SQLConstants.MySQL_CHECK_ROLE_NAME, r.roleName);
             if (isNameExist)
             {
                 return NetError.ROLR_CREAT_ERROR;
             }
             StringBuilder insert_sql = new StringBuilder();
             insert_sql.Append(SQLConstants.MySQL_INSERTINTO_ROLE);
             insert_sql.AppendFormat(SQLConstants.MySQL_INSERTINTO_ROLE_VALUE, r.roleId, r.accountId, r.roleName, r.profession, "1", DateTime.Now.ToString());
             DBUtility.RunSQL(insert_sql.ToString());
             return GetQueryValue(SQLConstants.MySQL_CHECK_ROLE_NAME, r.roleName, "roleid");
         }

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
             return dt.Rows[0][index].ToString();
         }
    }
}
