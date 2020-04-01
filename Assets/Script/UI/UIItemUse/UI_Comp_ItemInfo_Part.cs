using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Bag {
    public partial class UI_Comp_ItemInfo
    {
        public void Init(ItemVO item, bool usable)
        {
            m_txt_name.text = item.Name;
            m_btn_OK.visible = usable;
            m_btn_cancel.visible = usable;

            m_btn_OK.onClick.Add(()=> {
                if (item.Equipment != null)
                {
                    ItemDataManager.Instance.ChangeEquipment(item.Equipment);   
                }
            });
        }
    }
}
