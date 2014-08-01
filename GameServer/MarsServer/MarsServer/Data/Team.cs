using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using MarsServer;

public class Team
{
    [DefaultValue(null)]
    public string teamId;
    [DefaultValue(0L)]
    public long fightId;
    [DefaultValue(null)]
    public string teamName;
    [DefaultValue(null)]
    public List<Role> roles;

    /// <summary>
    /// Server dont send to client
    /// </summary>
    [DefaultValue(null)]
    public List<MarsPeer> peers;
}