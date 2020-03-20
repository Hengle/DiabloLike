using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;

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

    public override void Dispose()
    {
        OnDestroy();
        base.Dispose();
    }

    protected virtual void OnDestroy()
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
}
