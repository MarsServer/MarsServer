using System;
using System.Collections.Generic;

namespace EZServer
{
    public class Room
    {
        public Room(string roomName, int limit)
        {
            RoomName = roomName;
            Limit = limit;
            actorList = new List<RoomActor>();
        }

        public string RoomName { get; set; }            // 房間名稱
        public int Limit { get; set; }                  // 人數上限
        protected List<RoomActor> actorList;               // 房間中的會員列表
        public List<RoomActor> ActorList { get { return actorList; } }

        // 會員加入房間
        public bool Join(Guid _Guid, int UniqueID, string MemberID, string NickName, short Sex)
        {
            lock (this)
            {
                // 若房間已滿返回false
                if (actorList.Count >= Limit - 1)
                {
                    return false;
                }

                // 若有重複(已在房間中)
                for (int i = 0; i < actorList.Count; i++)
                {
                    if (UniqueID == actorList[i].uniqueID)
                    {
                        return false;
                    }
                }

                actorList.Add(new RoomActor(_Guid, UniqueID, MemberID, NickName, Sex));
                return true;
            }
        }

        // 會員離開房間
        public void Quit(long UniqueID)
        {
            for (int i = actorList.Count-1; i >= 0; i--)
            {
                if (UniqueID == actorList[i].uniqueID)
                {
                    actorList.RemoveAt(i);
                }
            }
        }

        // 清空房間
        public void Clear()
        {
            actorList.Clear();
        }

        // 更新會員行為
        public void UpdateActorAction(int UniqueID, float PosX, float PosY, float PosZ, short Direct, short ActionNum)
        {
            for (int i = 0; i< actorList.Count; i++)
            {
                if (UniqueID == actorList[i].uniqueID)
                {
                    RoomActor actor = new RoomActor(actorList[i].guid, actorList[i].uniqueID, actorList[i].memberID, actorList[i].nickname, actorList[i].sex);
                    actor.PosX = PosX;
                    actor.PosY = PosY;
                    actor.PosZ = PosZ;
                    actor.Direct = Direct;
                    actor.ActionNum = ActionNum;

                    actorList[i] = actor;

                    break;
                }
            }
        }
    }

    // 因為要多記錄加入房間時間的屬性，因此用繼承的方式建立房間用的會員類別
    public class RoomActor : Actor
    {
        public RoomActor(Guid _Guid, long UniqueID, string MemberID, string NickName, short Sex)
            : base(_Guid, UniqueID)
        {
            this.joinTime = System.DateTime.Now;    // 記錄加人此房間的時間

            PosX = 0f;
            PosY = 0f;
            PosZ = 0f;
            Direct = 0;
            ActionNum = 0;
        }

        protected DateTime joinTime; // 多增加進入房間時間的屬性，並且設為唯讀
        public DateTime JoinTime { get { return joinTime; } }

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public short Direct { get; set; }
        public short ActionNum { get; set; }

    }
}
