using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;
using FairyGUI;

public class UIMain : UIBase {
    /// <summary>
    /// 当前窗口名字
    /// </summary>
    public override string WinName { get { return "Main"; } }
    /// <summary>
    /// 当前窗口所属Panel
    /// </summary>
    public override string PanelName { get { return "Main"; } }

    UI_Main fgui;
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
        //====
    }
    void ShowWeapon()
    {

    }
    //void ShowBagList()
    //{
    //    RefreshEquipedItem();
    //    itemList = ItemDataManager.Instance.GetAllBagItem();
    //    fgui.m_list_item.SetVirtual();
    //    fgui.m_list_item.itemRenderer = RenderListItem;
    //    fgui.m_list_item.numItems = itemList.Count;
    //    fgui.m_list_item.RefreshVirtualList();
    //}
    //void RenderListItem(int index, GObject obj)
    //{
    //    UI_Comp_Item item = (UI_Comp_Item)obj;
    //    ItemVO itemvo = itemList[index];


    //    item.Init(itemvo);




    //    //GButton b = item.asButton;
    //    //item.draggable = true;
    //    //item.onDragStart.Add((EventContext context) =>
    //    //{
    //    //    //Cancel the original dragging, and start a new one with a agent.
    //    //    context.PreventDefault();

    //    //    DragDropManager.inst.StartDrag(item, item.m_txt_name.text, item.m_txt_name, (int)context.data);
    //    //});

    //    //GButton c = obj.GetChild("c").asButton;
    //    //c.icon = null;
    //    //c.onDrop.Add((EventContext context) =>
    //    //{
    //    //    c.icon = (string)context.data;
    //    //});
    //}
    //private void RefreshEquipedItem()
    //{
    //    fgui.m_head.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Head));
    //    fgui.m_chest.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Chest));
    //    fgui.m_yao.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Belt));
    //    fgui.m_leg.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Leg));
    //    fgui.m_shoe.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Foot));
    //    fgui.m_neck.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Necklace));
    //    fgui.m_ring_1.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Ring_1));
    //    fgui.m_ring_2.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Ring_2));
    //    fgui.m_weapon.Init(ItemDataManager.Instance.GetEquipedItemByPos(EEquipmentPosition.Weapon));

    //}
    //void OnDragDrop(EventContext context)
    //{
    //    fgui.m_head.m_txt_name.text = (string)context.data;
    //}
}
