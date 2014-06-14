﻿using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using LoginServer;
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