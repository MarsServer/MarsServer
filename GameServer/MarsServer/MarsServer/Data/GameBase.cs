using System.Collections;
using System.ComponentModel;

public class GameBase
{
	[DefaultValue(0L)]
	public long id;
	[DefaultValue(null)]
	public string type;
	[DefaultValue(null)]
	public string name;
	[DefaultValue (null)]
	public string desc;
	[DefaultValue (null)]
	public string icon;
	[DefaultValue (null)]
	public string model;
	[DefaultValue(0)]
	public int value;
	[DefaultValue(0)]
	public int gold;
	[DefaultValue(0)]
	public int gem;
	[DefaultValue (0)]
	public int level;
	[DefaultValue (0F)]
	public float hp;
	[DefaultValue (0F)]
	public float hpMax;
	[DefaultValue(0F)]
	public float deductHp;


	public float hpRatio
	{
		get
		{
			return (float)hp / (float)hpMax;
		}
	}

}
