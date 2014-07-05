using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class ExpMySQL
    {
        public readonly static ExpMySQL instance = new ExpMySQL();

        private Dictionary<int, int> exps = new Dictionary<int, int>();

        public void Init()
        {
            StringBuilder sb_sql = new StringBuilder();
            DataTable dt = null;
            sb_sql.AppendFormat(SQLConstants.MySQL_EXP_LIST);
            dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
            if (dt.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                int level = int.Parse(dt.Rows[i][0].ToString());
                int expMax = int.Parse(dt.Rows[i][1].ToString());
                //Debug.Log(level + "___________" + expMax);
                exps.Add(level, expMax);
            }
        }

        public int GetMaxExp(int level)
        {
            int maxExp = 0;
            exps.TryGetValue(level, out maxExp);
            return maxExp;
        }
    }
}
