using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemDrop;
using FairyGUI;

public class UIItemDrop : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "ItemDrop"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "ItemDrop"; } }

    UI_ItemDrop fgui;


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
        fgui = contentPane as UI_ItemDrop;
        //fgui.m_btn_close.onClick.Add(OnBtnClose);
    }
    /// <summary>
    /// 重写的显示界面
    /// </summary>
    protected override void OnShown()
    {
        base.OnShown();
        this.visible = true;
        fgui.m_itemName.visible = false;
        Debug.Log("OnShown");
        
    }
    protected override void OnHide()
    {
        base.OnHide();
        this.visible = false;
    }
    protected override void OnBtnClose()
    {
        OnHide();
    }

    public void Update()
    {
        foreach (var item in ItemDropsManager.Instance.ItemList)
        {
            if(item.OutLookTrans != null)
                item.UIName.xy = GlobalExpansion.WorldPos2FguiPos(item.OutLookTrans.position);
        }
    }
    /// <summary>
    /// 生成UI组件，并名字赋值
    /// </summary>
    /// <param name="item"></param>
    public UI_ItemName CreateItemName(DropItem item)
    {
        UI_ItemName name = UIPackage.CreateObject("ItemDrop", "ItemName") as UI_ItemName;
        fgui.AddChild(name);

        name.xy = GlobalExpansion.WorldPos2FguiPos(item.OutLookTrans.position);
        name.m_txt_name.text = item.ItemId.ToString();
        name.onClick.Add(()=> {
            ItemDropsManager.Instance.PickUpItem(item);
        });
        return name;
    }

}
