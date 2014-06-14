using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZServer
{
    public class RoomCollection
    {
        protected List<Room> roomList;

        public RoomCollection()
        {
            roomList = new List<Room>();

            // 建立聊天室的名字以及容許人數
            roomList.Add(new Room("公開聊天室1", 20));
            roomList.Add(new Room("公開聊天室2", 20));
            roomList.Add(new Room("公開聊天室3", 20));
            roomList.Add(new Room("公開聊天室4", 20));
            roomList.Add(new Room("公開聊天室5", 20));
        }

        public List<Room> RoomList { get { return roomList; } }

        /*public class RoomInfo
        {
            public int id { get; set; }
            public string RoomName { get; set; }
            public int Limit { get; set; }
            public int ActorCount { get; set; }
        }*/

        // 取得房間名稱及會員人數資訊
        public RoomInfo GetRoomInfo(int roomIndex)
        {
            if (roomIndex >= roomList.Count)
            {
                return null;
            }

            RoomInfo roomInfo = new RoomInfo();
            roomInfo.id = roomIndex;
            roomInfo.RoomName = roomList[roomIndex].RoomName;
            roomInfo.Limit = roomList[roomIndex].Limit;
            roomInfo.ActorCount = roomList[roomIndex].ActorList.Count;

            return roomInfo;
        }

        // 取得所有房間名稱及會員人數資訊列表
        public RoomInfo[] GetAllRoomInfo()
        {
            RoomInfo[] roomInfo = new RoomInfo[roomList.Count];

            for (int i = 0; i < roomList.Count; i++)
            {
                roomInfo[i] = new RoomInfo();

                roomInfo[i].id = i;
                roomInfo[i].RoomName = roomList[i].RoomName;
                roomInfo[i].Limit = roomList[i].Limit;
                roomInfo[i].ActorCount = roomList[i].ActorList.Count;
            }

            return roomInfo;
        }
    }
}
