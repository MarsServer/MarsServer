using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;


public enum ChatType
{
    Null = 0,
    World,
    Person,
    System,
    Team,
}
public class Message
{
    public const string fotmat = "[url={0}][u]{0}[/u][/url]";


    [DefaultValue(0)]
    public ChatType chatType;
    [DefaultValue(null)]
    public Role sender;
    [DefaultValue(null)]
    public Role receiver;//only Person isnot null
    [DefaultValue(null)]
    public string content;
}
