using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZServer
{
    public class ActorCollection
    {
        protected Dictionary<Guid, EZServerPeer> ConnectedClients { get; set; } // Peer 列表
        protected Dictionary<Guid, long> GuidUniqueID { get; set; }              // 從Guid取得會編號
       // protected Dictionary<string, int> MemberIDUniqueID { get; set; }        // 從會員帳號取得會員編號
        //protected Dictionary<string, int> NicknameUniqueID { get; set; }        // 從暱稱取得會員編號
        protected Dictionary<long, Actor> UniqueIDActor { get; set; }            // 從會員編號取得會員資料，實際儲存會員資料的列表

        public ActorCollection()
        {
            ConnectedClients = new Dictionary<Guid, EZServerPeer>();
            GuidUniqueID = new Dictionary<Guid, long>();
            //MemberIDUniqueID = new Dictionary<string, int>();
            //NicknameUniqueID = new Dictionary<string, int>();
            UniqueIDActor = new Dictionary<long, Actor>();
        }

        // 會員處理的回傳值
        public class ActorReturn
        {
            public int ReturnCode { get; set; }         // 回傳代碼
            public string DebugMessage { get; set; }    // 回傳說明字串
        }

        // 加入一個Client連線列表
        public void AddConnectedPeer(Guid guid, EZServerPeer peer)
        {
            ConnectedClients.Add(guid, peer);
        }

        // 取得與client連線的peer
        public EZServerPeer TryGetPeer(Guid guid)
        {
            EZServerPeer peer;
            ConnectedClients.TryGetValue(guid, out peer);
            return peer;
        }

        // 移除一筆連線的Peer
        public void RemovePeer(Guid guid)
        {
            if (ConnectedClients.ContainsKey(guid))
            {
                ConnectedClients.Remove(guid);
            }
        }

        // 加入一筆會員資料(成功傳回0，不為0為錯誤碼)
        public ActorReturn ActorOnline(Guid guid, long UniqueID)
        {
            ActorReturn ActorRet = new ActorReturn();
            ActorRet.ReturnCode = -1;

            if (UniqueID <= 0)
            {
                ActorRet.ReturnCode = 2;
                ActorRet.DebugMessage = "UniqueID必須大於0";
                return ActorRet;
            }

            /*if (MemberID.Length <= 0)
            {
                ActorRet.ReturnCode = 2;
                ActorRet.DebugMessage = "MemberID不得為空白";
                return ActorRet;
            }

            if (Nickname.Length <= 0)
            {
                ActorRet.ReturnCode = 2;
                ActorRet.DebugMessage = "Nickname不得為空白";
                return ActorRet;
            }*/

            lock (this)
            {
                // 檢查GuidUniqueID的索引確保沒有重複登入
                if (GuidUniqueID.ContainsKey(guid))
                {
                    ActorRet.ReturnCode = 3;
                    ActorRet.DebugMessage = "重複登入";
                    return ActorRet;                            // 不允許重複登入
                }
                else
                {
                    GuidUniqueID.Add(guid, UniqueID);           // 加入Guid索引會員編號的列表


                    Actor actor = new Actor(guid, UniqueID); // 加入會員列表
                    actor.LoginTime = System.DateTime.Now;
                    actor.status = 1;                           // 代表上線中
                    UniqueIDActor.Add(UniqueID, actor);

                    /*if (!MemberIDUniqueID.ContainsKey(MemberID)) // 若會員帳號索引會員編號列表沒有資料，加入索引
                    {
                        MemberIDUniqueID.Add(MemberID, UniqueID);
                    }

                    if (!NicknameUniqueID.ContainsKey(Nickname)) // 若會員帳號暱稱索引會員編號列表沒有資料，加入索引
                    {
                        NicknameUniqueID.Add(Nickname, UniqueID);
                    }*/

                    ActorRet.ReturnCode = 1;        // 加入會員資料成功
                    ActorRet.DebugMessage = "";
                }

            }
            return ActorRet;
        }


        // 以會員編號取得會員資料
        public Actor GetActor(long UniqueID)
        {
            Actor actor;
            UniqueIDActor.TryGetValue(UniqueID, out actor);
            return actor;
        }

        // 以會員帳號取得會員資料
        public Actor GetActorFromMemberID(string MemberID)
        {
            /*if (!MemberIDUniqueID.ContainsKey(MemberID))
            {
                return null;
            }
            else
            {
                return GetActor(MemberIDUniqueID[MemberID]);
            }*/
            return null;
        }


        // 以會員暱稱取得會員資料
        public Actor GetActorFromNickName(String NickName)
        {
           /* if (!NicknameUniqueID.ContainsKey(NickName))
            {
                return null;
            }
            else
            {
                return GetActor(NicknameUniqueID[NickName]);
            }*/
            return null;
        }


        // 登出一筆會員資料，會順便移除Peer
        public void ActorOffline(Guid guid)
        {

            lock (this)
            {
                RemovePeer(guid); // 移除Peer

                long uniqueID = 0;
                if (GuidUniqueID.ContainsKey(guid)) // 若有資料
                {
                    uniqueID = GuidUniqueID[guid];
                    GuidUniqueID.Remove(guid);      // 移除guid列表資料

                    if (UniqueIDActor.ContainsKey(uniqueID))                // 若會員列表有資料
                    {
                        Actor actor = GetActor(uniqueID);                   // 先取得會員資料

                        /*if (MemberIDUniqueID.ContainsKey(actor.memberID))   // 移除會員帳號索引列表資料
                        {
                            MemberIDUniqueID.Remove(actor.memberID);
                        }

                        if (NicknameUniqueID.ContainsKey(actor.nickname))   // 移除會員暱稱索引列表資料
                        {
                            NicknameUniqueID.Remove(actor.nickname);
                        }*/
                        UniqueIDActor.Remove(uniqueID);                     // 移除會員列表資料
                    }
                }

            }

        }


        // 以Guid取得會員資料
        public Actor GetActorFromGuid(Guid guid)
        {
            if (!GuidUniqueID.ContainsKey(guid))
            {
                return null;
            }
            else
            {
                return GetActor(GuidUniqueID[guid]);
            }
        }


        // 更新一筆會員
        private void UpdateActor(Actor actor)
        {
            if (UniqueIDActor.ContainsKey(actor.uniqueID))
            {
                UniqueIDActor[actor.uniqueID] = actor;
            }
        }


        // 設置會員狀態
        public void SetActorStatus(int UniqueID, short Status, short RoomIndex)
        {
            Actor actor = GetActor(UniqueID);
            if (actor != null)
            {
                actor.status = Status;
                actor.roomIndex = RoomIndex;
                UpdateActor(actor);
            }
        }

    }

}
