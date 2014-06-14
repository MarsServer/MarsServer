using System.Collections;
using System.ComponentModel;
public class RoomInfo 
{
	[DefaultValue (0)]
	public int id;
	[DefaultValue (null)]
	public string RoomName;
	[DefaultValue (null)]
	public int Limit;
	[DefaultValue (null)]
	public int ActorCount;
}
