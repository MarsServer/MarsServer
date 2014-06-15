using System.Collections;
using System.ComponentModel;

public class Server
{
    [DefaultValue(0L)]
    public long accountId;
    [DefaultValue(0)]
    public int serverId;
    [DefaultValue(null)]
    public string serverName;
    [DefaultValue(0)]
    public int limit; //0-normal, 1-crowd, 2-busy
    [DefaultValue(null)]
    public string ip;
    [DefaultValue(null)]
    public string belong;

    public Server() {   }
}
