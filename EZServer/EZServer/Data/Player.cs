using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Player
{
    [DefaultValue(0L)]
    public long uniqueId;
    [DefaultValue(null)]
    public string roleName;
    [DefaultValue(0)]
    public float x;//xPos
    [DefaultValue(0)]
    public float z;//zPos
    [DefaultValue(0)]
    public float xRo;//xRotation
    [DefaultValue(0)]
    public float zRo;//yRotation
    [DefaultValue(0)]
    public int actionId;//Link Clip
}
