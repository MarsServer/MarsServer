using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public abstract class SQLCommon<K, T>
    {
        protected readonly Dictionary<K, T> datas = new Dictionary<K, T>();
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
