using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


public class GameMonster
{
    [DefaultValue(null)]
    public string id{ get; set; }
    [DefaultValue(null)]
    public string type{ get; set; }
    [DefaultValue(null)]
    public string name { get; set; }
    [DefaultValue(null)]
    public string desc { get; set; }
    [DefaultValue(null)]
    public string icon { get; set; }
    [DefaultValue(null)]
    public string model { get; set; }
    [DefaultValue(0)]
    public int value { get; set; }
    [DefaultValue(0)]
    public int gold { get; set; }
    [DefaultValue(0)]
    public int gem { get; set; }
    [DefaultValue(0)]
    public int level { get; set; }
    [DefaultValue(0F)]
    public float hp { get; set; }
    [DefaultValue(0F)]
    public float hpMax { get; set; }
    [DefaultValue(0F)]
    public float deductHp { get; set; }
    [DefaultValue(0L)]
    public long attId { get; set; }

    [DefaultValue(0F)]
    public float x { get; set; }
    [DefaultValue(0F)]
    public float z { get; set; }
    [DefaultValue(0F)]
    public float xRo { get; set; }
    [DefaultValue(0F)]
    public float zRo { get; set; }
    [DefaultValue(0F)]
    public float action { get; set; }
    [DefaultValue(false)]
    public bool isBoss { get; set; }

    [DefaultValue(0)]
    public int state { get; set; }

    [DefaultValue(0F)]
    public float target_x { get; set; }
    [DefaultValue(0F)]
    public float target_y { get; set; }
    [DefaultValue(0F)]
    public float target_z { get; set; }

    [DefaultValue(null)]
    public GameReward gameReward { get; set; }
}
