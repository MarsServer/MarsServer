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
             insert_sql.AppendFormat(SQLConstants.MySQL_INSERTINTO_ROLE_VALUE, r.roleId, r.accountId, r.roleName, r.sex, r.profession, "1", DateTime.Now.ToString());
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

         public List<Role> GetDataList (long value)
         {
             List<Role> list = new List<Role>();
             StringBuilder sb_sql = new StringBuilder();
             DataTable dt = null;
             sb_sql.AppendFormat(SQLConstants.MySQL_CHECK_ACCOUNT_ID_ROLE, value.ToString());
             dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
             if (dt.Rows.Count == 0)
             {
                 return null;
             }
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 Role r = new Role();
                 r.roleId = long.Parse (dt.Rows[i][0].ToString());
                 r.accountId = long.Parse(dt.Rows[i][1].ToString());
                 r.roleName = dt.Rows[i][2].ToString();
                 r.sex = int.Parse(dt.Rows[i][3].ToString());
                 r.profession = dt.Rows[i][4].ToString();
                 r.level = int.Parse(dt.Rows[i][5].ToString());
                 list.Add(r);
             }
             return list;
         }
    }
}
