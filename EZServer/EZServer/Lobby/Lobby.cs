using System;
using System.Collections.Generic;

namespace EZServer
{
    public class Lobby
    {
        // 存放大廳中的會員
        protected List<long> actorUIDs;

        public List<long> ActorUniqueIDs
        {
            get { return actorUIDs; }
        }


        public Lobby()
        {
            actorUIDs = new List<long>();
        }

        // 將會員加入大廳
        public void Add( long actorUniqueID )
        {
            actorUIDs.Add(actorUniqueID);
        }

        // 從大廳移除會員
        public void Remove(long actorUniqueID)
        {
            for (int i = actorUIDs.Count - 1; i >= 0; i--)
            {
                if (actorUIDs[i] == actorUniqueID)
                {
                    actorUIDs.RemoveAt(i);
                }

            }
        }

        // 清空所有大廳會員
        public void Clear()
        {
            actorUIDs.Clear();
        }

    }
}
