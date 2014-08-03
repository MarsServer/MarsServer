using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


public class GameMonster
{
    [DefaultValue(0L)]
    public long id;
    [DefaultValue(null)]
    public string type;
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
}
