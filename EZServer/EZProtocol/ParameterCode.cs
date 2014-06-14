
namespace EZProtocol
{
    public enum LoginParameterCode
    {
        MemberID = 1,
        MemberPW,
        MemberNa,
        MemberSex = 4,
    }

    public enum LoginResponseCode
    {
        MemberID = 1,
        MemberPW,
        Nickname,
        MemberUniqueID,
        Sex,
        
        PosX,
        PosY,
        PosZ,
        Direct,
        ActionNum,

        MemberSex,
        RegisterState,
        
        Ret = 80,
    }

    public enum GetRoomInfoParameterCode
    {
        RoomIndex = 1,
    }

    public enum GetRoomInfoResponseCode
    {
        RoomIndex = 1,
        RoomName,
        Limit,
        ActorCount,
    }

    public enum JoinRoomParameterCode
    {
        RoomIndex = 1,
    }

    public enum JoinRoomResponseCode
    {
        RoomIndex = 1,
        RoomName,
        Limit,
        ActorCount,
    }

    public enum GetRoomInfoEventCode
    {
        RoomIndex = 1,
        RoomName,
        Limit,
        ActorCount,
    }

    public enum RoomActorActionInfo
    {
        MemberUniqueID = 1,
        NickName,
        PosX,
        PosY,
        PosZ,
        Direct,
        ActionNum,
    }

    public enum RoomActorQuit
    {
        MemberUniqueID = 1,
        MemberUniqueName
    }

    public enum RoomActorSpeak
    {
        MemberUniqueID = 1,
        MemberUniqueName,
        TalkString,
    }
}
