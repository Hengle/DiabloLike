using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Dictionary<int, EquipmentVO> equipmentDict = new Dictionary<int, EquipmentVO>();
    private Attribute allEquipmentsAttribute = new Attribute();
    public Attribute AllEquipmentsAttribute
    {
        get
        {
            foreach (var item in equipmentDict)
            {
                if (item.Value != null)
                {
                    allEquipmentsAttribute = GlobalExpansion.GetSumOfAttributes(allEquipmentsAttribute, item.Value);
                }
            }
            return allEquipmentsAttribute;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 换或者穿
    /// </summary>
    /// <param name="equipment"></param>
    /// <param name="pos"></param>
    public void ChangeEquipment(EquipmentVO equipment, int pos = 0)
    {
        if (equipment == null)
            return;
        if (pos == 0)
            pos = equipment.Position;

        equipmentDict[pos] = equipment;
        EventCenter.Broadcast(EGameEvent.eEquipmentChange);
    }
    /// <summary>
    /// 卸下装备
    /// </summary>
    /// <param name="pos"></param>
    public void PutOffEquipment(int pos)
    {
        if (equipmentDict.ContainsKey(pos))
        {
            equipmentDict.Remove(pos);
        }
        EventCenter.Broadcast( EGameEvent.eEquipmentChange);
    }
    public EquipmentVO GetEquipmentByPos(int pos)
    {
        if (equipmentDict.ContainsKey(pos))
        {
            return equipmentDict[pos];
        }
        return null;
    }
}
public class EquipmentVO
{
    public long UId;
    public int Position;
    public int ConfigId;

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

