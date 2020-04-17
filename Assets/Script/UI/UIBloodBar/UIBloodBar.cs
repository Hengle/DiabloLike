using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemDrop;
using FairyGUI;

public class UIBloodBar : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "BloodBar"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "ItemDrop"; } }

    UI_BloodBar fgui;
    List<EnemyBarPair> enemyStatsList = new List<EnemyBarPair>();

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
 
        fgui = contentPane as UI_BloodBar;
    }

    public override void AfterOnShown(params object[] datas)
    {
        base.AfterOnShown();

        Debug.Log("AfterOnShown");
        fgui.m_txt_null.visible = false;

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
        for(int i = enemyStatsList.Count - 1; i > -1; i--)
        {
            if(enemyStatsList[i].enemy == null)
            {
                enemyStatsList[i].bar = null;
                enemyStatsList.RemoveAt(i);
            }
        }
        foreach (var item in enemyStatsList)
        {
            Vector2 xy = GlobalExpansion.WorldPos2FguiPos(item.enemy.transform.position);
            if (GlobalExpansion.IsInFguiScreen(xy))
            {
                if(item.bar == null)
                {
                    item.bar = GetBar();
                }
                item.bar.xy = xy;
                item.bar.value = item.enemy.curAttr.Health / (double)item.enemy.baseAttr.Health;
            }
            else
            {
                if (item.bar != null)
                {
                    item.bar.Dispose();
                }
            }
        }
    }
    private UI_Bar_Blood GetBar()
    {
        UI_Bar_Blood bar = UIPackage.CreateObject("ItemDrop", "Bar_Blood") as UI_Bar_Blood;
        fgui.AddChild(bar);
        return bar;
    }

    public void ShowBloodBar(aRPG_EnemyStats enemyStats)
    {
        if (enemyStats != null)
        {
            enemyStatsList.Add(new EnemyBarPair(enemyStats, null));
        }
    }
}
public class EnemyBarPair
{
    public aRPG_EnemyStats enemy;
    public UI_Bar_Blood bar;

    public EnemyBarPair(aRPG_EnemyStats e, UI_Bar_Blood b)
    {
        enemy = e;
        bar = b;
    }
}
