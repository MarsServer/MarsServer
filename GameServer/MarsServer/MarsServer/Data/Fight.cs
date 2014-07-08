using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class Fight//FB 
{
    [DefaultValue(0L)]
    public long id;
    [DefaultValue(null)]
    public string type;
    [DefaultValue(null)]
    public string desc;
    [DefaultValue(null)]
    public string icon;
    [DefaultValue(null)]
    public string name;
    [DefaultValue(null)]
    public string model;
    [DefaultValue(false)]
    public bool isShow;
    [DefaultValue(0)]
    public int limit;
    [DefaultValue(null)]
    public string difficulty;
    [DefaultValue(0)]
    public string level;
    [DefaultValue(null)]
    public Team team;

}
