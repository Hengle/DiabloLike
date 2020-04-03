using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentVO
{
    public long UId;

    public int Position;
    public int Id;
    public EquipmentData equipmentData;

    public int Strenght;//力量
    public int Intelligence;//智力
    public int Constitution;//体质
    public int Agility;//敏捷
    public int Lucky;//幸运

    public long Health;//生命
    public long Mana;//法力
    public long Attack;//攻击
    public long Defense;//防御
    public long HealthRegen;//回血
    public long ManaRegen;//回蓝

    public int AtkSpeed;//攻速
    public int MoveSpeed;//移速

    public int CriticalRate;//暴击率
    public int CriticalDamageRate;//暴伤率

    /// <summary>
    /// 随机的生成
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="id"></param>
    public EquipmentVO(long uid, int id)
    {
        UId = uid;
        Id = id;

        equipmentData = DataManager.Instance.GetEquipment(Id);
        Position = equipmentData.Position;

        Strenght = Random.Range(equipmentData.MinStr, equipmentData.MaxStr);
        Intelligence = Random.Range(equipmentData.MinInt, equipmentData.MaxInt);
        Constitution = Random.Range(equipmentData.MinCon, equipmentData.MaxCon);
        Agility = Random.Range(equipmentData.MinAgi, equipmentData.MaxAgi);
        Lucky = Random.Range(equipmentData.MinLuc, equipmentData.MaxLuc);//幸运

        Health = Random.Range(equipmentData.MinHealth, equipmentData.MaxHealth);//血量
        Mana = Random.Range(equipmentData.MinMana, equipmentData.MaxMana);//法力值
        Attack = Random.Range(equipmentData.MinAtk, equipmentData.MaxAtk);//攻击
        Defense = Random.Range(equipmentData.MinDef, equipmentData.MaxDef);//防御
        HealthRegen = Random.Range(equipmentData.MinHealthRegen, equipmentData.MaxHealthRegen);//回血
        ManaRegen = Random.Range(equipmentData.MinManaRegen, equipmentData.MaxManaRegen);//回蓝

        AtkSpeed = Random.Range(equipmentData.MinAtkSpeed, equipmentData.MaxAtkSpeed);//攻速
        MoveSpeed = Random.Range(equipmentData.MinMoveSpeed, equipmentData.MaxMoveSpeed);//移速

        CriticalRate = Random.Range(equipmentData.MinCritRate, equipmentData.MaxCritRate);//暴击率
        CriticalDamageRate = Random.Range(equipmentData.MinCritDam, equipmentData.MaxCritDam);//暴伤率
    }
}
[System.Serializable]
public class Attribute
{
    public int Strenght;//力量
    public int Intelligence;//智力
    public int Constitution;//体质
    public int Agility;//敏捷
    public int Lucky;//幸运

    public long Health;//血量
    public long Mana;//法力值
    public long Attack;//攻击
    public long Defense;//防御
    public long HealthRegen;//回血
    public long ManaRegen;//回蓝

    public int AtkSpeed;//攻速
    public int MoveSpeed;//移速

    public int CriticalRate;//暴击率
    public int CriticalDamageRate;//暴伤率
}

