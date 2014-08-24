using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


public class GameMonster
{
    [DefaultValue(null)]
    public string id;
    [DefaultValue(null)]
    public string type;
    [DefaultValue(null)]
    public string name;
    [DefaultValue(null)]
    public string desc;
    [DefaultValue(null)]
    public string icon;
    [DefaultValue(null)]
    public string model;
    [DefaultValue(0)]
    public int value;
    [DefaultValue(0)]
    public int gold;
    [DefaultValue(0)]
    public int gem;
    [DefaultValue(0)]
    public int level;
    [DefaultValue(0)]
    public float hp;
    [DefaultValue(0)]
    public float hpMax;

    [DefaultValue(0F)]
    public float x;
    [DefaultValue(0F)]
    public float z;
    [DefaultValue(0F)]
    public float xRo;
    [DefaultValue(0F)]
    public float zRo;
    [DefaultValue(0F)]
    public float action;
    [DefaultValue(false)]
    public bool isBoss;

    [DefaultValue(0)]
    public int state;

    [DefaultValue(0F)]
    public float target_x;
    [DefaultValue(0F)]
    public float target_y;
    [DefaultValue(0F)]
    public float target_z;
}
