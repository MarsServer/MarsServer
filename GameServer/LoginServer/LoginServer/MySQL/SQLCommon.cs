using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    public abstract class SQLCommon<K, T>
    {
        protected Dictionary<K, T> datas = new Dictionary<K, T>();
        public void Init ()
        {
            StringBuilder sb_sql = new StringBuilder();
            DataTable dt = null;
            sb_sql.AppendFormat(GetTableName ());
            dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
            if (dt.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RefreshData(dt.Rows[i]);
                /*int level = int.Parse(dt.Rows[i][0].ToString());
                int expMax = int.Parse(dt.Rows[i][1].ToString());
                //Debug.Log(level + "___________" + expMax);
                exps.Add(level, expMax);*/
            }
        }

        public T GetValueByK(K k)
        {//
            T t;
            datas.TryGetValue(k, out t);
            return t;
        }

        public abstract string GetTableName();
        public abstract void RefreshData(DataRow row);

    }
}
