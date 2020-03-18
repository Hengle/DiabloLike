using System.Collections;
using System.Collections.Generic;

public class EquipmentData
{
    public int Id;
    public int Position;
    public int SuitType;
    //属性下限
    public int MinStr = 1;//力量
    public int MinInt = 1;//智力
    public int MinCon = 1;//体质
    public int MinAgi = 1;//敏捷
    public int MinLuc = 1;//幸运

    public int MinHealth = 1;//血量
    public int MinMana = 1;//法力值
    public int MinAtk = 1;//攻击
    public int MinDef = 1;//防御
    public int MinHealthRegen = 1;//回血
    public int MinManaRegen = 1;//回蓝

    public int MinAtkSpeed = 1;//攻速
    public int MinMoveSpeed = 1;//移速

    public int MinCritRate = 1;//暴击率
    public int MinCritDam = 1;//暴伤率

    //属性上限
    public int MaxStr = 1;//力量
    public int MaxInt = 1;//智力
    public int MaxCon = 1;//体质
    public int MaxAgi = 1;//敏捷
    public int MaxLuc = 1;//幸运

    public int MaxHealth = 1;//血量
    public int MaxMana = 1;//法力值
    public int MaxAtk = 1;//攻击
    public int MaxDef = 1;//防御
    public int MaxHealthRegen = 1;//回血
    public int MaxManaRegen = 1;//回蓝

    public int MaxAtkSpeed = 1;//攻速
    public int MaxMoveSpeed = 1;//移速

    public int MaxCritRate = 1;//暴击率
    public int MaxCritDam = 1;//暴伤率
}
