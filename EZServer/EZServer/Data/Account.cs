using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Account
{
	[DefaultValue (0L)]
	public long uniqueId;
	[DefaultValue (null)]
	public string id;
	[DefaultValue (null)]
	public string pw;
    [DefaultValue(null)]
    public string roleName;
	[DefaultValue (0L)]
	public long creatTime;
}
