using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Bag {
    public partial class UI_Comp_Item
    {
        public void Init(ItemVO item)
        {
            if (item != null)
            {
                m_txt_name.text = item.Name;


                this.onClick.Add(() =>
                {
                    UIManager.Instance.ShowWind(EUIType.UIItemUse, item);
                });
            }
        }
    }
}
