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

    [DefaultValue(0F)]
    public float x;
    [DefaultValue(0F)]
    public float z;
    [DefaultValue(0)]
    public float exp;


	[DefaultValue (0F)]
	public float speed;
}
