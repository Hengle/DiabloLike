using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;
using FairyGUI;
using System.Linq;

public class UIMain : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "Main"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "Main"; } }

    public UI_Main fgui;
    //private List<ItemVO> itemList = new List<ItemVO>();
    public int key1;
    public int key2;
    public int key3;
    public int key4;
    public int mouseRight;
    public int mouseLeft;
    public int weaponId;
    public string[] InputKeys = new string[4] { "1","2", "3", "4" };
    public List<SkillData> InputSkills = new List<SkillData>();
    public SkillData rightMouseSkill;
    public SkillData leftMouseSkill;

    List<SkillData> skills;
    UI_Btn_Main curSelectBtn;

    List<aRPG_DB_MakeItemSO> weapons;
    Dictionary<int, aRPG_DB_MakeItemSO> items_Dictionary = new Dictionary<int, aRPG_DB_MakeItemSO>();

    public override void Dispose()
    {
        OnDestroy();
        base.Dispose();
        //EventCenter.RemoveListener(EGameEvent.eEquipmentChange, ShowBagList);
    }

    protected override void OnDestroy()
    {

    }
    protected override void OnInit()
    {
        base.OnInit();
        this.visible = true;
        fgui = contentPane as UI_Main;
        //fgui.m_btn_close.onClick.Add(OnBtnClose);

        //EventCenter.AddListener(EGameEvent.eEquipmentChange, ShowBagList);
        //fgui.m_head.onDrop.Add(OnDragDrop);//拖动没行通
        fgui.m_btn1.onClick.Add(OnSkillBtnClick);
        fgui.m_btn2.onClick.Add(OnSkillBtnClick);
        fgui.m_btn3.onClick.Add(OnSkillBtnClick);
        fgui.m_btn4.onClick.Add(OnSkillBtnClick);
        fgui.m_btnLeft.onClick.Add(OnSkillBtnClick);
        fgui.m_btnRight.onClick.Add(OnSkillBtnClick);
        fgui.m_btnWeapon.onClick.Add(OnWeaponBtnClick);
        fgui.m_btnRespawn.onClick.Add(OnRespawnClick);
        fgui.m_btnLv1.onClick.Add(OnBtnLvClick);
        fgui.m_btnLv2.onClick.Add(OnBtnLvClick);
        fgui.m_btnLv3.onClick.Add(OnBtnLvClick);

        foreach (Object o in Resources.LoadAll("aRPG_WeaponDB", typeof(ScriptableObject)))
        {
            aRPG_DB_MakeItemSO oo = o as aRPG_DB_MakeItemSO;
            items_Dictionary.Add(oo.id, oo);
        }

        ShowSkill();
        ShowWeapon();
    }
    /// <summary>
    /// 重写显示界面
    /// </summary>
    public override void AfterOnShown(params object[] datas)
    {
        base.AfterOnShown();

        Debug.Log("AfterOnShown");
        //ShowBagList();

        fgui.m_group_top.visible = false;
        fgui.m_group_die.visible = false;
        fgui.m_group_lv.visible = false;
        fgui.m_listSkill.visible = false;
        fgui.m_listWeapon.visible = false;

    }
    protected override void OnHide()
    {
        base.OnHide();
        
    }
    protected override void OnBtnClose()
    {
        Hide();
    }
    public void Update()
    {
        fgui.m_bar_blood.value = aRPG_Master.Instance.psStats.curAttr.Health / (double)aRPG_Master.Instance.psStats.baseAttr.Health * 100;
        fgui.m_bar_blue.value = aRPG_Master.Instance.psStats.curAttr.Mana / (double)aRPG_Master.Instance.psStats.baseAttr.Mana * 100;
        fgui.m_bar_exp.value = aRPG_Master.Instance.psStats.Exp / (double)aRPG_Master.Instance.psStats.prevExpToLevelUp * 100;

        if (aRPG_Master.Instance.psInventory.startingEquippedWeapon != null && fgui.m_btnWeapon.m_txt_num.visible)
        {
            fgui.m_btnWeapon.m_txt_num.text = aRPG_Master.Instance.psInventory.startingEquippedWeapon.ammo.ToString();
        }

        if (enemyHealthScript != null)
        {
            fgui.m_barEnemy.value = enemyHealthScript.curAttr.Health / (double)enemyHealthScript.baseAttr.Health * 100;
            if (enemyHealthScript.isDead)
            {
                fgui.m_group_top.visible = false;
            }
        }
    }
    void ShowSkill()
    {
        //key1 = PlayerPrefs.GetInt("key1");
        //key2 = PlayerPrefs.GetInt("key2");
        //key3 = PlayerPrefs.GetInt("key3");
        //key4 = PlayerPrefs.GetInt("key4");
        //mouseRight = PlayerPrefs.GetInt("mouseRight");
        //mouseLeft = PlayerPrefs.GetInt("mouseLeft");
        //weaponId = PlayerPrefs.GetInt("weaponId");
        key1 = key1 == 0 ? 4 : key1;
        key2 = key2 == 0 ? 11 : key2;
        key3 = key3 == 0 ? 13 : key3;
        key4 = key4 == 0 ? 4 : key4;
        mouseRight = mouseRight == 0 ? 13 : mouseRight;
        mouseLeft = mouseLeft == 0 ? 13 : mouseLeft;
        weaponId = weaponId == 0 ? 1 : weaponId;

        //FairyGUI.UIPackage.GetItemAsset("SpriteResource", name) as FairyGUI.NTexture;
        SkillData skill1 = DataManager.Instance.GetSkill(key1);        
        SkillData skill2 = DataManager.Instance.GetSkill(key2);
        SkillData skill3 = DataManager.Instance.GetSkill(key3);
        SkillData skill4 = DataManager.Instance.GetSkill(key4);
        leftMouseSkill  = DataManager.Instance.GetSkill(mouseLeft);
        rightMouseSkill = DataManager.Instance.GetSkill(mouseRight);

        fgui.m_btn1.icon = UIPackage.GetItemURL("SpriteRes", skill1.sprite);
        fgui.m_btn2.icon = UIPackage.GetItemURL("SpriteRes", skill2.sprite);
        fgui.m_btn3.icon = UIPackage.GetItemURL("SpriteRes", skill3.sprite);
        fgui.m_btn4.icon = UIPackage.GetItemURL("SpriteRes", skill4.sprite);
        fgui.m_btnLeft.icon = UIPackage.GetItemURL("SpriteRes", leftMouseSkill.sprite);
        fgui.m_btnRight.icon = UIPackage.GetItemURL("SpriteRes", rightMouseSkill.sprite);

        InputSkills.Clear();
        InputSkills.Add(skill1);
        InputSkills.Add(skill2);
        InputSkills.Add(skill3);
        InputSkills.Add(skill4);

        //weaponId
        if (items_Dictionary[weaponId].weaponIcon != null)
        {
            fgui.m_btnWeapon.m_icon.texture = new NTexture(items_Dictionary[weaponId].weaponIcon);
        }
        else
        {
            fgui.m_btnWeapon.m_icon.texture = null;
        }
        fgui.m_btnWeapon.m_txt_num.visible = items_Dictionary[weaponId].hasLimitedNoOfUses;
    }
    void ShowWeapon()
    {
        weapons = items_Dictionary.Values.ToList();
        fgui.m_listWeapon.SetVirtual();
        fgui.m_listWeapon.itemRenderer = RenderListItemWeapon;
        fgui.m_listWeapon.numItems = weapons.Count;
        fgui.m_listWeapon.RefreshVirtualList();
    }
    void RenderListItemWeapon(int index, GObject obj)
    {
        UI_Btn_List item = (UI_Btn_List)obj;
        aRPG_DB_MakeItemSO wea = weapons[index];


        if (wea.weaponIcon != null)
        {
            item.m_icon.texture = new NTexture(wea.weaponIcon);
        }
        else
        {
            item.m_icon.texture = null;
        }
        item.onClick.Add(() => {
            weaponId = wea.id;
            fgui.m_btnWeapon.m_icon.texture = item.m_icon.texture;
            fgui.m_btnWeapon.m_txt_num.visible = wea.hasLimitedNoOfUses;
            aRPG_Master.Instance.psInventory.ChangeWeapon(wea);
            fgui.m_listWeapon.visible = false;
        });

    }

    void OnSkillBtnClick(EventContext context)
    {
        //fgui.m_head.m_txt_name.text = (string)context.data;
        UI_Btn_Main btn = context.sender as UI_Btn_Main;
        //fgui.m_listSkill.x = btn.x;
        fgui.m_listSkill.visible = true;
        
        curSelectBtn = btn;

        skills = DataManager.Instance.skillDataDic.Values.ToList();
        fgui.m_listSkill.SetVirtual();
        fgui.m_listSkill.itemRenderer = RenderListItem;
        fgui.m_listSkill.numItems = skills.Count;
        fgui.m_listSkill.RefreshVirtualList();
    }
    void RenderListItem(int index, GObject obj)
    {
        UI_Btn_List item = (UI_Btn_List)obj;
        SkillData skill = skills[index];


        item.icon = UIPackage.GetItemURL("SpriteRes", skill.sprite);
        item.onClick.Add(()=> {
            SelectSkill(skill.Id);
        });

    }
    void SelectSkill(int id)
    {
        if (curSelectBtn == fgui.m_btn1)
        {
            key1 = id;
        }
        else if (curSelectBtn == fgui.m_btn2)
        {
            key2 = id;
        }
        else if (curSelectBtn == fgui.m_btn3)
        {
            key3 = id;
        }
        else if (curSelectBtn == fgui.m_btn4)
        {
            key4 = id;
        }
        else if (curSelectBtn == fgui.m_btnLeft)
        {
            mouseLeft = id;
        }
        else if (curSelectBtn == fgui.m_btnRight)
        {
            mouseRight = id;
        }
        fgui.m_listSkill.visible = false;
        ShowSkill();
    }

   void OnWeaponBtnClick(EventContext context)
    {
        fgui.m_listWeapon.visible = true;
    }
    void OnRespawnClick(EventContext context)
    {
        aRPG_Master.Instance.Respawn();
    }
    void OnBtnLvClick(EventContext context)
    {
        if(context.sender == fgui.m_btnLv1)
        {
            Application.LoadLevel("1.Level_Small");
        }
        else if (context.sender == fgui.m_btnLv2)
        {
            Application.LoadLevel("3.Level_Large");
        }
        else if (context.sender == fgui.m_btnLv3)
        {
            Application.LoadLevel("2.Level_Doors");
        }
    }


    GameObject enemy;
    aRPG_EnemyStats enemyHealthScript;
    string enemyName;
    /// <summary>
    /// 顶部血条
    /// </summary>
    /// <param name="receivedEnemy"></param>
    public void GetTargetEnemy(GameObject receivedEnemy)
    {
        enemy = receivedEnemy;
        enemyHealthScript = enemy.GetComponent<aRPG_EnemyStats>();
        enemyName = enemyHealthScript.thisName;
        fgui.m_txtEnemyName.text = enemyName;
        fgui.m_txtEnemyName.color = Color.white;
        fgui.m_group_top.visible = true;
        if (enemyHealthScript.monsterModsDefinition == aRPG_EnemyStats.modsDefinition.Rare)
        {
            fgui.m_txtEnemyName.color = Color.yellow;
        }
        if (enemyHealthScript.monsterModsDefinition == aRPG_EnemyStats.modsDefinition.Champion)
        {
            fgui.m_txtEnemyName.color = Color.cyan;
        }
    }
    public void ShowDieUI(bool die)
    {
        fgui.m_group_die.visible = die;
    }
    public void ShowWayPointMenu(bool show)
    {
        fgui.m_group_lv.visible = show;
    }
}
