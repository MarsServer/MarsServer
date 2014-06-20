using System.Collections;
using System.ComponentModel;

public class Server
{
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
    [DefaultValue (false)]
    public bool isSwitch;

    public Server() {   }
}
