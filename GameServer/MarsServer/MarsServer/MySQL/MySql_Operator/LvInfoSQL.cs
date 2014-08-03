using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MarsServer
{
    public class LvInfo
    {
        [DefaultValue(0)]
        public int id;
        [DefaultValue(null)]
        public string scId;
        [DefaultValue(null)]
        public string scInfoJson;

    }

    public class LvInfoSQL : SQLCommon<string, LvInfo>
    {
        public readonly static LvInfoSQL instance = new LvInfoSQL();

        public override string GetTableName()
        {
            return SQLConstants.MySQL_LV_INF_LIST;
        }

        public override void RefreshData(DataRow row)
        {
            LvInfo li = new LvInfo();
            li.id = int.Parse(row[0].ToString());
            li.scId = row[1].ToString();
            li.scInfoJson = row[2].ToString();
            datas.Add(li.scId, li);
        }
    }
}
