using System;

namespace EZServer
{
    public class Actor
    {
        public Actor(Guid _Guid, long UniqueID)
        {
            this.guid = _Guid;
            this.uniqueID = UniqueID;
            /*this.memberID = MemberID;
            this.nickname = NickName;
            this.sex = Sex;*/

            roomIndex = -1; // 預設值設為-1，代表不在任何房間中
        }

        public Guid guid { get; set; }          // Peer 列表的guid
        public long uniqueID { get; set; }       // 會員編號
        public string memberID { get; set; }    // 會員帳號
        public string nickname { get; set; }    // 會員暱稱
        public short sex { get; set; }          // 會員性別

        public DateTime LoginTime { get; set; } // 登入時間
        public short status { get; set; }       // 狀態 1:上線中 2:遊戲中
        public short roomIndex { get; set; }    // 在哪個房間，只有status是2時才有效

    }

}
