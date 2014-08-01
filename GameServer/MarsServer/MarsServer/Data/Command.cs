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

    EnterFight,

    PlayerDone,

    MonsterStateUpdate,

    NetError,
    AbortDiscount,
}

public enum EventCommand
{
    
}