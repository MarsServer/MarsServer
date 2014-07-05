using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class PropertyValue
{
    [DefaultValue(0)]
    public int pro;
    [DefaultValue(0)]
    public int strength;
    [DefaultValue(0)]
    public int agility;
    [DefaultValue(0)]
    public int stamina;
    [DefaultValue(0)]
    public int wit;
}

namespace MarsServer
{
    public class Property : SQLCommon<string, PropertyValue>
    {
        public readonly static Property instance = new Property();

        public override void RefreshData(DataRow row)
        {
            PropertyValue pv = new PropertyValue();
            pv.pro = int.Parse(row[0].ToString());
            pv.strength = int.Parse(row[1].ToString());
            pv.agility = int.Parse(row[2].ToString());
            pv.stamina = int.Parse(row[3].ToString());
            pv.wit = int.Parse(row[4].ToString());

            string pro = ((PRO)pv.pro).ToString();
            //Debug.Log("_______________________________" + pro);
            datas.Add(pro, pv);
        }

        public override string GetTableName()
        {
            return SQLConstants.MySQL_PROPERTY_LIST;
        }

        /*private Dictionary<string, PropertyValue> propertys = new Dictionary<string, PropertyValue>();
        public void Init()
        {
            StringBuilder sb_sql = new StringBuilder();
            DataTable dt = null;
            sb_sql.AppendFormat(SQLConstants.MySQL_PROPERTY_LIST);
            dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
            if (dt.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PropertyValue pv = new PropertyValue();
                pv.pro = int.Parse(dt.Rows[i][0].ToString());
                pv.strength = int.Parse(dt.Rows[i][1].ToString());
                pv.agility = int.Parse(dt.Rows[i][2].ToString());
                pv.stamina = int.Parse(dt.Rows[i][3].ToString());
                pv.wit = int.Parse(dt.Rows[i][4].ToString());

                string pro = ((PRO) pv.pro).ToString ();
                //Debug.Log("_______________________________" + pro);
                propertys.Add(pro, pv);
            }
        }

        public PropertyValue GetPv(string pro)
        {
            PropertyValue pv = null;
            propertys.TryGetValue(pro, out pv);
            return pv;
        }*/
    }
}
