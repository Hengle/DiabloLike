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
    }
    /// <summary>
    /// 重写的显示界面
    /// </summary>
    protected override void OnShown()
    {
        base.OnShown();
        this.visible = true;
        Debug.Log("OnShown");
        ShowBagList();
    }
    protected override void OnHide()
    {
        base.OnHide();
        this.visible = false;
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

    }
}
