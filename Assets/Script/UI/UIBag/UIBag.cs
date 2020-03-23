using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;
using FairyGUI;

public class UIBag : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "Bag"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "Bag"; } }

    UI_Bag fgui;
    private List<ItemVO> itemList = new List<ItemVO>();

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
        fgui = contentPane as UI_Bag;
        fgui.m_btn_close.onClick.Add(OnBtnClose);

        //fgui.m_head.onDrop.Add(OnDragDrop);//拖动没行通
    }
    /// <summary>
    /// 重写显示界面
    /// </summary>
    public override void AfterOnShown(params object[] datas)
    {
        base.AfterOnShown();

        Debug.Log("AfterOnShown");
        ShowBagList();

    }
    protected override void OnHide()
    {
        base.OnHide();
        
    }
    protected override void OnBtnClose()
    {
        Hide();
    }

    void ShowBagList()
    {
        itemList = ItemDataManager.Instance.GetAllBagItem();
        fgui.m_list_item.SetVirtual();
        fgui.m_list_item.itemRenderer = RenderListItem;
        fgui.m_list_item.numItems = itemList.Count;
        fgui.m_list_item.RefreshVirtualList();
    }
    void RenderListItem(int index, GObject obj)
    {
        UI_Comp_Item item = (UI_Comp_Item)obj;
        ItemVO itemvo = itemList[index];


        item.m_txt_name.text = itemvo.Name;


        //item.onClick.Set(() => {
        //    GameDataManager.Instance.UseItem(itemvo.Id, 1, true);
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
