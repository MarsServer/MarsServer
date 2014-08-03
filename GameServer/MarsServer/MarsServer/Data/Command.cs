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


    CreatTeam,
    JoinTeam,
    LeaveTeam,
    SwapTeamLeader,
    DismissTeam,
    TeamUpdate,
    PlayerAdd,

    EnterFight,

    PlayerDone,

    MonsterRefresh,
    MonsterStateUpdate,

    NetError,
    AbortDiscount,
}

public enum EventCommand
{
    
}