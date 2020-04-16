using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemDrop;
using FairyGUI;

public class UIDamageNum : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "DamageNum"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "ItemDrop"; } }

    UI_DamageNum fgui;


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
 
        fgui = contentPane as UI_DamageNum;
    }

    public override void AfterOnShown(params object[] datas)
    {
        base.AfterOnShown();

        Debug.Log("AfterOnShown");
        fgui.m_txt_num.visible = false;

    }
    protected override void OnHide()
    {
        base.OnHide();
    }
    protected override void OnBtnClose()
    {
        OnHide();
    }

    public void Update()
    {
        //foreach (var item in ItemDropsManager.Instance.ItemList)
        //{
        //    if(item.OutLookTrans != null && item.UIName != null)
        //        item.UIName.xy = GlobalExpansion.WorldPos2FguiPos(item.OutLookTrans.position);
        //}
    }
    /// <summary>
    /// 生成UI组件，并名字赋值
    /// </summary>
    /// <param name="item"></param>
    //public UI_ItemName CreateItemName(DropItem item)
    //{
    //    UI_ItemName name = UIPackage.CreateObject("ItemDrop", "ItemName") as UI_ItemName;
    //    fgui.AddChild(name);

    //    name.xy = GlobalExpansion.WorldPos2FguiPos(item.OutLookTrans.position);

    //    if(item.itemvo.ItemData != null)
    //        name.m_txt_name.text = item.itemvo.ItemData.Name.ToString();
    //    else
    //        name.m_txt_name.text = item.itemvo.Id.ToString();
    //    name.onClick.Add(()=> {
    //        ItemDropsManager.Instance.PickUpItem(item);
    //    });
    //    return name;
    //}
    public void ShowDamageNum(Transform enemyTrans, long num)
    {
        Vector2 xy = GlobalExpansion.WorldPos2FguiPos(enemyTrans.position);
        if( GlobalExpansion.IsInFguiScreen(xy) )
        {
            UI_ItemName name = UIPackage.CreateObject("ItemDrop", "ItemName") as UI_ItemName;
            fgui.AddChild(name);
            name.m_txt_name.text = num.ToString();

            Timers.inst.Add(1, 1, (obj)=>{
                name.Dispose();
            });
        }
    }
}
