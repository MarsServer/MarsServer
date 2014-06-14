using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using EZProtocol;
using EZServer;

public class Bundle
{
    [DefaultValue (null)]
    public Command cmd;
    [DefaultValue(null)]
    public EventCommand eventCmd;


    [DefaultValue(null)]
    public SQLiteVer sqliteVer;

	[DefaultValue (false)]
	public bool isRegister;
	[DefaultValue (null)]
	public string error;
	[DefaultValue (null)]
	public string notice;
	

	[DefaultValue (null)]
	public Account account;

    [DefaultValue(null)]
    public Player player;

    [DefaultValue (null)]
    public List<Player> players;

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