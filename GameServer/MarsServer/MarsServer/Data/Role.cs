using System.ComponentModel;
using System.Collections;

public class Role
{
	[DefaultValue (0L)]
	public long roleId;
	[DefaultValue (0L)]
	public long accountId;
	[DefaultValue (null)]
	public string roleName;
	[DefaultValue (null)]
	public string profession;//zs fs
	[DefaultValue (0)]
	public int level;
    [DefaultValue(0)]
    public int sex;//0-male, 1-female

    [DefaultValue (0F)]
    public float x;//xPos
    [DefaultValue(0F)]
    public float z;//yPos
    [DefaultValue(0F)]
    public float xRo;//xRotation
    [DefaultValue(0F)]
    public float zRo;//yRotation

    [DefaultValue(0)]
    public int action;
    [DefaultValue(0)]
    public int region;//0-public, else is roomid;

    [DefaultValue(0)]
    public int exp;
    [DefaultValue(0)]
    public int expMax;

    [DefaultValue(0)]
    public int strength;
    [DefaultValue(0)]
    public int agility;
    [DefaultValue(0)]
    public int stamina;
    [DefaultValue(0)]
    public int wit;


	[DefaultValue (0F)]
	public float speed;
}
