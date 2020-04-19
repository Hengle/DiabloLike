using System;
using System.Collections.Generic;
using UnityEngine;
using ItemDrop;

/// <summary>
/// 特殊界面入口，主界面等，不适用UImanager同意管理的，都在这里
/// 提供血条，伤害飘字的入口
/// 其实也可以在uimanager中另加队列，管理这些，但是肯定代码越写越多。暂时先这样
/// </summary>
public class SpecialUIManager : MonoSingleton<SpecialUIManager>
{

    UIBloodBar bloodBar = null;
    UIDamageNum damageNum = null;
    public UIMain main = null;

    GameObject m;
    aRPG_Master ms;
    private void Start()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
        
    }
    private void Update()
    {
        if(bloodBar != null)
        {
            bloodBar.Update();
        }
        if (main != null)
        {
            main.Update();
        }
    }

    public void ShowDamageNum(Transform enemyTrans, long num)
    {
        if (damageNum == null)
        {
            damageNum = UIManager.Instance.CreateWindow(EUIType.UIDamageNum) as UIDamageNum;
            damageNum.Show();
            damageNum.AfterOnShown();
        }
        damageNum.ShowDamageNum(enemyTrans, num);
    }
    public void ShowBloodBar(aRPG_EnemyStats enemyStats)
    {
        if (bloodBar == null)
        {
            bloodBar = UIManager.Instance.CreateWindow(EUIType.UIBloodBar) as UIBloodBar;
            bloodBar.Show();
            bloodBar.AfterOnShown();
        }
        bloodBar.ShowBloodBar(enemyStats);
    }
    public void ShowMainUI()
    {
        if (main == null)
        {
            main = UIManager.Instance.CreateWindow(EUIType.UIMain) as UIMain;
            main.Show();
            main.AfterOnShown();
        }
    }
    public void HideMainUI()
    {
        if (main != null)
        {
            main.Hide();
        }
    }
}

