using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;
using FairyGUI;

public class UIItemUse : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "ItemUse"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "Bag"; } }

    UI_ItemUse fgui;
    private ItemVO useItem;

    public override void Dispose()
    {
        OnDestroy();
        base.Dispose();
    }

    protected override void OnDestroy()
    {

    }
    protected override void OnInit()
    {
        base.OnInit();
        this.visible = true;
        fgui = contentPane as UI_ItemUse;
        fgui.onClick.Add(OnBtnClose);

        
    }
    /// <summary>
    /// 重写显示界面
    /// </summary>
    public override void AfterOnShown(params object[] datas)
    {
        base.AfterOnShown();

        Debug.Log("AfterOnShown");
        useItem = datas[0] as ItemVO;
        ShowDetail();

    }
    protected override void OnHide()
    {
        base.OnHide();
        
    }
    protected override void OnBtnClose()
    {
        Hide();
    }

    void ShowDetail()
    {
        ItemVO equipedItem = null;
        if (useItem.Equipment != null)
        {
            equipedItem = ItemDataManager.Instance.GetEquipedItemByPos(useItem.Equipment.Position);
        }
        //点击的是以装备的item
        if(equipedItem != null && useItem.UId == equipedItem.UId)
        {
            fgui.m_comp_1.Init(useItem, true, true);
            fgui.m_comp_2.visible = false;
        }
        else
        {
            fgui.m_comp_1.Init(useItem, true, false);
            fgui.m_comp_2.visible = equipedItem != null;
            if (equipedItem != null)
            {
                fgui.m_comp_2.Init(equipedItem, false, false);
            }
        }
        
        
    }
    void RenderListItem(int index, GObject obj)
    {
        UI_Comp_Item item = (UI_Comp_Item)obj;
        //ItemVO itemvo = itemList[index];


        //item.m_txt_name.text = itemvo.Name;


        //item.onClick.Add(() =>
        //{
        //    UIManager.Instance.ShowWind( EUIType.UIItemUse);
        //});

        //GButton b = item.asButton;
        //item.draggable = true;
        //item.onDragStart.Add((EventContext context) =>
        //{
        //    //Cancel the original dragging, and start a new one with a agent.
        //    context.PreventDefault();

        //    DragDropManager.inst.StartDrag(item, item.m_txt_name.text, item.m_txt_name, (int)context.data);
        //});

        //GButton c = obj.GetChild("c").asButton;
        //c.icon = null;
        //c.onDrop.Add((EventContext context) =>
        //{
        //    c.icon = (string)context.data;
        //});
    }
    //void OnDragDrop(EventContext context)
    //{
    //    fgui.m_head.m_txt_name.text = (string)context.data;
    //}
}
