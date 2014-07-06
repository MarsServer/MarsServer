using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class Team
{
    [DefaultValue(0L)]
    public long teamId;
    [DefaultValue(null)]
    public List<Role> roles;
}