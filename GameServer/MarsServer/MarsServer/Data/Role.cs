using System.ComponentModel;
using System.Collections;

public class Role
{
	[DefaultValue (null)]
	public string roleId;
	[DefaultValue (0L)]
	public long accountId;
	[DefaultValue (null)]
	public string roleName;
	[DefaultValue (null)]
	public string profession;//zs fs
	[DefaultValue (0)]
	public int level;


	[DefaultValue (0F)]
	public float speed;
}
