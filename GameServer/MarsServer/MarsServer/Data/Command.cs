public enum Command
{
    Handshake,
    Login,
    Register,
    ServerList,

    LinkServer,
    ServerSelect,//linke server and get role list

    CreatRole,

    EnterGame,

    SendChat,

    InitAllPlayers,
    AddNewPlayer,
    UpdatePlayer,
    DestroyPlayer,

    NetError,
    AbortDiscount,
}

public enum EventCommand
{
    
}