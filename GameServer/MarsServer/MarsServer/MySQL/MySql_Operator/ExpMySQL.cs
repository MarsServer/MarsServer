using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class ExpMySQL : SQLCommon<int, int>
    {
        public readonly static ExpMySQL instance = new ExpMySQL();

        private int MaxLevel = 0;

        public override void RefreshData(DataRow row)
        {
            int level = int.Parse(row[0].ToString());
            int expMax = int.Parse(row[1].ToString());

            if (level > MaxLevel)
            {
                MaxLevel = level;
            }
            datas.Add(level, expMax);
        }

        public override string GetTableName()
        {
            return SQLConstants.MySQL_EXP_LIST;
        }

        public int GetNextExp(int nextLevel)
        {
            if (nextLevel < MaxLevel)
            {
                return GetValueByK(nextLevel);
            }
            return 0;
        }

        public void CheckUpgrade(Role role)
        {
            int nextLevel = role.level + 1;
            if (nextLevel <= MaxLevel)
            {
                int expMax = GetNextExp(nextLevel);
                if (role.exp >= expMax)
                {
                    int val = role.exp - expMax;
                    role.level = nextLevel;
                    int nextLeve0 = nextLevel + 1;
                    if (nextLeve0 < MaxLevel)
                    {
                        if (val >= 0)
                        {
                            role.exp = val;
                            CheckUpgrade(role);
                        }
                    }
                    else
                    {
                        role.exp = 0;
                    }
                }
            }
        }
    }
}
