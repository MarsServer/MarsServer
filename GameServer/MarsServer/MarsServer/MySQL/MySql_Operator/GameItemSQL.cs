using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class GameItemSQL : SQLCommon<long, GameItem>
    {
        public readonly static GameItemSQL instance = new GameItemSQL();

        public override void RefreshData(System.Data.DataRow row)
        {
            GameItem gameItem = new GameItem();
            gameItem.id = long.Parse(row[0].ToString());
            gameItem.type = row[1].ToString();
            gameItem.cd = int.Parse (row[2].ToString());
            gameItem.value = int.Parse (row[3].ToString());
            gameItem.gold = int.Parse (row[4].ToString());
            gameItem.gem = int.Parse (row[5].ToString());
            datas.Add(gameItem.id, gameItem);
        }

        public override string GetTableName()
        {
            return SQLConstants.MySQL_GAME_ITEM_LIST;
        }
    }
}
