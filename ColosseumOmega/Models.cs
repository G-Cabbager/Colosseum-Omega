namespace ColosseumOmega;

/// <summary>实体类型</summary>
public enum EntityType
{
    Unit = 1,   // 单位
    Infantry = 2 // 步兵
}

/// <summary>基础实体数据</summary>
public class BaseEntity
{
    public string Owner { get; set; } = "";
    public string Type { get; set; } = "";
    public int Health { get; set; } = 256;
    public int X { get; set; }
    public int Y { get; set; }
    public int Facing { get; set; } = 32;
    public int SubPos { get; set; } = 1;
    public string Status { get; set; } = "Guard";
    public string Tag { get; set; } = "None";
    public int Veterancy { get; set; }
    public int Group { get; set; } = -1;
    public int Layer { get; set; } = -1;
    public int Reserved1 { get; set; } = 1;
    public int Reserved2 { get; set; } = 0;
}