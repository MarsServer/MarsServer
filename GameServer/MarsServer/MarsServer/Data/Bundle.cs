using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using MarsServer;

public enum PRO
{
    NULL = 0,
    ZS = 100001,
    FS = 100002,
    DZ = 100003
}

public class Bundle
{
    [DefaultValue (null)]
    public Command cmd;


    [DefaultValue(null)]
    public SQLiteVer sqliteVer;

    [DefaultValue(null)]
    public Dictionary<string, Server[]> serverList;

	[DefaultValue (null)]
	public Error error;

	[DefaultValue (null)]
	public Account account;

    [DefaultValue(null)]
    public Server server;

    [DefaultValue(null)]
    public List<Role> roles;

    [DefaultValue(null)]
    public Role role;

    [DefaultValue(null)]
    public List<Role> onlineRoles;

    [DefaultValue(null)]
    public Message message;

    [DefaultValue(null)]
    public Fight fight;

    public Bundle() {  }

	/*[DefaultValue (null)]
	public User user;
	[DefaultValue (null)]
	public Room room;
	[DefaultValue (null)]
	public List<Room> rooms;
	[DefaultValue (null)]
	public Message mesaage;
	[DefaultValue (null)]
	public RoomMember roomMember;*/
}