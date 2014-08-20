using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LoginServer
{
    public class SqliteSQL : SQLCommon<int, SQLiteVer>
    {
        public static readonly SqliteSQL instance = new SqliteSQL();

        public override string GetTableName()
        {
            return "select * from Files";
        }

        public override void RefreshData(DataRow row)
        {
            SQLiteVer sqliteVer = new SQLiteVer();
            sqliteVer.ver = int.Parse (row[0].ToString ());
            sqliteVer.url = row[1].ToString ();
            datas.Add(0, sqliteVer);
        }
    }
}
