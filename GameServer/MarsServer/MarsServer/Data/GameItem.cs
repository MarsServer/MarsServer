using System.Collections;
using System.ComponentModel;

public class GameItem : GameBase
{
	[DefaultValue(null)]
	public string func;
	[DefaultValue(null)]
	public string belong;
	[DefaultValue(0)]
	public int cd;
}
