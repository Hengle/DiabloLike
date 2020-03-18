using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Dictionary<int, EquipmentVO> euiqpmentDict = new Dictionary<int, EquipmentVO>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class EquipmentVO
{
    public long UId;
    public long Position;
    public int ConfigId;

    public int Strenght = 1;//力量
    public int Intelligence = 1;//智力
    public int Constitution = 1;//体质
    public int Agility = 1;//敏捷
    public int Lucky = 1;//幸运

    public long Health = 1;//血量
    public long Mana = 1;//法力值
    public long Attack = 1;//攻击
    public long Defense = 1;//防御
    public long HealthRegen = 1;//回血
    public long ManaRegen = 1;//回蓝

    public int AtkSpeed = 1;//攻速
    public int MoveSpeed = 1;//移速

    public int CriticalRate = 1;//暴击率
    public int CriticalDamageRate = 1;//暴伤率
}
public class Attribute
{
    public int Strenght = 1;//力量
    public int Intelligence = 1;//智力
    public int Constitution = 1;//体质
    public int Agility = 1;//敏捷
    public int Lucky = 1;//幸运

    public long Health = 1;//血量
    public long Mana = 1;//法力值
    public long Attack = 1;//攻击
    public long Defense = 1;//防御
    public long HealthRegen = 1;//回血
    public long ManaRegen = 1;//回蓝

    public int AtkSpeed = 1;//攻速
    public int MoveSpeed = 1;//移速

    public int CriticalRate = 1;//暴击率
    public int CriticalDamageRate = 1;//暴伤率
}

