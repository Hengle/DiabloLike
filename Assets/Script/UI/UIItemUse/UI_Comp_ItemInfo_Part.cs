using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Bag {
    public partial class UI_Comp_ItemInfo
    {
        public void Init(ItemVO item, bool usable, bool isRightAndEquiped)
        {
            m_txt_name.text = item.Name;
            m_btn_OK.visible = usable;
            m_btn_cancel.visible = usable;

            m_txt_property.text = GetAttributeString(item.Equipment);

            if (isRightAndEquiped)
            {
                m_btn_OK.text = "卸下";
                m_btn_OK.onClick.Set(() =>
                {
                    ItemDataManager.Instance.ChangeEquipment(item, item.Equipment.Position, true);
                });
            }
            else
            {
                m_btn_OK.text = "替换";
                m_btn_OK.onClick.Set(() =>
                {
                    if (item.Equipment != null)
                    {
                        ItemDataManager.Instance.ChangeEquipment(item);
                    }
                });
            }
        }
        private string GetAttributeString(EquipmentVO equipment)
        {
            string att = string.Empty;

            if(equipment.Health > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n",  "生命：", equipment.Health, equipment.equipmentData.MinHealth, equipment.equipmentData.MaxHealth);
            }
            if (equipment.Mana > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "法力：", equipment.Mana, equipment.equipmentData.MinMana, equipment.equipmentData.MaxMana);
            }
            if (equipment.Attack > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "攻击：", equipment.Attack, equipment.equipmentData.MinAtk, equipment.equipmentData.MaxAtk);
            }
            if (equipment.Defense > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "防御：", equipment.Defense, equipment.equipmentData.MinDef, equipment.equipmentData.MaxDef);
            }
            if (equipment.Strenght > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "力量：", equipment.Strenght, equipment.equipmentData.MinStr, equipment.equipmentData.MaxStr);
            }
            if (equipment.Intelligence > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "智力：", equipment.Intelligence, equipment.equipmentData.MinInt, equipment.equipmentData.MaxInt);
            }
            if (equipment.Constitution > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "体质：", equipment.Constitution, equipment.equipmentData.MinCon, equipment.equipmentData.MaxCon);
            }
            if (equipment.Agility > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "敏捷：", equipment.Agility, equipment.equipmentData.MinAgi, equipment.equipmentData.MaxAgi);
            }
            if (equipment.Lucky > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "幸运：", equipment.Lucky, equipment.equipmentData.MinLuc, equipment.equipmentData.MaxLuc);
            }
            if (equipment.HealthRegen > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "生命回复：", equipment.HealthRegen, equipment.equipmentData.MinHealthRegen, equipment.equipmentData.MaxHealthRegen);
            }
            if (equipment.ManaRegen > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "法力回复：", equipment.ManaRegen, equipment.equipmentData.MinManaRegen, equipment.equipmentData.MaxManaRegen);
            }
            if (equipment.AtkSpeed > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "攻速：", equipment.AtkSpeed, equipment.equipmentData.MinAtkSpeed, equipment.equipmentData.MaxAtkSpeed);
            }
            if (equipment.MoveSpeed > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "移速：", equipment.MoveSpeed, equipment.equipmentData.MinMoveSpeed, equipment.equipmentData.MaxMoveSpeed);
            }
            if (equipment.CriticalRate > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "暴击：", equipment.CriticalRate, equipment.equipmentData.MinCritRate, equipment.equipmentData.MaxCritRate);
            }
            if (equipment.CriticalDamageRate > 0)
            {
                att += string.Format("{0}{1}({2}--{3})\n", "暴伤：", equipment.CriticalDamageRate, equipment.equipmentData.MinCritDam, equipment.equipmentData.MaxCritDam);
            }

            return att;
        }
    }
}
