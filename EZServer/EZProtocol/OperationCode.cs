
namespace EZProtocol
{
    public enum Command
    {
        Handshake,
        Login = 5,
        Register,
        GetRoomInfo,
        GetAllRoomInfo,
        JoinRoom,
        QuitRoom,
        RoomActorBorning, // 玩家進入聊天室
        RoomActorActionUpdate, // 玩家更新行為資訊
        RoomSpeak,        // 玩家發言聊天
        UpdatePlayer,
    }

    public enum EventCommand
    {
        LobbyBroadcast = 1,
        RoomBroadcastActorAction,
        RoomBroadcastActorQuit,
        RoomBroadcastActorSpeak,
        JoinRoomNotify,
        InitAllPlayer,
        UpdatePlayer,
        PlayerDisConnect,
    }
}
