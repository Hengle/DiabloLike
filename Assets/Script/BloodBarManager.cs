using System;
using System.Collections.Generic;
using UnityEngine;
using ItemDrop;

/// <summary>
/// 提供血条，伤害飘字的入口
/// </summary>
public class BloodBarManager : MonoSingleton<BloodBarManager>
{

    UIBloodBar bloodBar = null;
    UIDamageNum damageNum = null;
    

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
}

