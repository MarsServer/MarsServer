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

         /*public string CreatRole(Role r)
         {
             bool isNameExist = CheckExist(SQLConstants.MySQL_CHECK_ROLE_NAME, r.roleName);
             if (isNameExist)
             {
                 return NetError.ROLR_CREAT_ERROR;
             }
             StringBuilder insert_sql = new StringBuilder();
             insert_sql.Append(SQLConstants.MySQL_INSERTINTO_ROLE);
             insert_sql.AppendFormat(SQLConstants.MySQL_INSERTINTO_ROLE_VALUE, r.roleId, r.accountId, r.roleName, r.sex, 0, r.profession, "1", DateTime.Now.ToString());
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
                 r.exp = int.Parse(dt.Rows[i][4].ToString());
                 r.profession = dt.Rows[i][5].ToString();
                 r.level = int.Parse(dt.Rows[i][6].ToString());
                 list.Add(r);
             }
             return list;
         }*/

         //Follow is new
         private Dictionary<long, Role> allRolesByRoId = new Dictionary<long, Role>();
         private Dictionary<string, Role> allRolesByRoName = new Dictionary<string, Role>();
         private Dictionary<long, List<Role>> allRolesList = new Dictionary<long,List<Role>> ();
         private long maxRoleId = 1000000;
         public void Init()
         {
             StringBuilder sb_sql = new StringBuilder();
             DataTable dt = null;
             sb_sql.AppendFormat(SQLConstants.MySQL_ROLE_LIST);
             dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
             if (dt.Rows.Count == 0)
             {
                 return;
             }
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 Role role = new Role();
                 role.roleId = long.Parse(dt.Rows[i][0].ToString());
                 role.accountId = long.Parse(dt.Rows[i][1].ToString());
                 role.roleName = dt.Rows[i][2].ToString();
                 role.sex = int.Parse(dt.Rows[i][3].ToString());
                 role.exp = int.Parse(dt.Rows[i][4].ToString());
                 role.profession = dt.Rows[i][5].ToString();
                 role.level = int.Parse(dt.Rows[i][6].ToString());
                 AddRole(role);
                 //Debug.Log(role.roleName + "____" + role.accountId);
                 if (role.roleId > maxRoleId)
                 {
                     maxRoleId = role.roleId;
                 }
             }
         }


         void AddRole(Role role)
         {
             if (allRolesList.ContainsKey(role.accountId) == false)
             {
                 allRolesList[role.accountId] = new List<Role>();
             }
             allRolesList[role.accountId].Add(role);
             allRolesByRoId.Add(role.roleId, role);
             allRolesByRoName.Add(role.roleName, role);
         }

         public string CreatRole(Role r)
         {
             Role role;
             bool isNameExist = allRolesByRoName.TryGetValue(r.roleName, out role);
             if (isNameExist)
             {
                 return NetError.ROLR_CREAT_ERROR;
             }
             role = r;
             role.exp = 1;
             role.level = 1;
             maxRoleId++;
             role.roleId = maxRoleId;
             StringBuilder insert_sql = new StringBuilder();
             insert_sql.Append(SQLConstants.MySQL_INSERTINTO_ROLE);
             insert_sql.AppendFormat(SQLConstants.MySQL_INSERTINTO_ROLE_VALUE, r.roleId, r.accountId, r.roleName, r.sex, 1, r.profession, "1", DateTime.Now.ToString());
             DBUtility.RunSQL(insert_sql.ToString());
             
             AddRole(role);
             return maxRoleId.ToString ();//GetQueryValue(SQLConstants.MySQL_CHECK_ROLE_NAME, r.roleName, "roleid");
         }

         public List<Role> GetDataListByAccountId(long accountId)
         {
             List<Role> roles = null;
             allRolesList.TryGetValue(accountId, out roles);

             List<Role> m_Roles = new List<Role>();
             foreach (Role r in roles)
             {
                 Role for_r = new Role();
                 for_r.roleId = r.roleId;
                 for_r.profession = r.profession;
                 for_r.roleName = r.roleName;
                 for_r.level = r.level;
                 m_Roles.Add(for_r);
             }
             return m_Roles;
         }

         public Role getRoleByRoleId(long roleId)
         {
             Role role = null;
             allRolesByRoId.TryGetValue(roleId, out role);
             return role;
         }
    }
}
