using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// some general info on GUI toolbar:
// all childs of MainCanvas that name ends with "@" have a name that is sensitive, meaning this script uses its name to find it. Changing name of such object will result in error.
// all childs of MainCanvas that name ends with "#" have an GUI event in it.

// this script manages toolbar button behaviore and weapon/skill switching.
//图形用户界面工具栏上的一些常规信息：
//以“@”结尾的主画布的所有子级都有一个敏感的名称，这意味着此脚本使用其名称来查找它。更改此对象的名称将导致错误。
//名字以“#”结尾的主画布的所有子级都有一个GUI事件。

//此脚本管理工具栏按钮行为和武器/技能切换。

public class aRPG_GuiTbManagement : MonoBehaviour {
    
    GameObject m;
    aRPG_Master ms;

    int i = 0;

    GameObject pressedButton;
    GameObject actionButtonsPanel;
    GameObject skillsSelectionPanel;
    GameObject weaponSelectionPanel;

    [Header("Skills selection menu position")]
    public float skillsSelectionPositionMod_X = -30f;
    public float skillsSelectionPositionMod_Y = 77f;
    [Header("Weapons selection menu position")]
    public float weaponSelectionPositionMod_X = 77f;
    public float weaponSelectionPositionMod_Y = 77f;

    internal int Buttons_numberOf = 0;
    
    // left, right mouse buttons and 1,2,3 etc., buttons...
    internal GameObject[]             Buttons = new GameObject[22];
    // ... and their components:
    internal Image[]                  Buttons_Image;
    internal aRPG_DB_MakeSkillSO[]    Buttons_ActiveSkill;
    aRPG_GuiButton_Action[]         Buttons_Script;
    internal string[]                 Buttons_InputKey;
    Text[]                          Buttons_InputText;
    Text[]                          Buttons_AmmoText;
    
    public Dictionary<int, GameObject> buttons_Dictionary = new Dictionary<int, GameObject>();
    
    int button_WeaponSelection_Index;
    internal aRPG_GuiButton_Action buttonWeaponSelectionScript;
    internal GameObject buttonWeaponSelection;
    internal Image buttonWeaponSelectionImage;
    internal Text buttonWeaponSelectAmmoText;
    internal Text buttonWeaponSelectInputText;

    int button_MouseLeft_Index;
    internal GameObject buttonMouseLeft;
    internal Image buttonMouseLeftImage;
    internal aRPG_DB_MakeSkillSO buttonMouseLeftActiveSkill;
    internal aRPG_GuiButton_Action buttonMouseLeftScript;
    internal Text btt_mLeft_AmmoCounter;
    internal Text btt_mLeft_InputText;

    int button_MouseRight_Index;
    internal GameObject buttonMouseRight;
    internal Image buttonMouseRightImage;
    internal aRPG_DB_MakeSkillSO buttonMouseRightActiveSkill;
    internal aRPG_GuiButton_Action buttonMouseRightScript;
    internal Text btt_mRight_AmmoCounter;
    internal Text btt_mRight_InputText;
    
    // buttons for weapons selection
    internal GameObject[] Buttons_WeaponSelect;
    internal aRPG_GuiButton_Selection[] Buttons_WeaponSelect_Script;

    // for scriptable objects operations
    internal Dictionary<string, aRPG_DB_MakeItemSO> items_Dictionary = new Dictionary<string, aRPG_DB_MakeItemSO>();
    internal Dictionary<string, aRPG_DB_MakeSkillSO> skills_Dictionary = new Dictionary<string, aRPG_DB_MakeSkillSO>();
    internal aRPG_DB_MakeSkillSO[] skills_Array;
    internal string[] skills_NamesArray;
    
    // below variables are used for mobile input
    [HideInInspector] public bool movementLocked = false;
    GameObject buttonHeld;
    
    void Awake()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

        weaponSelectionPanel = GameObject.Find("MainCanvas/Weapons_@#");
        skillsSelectionPanel = GameObject.Find("MainCanvas/Skills_@#");
        actionButtonsPanel = GameObject.Find("MainCanvas/ActionButtons");

        skillsSelectionPanel.SetActive(false);
        weaponSelectionPanel.SetActive(false);

        CreateButtonsArrays();
        CreateSkillsSoDatabase();
        CreateItemsSoDatabase();
        CreateWeaponsPanelArray();

        SetupSkillsForButtons();

    }

    void Start ()
    {

        RefreshInputKeysDisplay();
        RefreshButtonMainTexture();
        RefreshWeaponButtonMainTexture();
        ResolveAmmoCountersStatus();
    }

    void Update()
    {
        RefreshAmmoInCounters();
        ChangeTextureBaseOnMana();
        ButtonTextureCooldown();
        PointerHeld();
    }
    
    void SetupSkillsForButtons()
    {
        // fill up array of active skills for action buttons
        Buttons_ActiveSkill = new aRPG_DB_MakeSkillSO[Buttons_numberOf];
        for (i = 0; i < Buttons_numberOf; i++)
        {
            if (Buttons_Script[i].startingSkill == null)
            {
                Buttons_ActiveSkill[i] = skills_Dictionary["BaseAttack"];
            }
            else
            {
                Buttons_ActiveSkill[i] = Buttons_Script[i].startingSkill;
            }
        }
        // setup active skills for mouse buttons
        if (buttonMouseLeftScript.startingSkill == null)
        {
            buttonMouseLeftActiveSkill = skills_Dictionary["BaseAttack"];
        }
        else
        {
            buttonMouseLeftActiveSkill = buttonMouseLeftScript.startingSkill;
        }

        if (buttonMouseRightScript.startingSkill == null)
        {
            buttonMouseRightActiveSkill = skills_Dictionary["BaseAttack"];
        }
        else
        {
            buttonMouseRightActiveSkill = buttonMouseRightScript.startingSkill;
        }
    }

    void CreateButtonsArrays()
    {
        Buttons_Script = new aRPG_GuiButton_Action[actionButtonsPanel.transform.childCount];

        // Find skills buttons(1,2,3, etc., mouse left, right, weapon selection) and segregate them.
        for (i = 0; i < actionButtonsPanel.transform.childCount; i++)
        {
            Buttons[i] = actionButtonsPanel.transform.GetChild(i).gameObject;
            Buttons_Script[i] = Buttons[i].GetComponent<aRPG_GuiButton_Action>();

            if (Buttons_Script[i].thisButton_Character == buttonCharacter.WeaponSelection)
            {
                button_WeaponSelection_Index = i;
            }
            if (Buttons_Script[i].thisButton_Character == buttonCharacter.MouseRight)
            {
                button_MouseRight_Index = i;
            }
            if (Buttons_Script[i].thisButton_Character == buttonCharacter.MouseLeft)
            {
                button_MouseLeft_Index = i;
            }
            if (Buttons_Script[i].thisButton_Character == buttonCharacter.Keyboard_Mobile)
            {
                buttons_Dictionary.Add(Buttons_numberOf, Buttons[i]);
                Buttons_numberOf++;
            }
        }

        // assign components to variables:
        // weapon selection button
        buttonWeaponSelection = Buttons[button_WeaponSelection_Index];
        buttonWeaponSelectionScript = buttonWeaponSelection.GetComponent<aRPG_GuiButton_Action>();
        buttonWeaponSelectionImage = buttonWeaponSelection.transform.Find("Icon").GetComponent<Image>();
        buttonWeaponSelectAmmoText = buttonWeaponSelection.transform.Find("Text_Counter_@").GetComponent<Text>();
        buttonWeaponSelectInputText = buttonWeaponSelection.transform.Find("Text_InputKey_@").GetComponent<Text>();
        // right mouse button
        buttonMouseRight = Buttons[button_MouseRight_Index];
        buttonMouseRightScript = buttonMouseRight.GetComponent<aRPG_GuiButton_Action>();
        buttonMouseRightImage = buttonMouseRight.transform.Find("Icon").GetComponent<Image>();
        btt_mRight_AmmoCounter = buttonMouseRight.transform.Find("Text_Counter_@").GetComponent<Text>();
        btt_mRight_InputText = buttonMouseRight.transform.Find("Text_InputKey_@").GetComponent<Text>();
        // left mouse button
        buttonMouseLeft = Buttons[button_MouseLeft_Index];
        buttonMouseLeftScript = buttonMouseLeft.GetComponent<aRPG_GuiButton_Action>();
        buttonMouseLeftImage = buttonMouseLeft.transform.Find("Icon").GetComponent<Image>();
        btt_mLeft_AmmoCounter = buttonMouseLeft.transform.Find("Text_Counter_@").GetComponent<Text>();
        btt_mLeft_InputText = buttonMouseLeft.transform.Find("Text_InputKey_@").GetComponent<Text>();
        // action buttons arrays
        Buttons = new GameObject[Buttons_numberOf];
        Buttons_Script = new aRPG_GuiButton_Action[Buttons_numberOf];
        Buttons_Image = new Image[Buttons_numberOf];
        Buttons_AmmoText = new Text[Buttons_numberOf];
        Buttons_InputText = new Text[Buttons_numberOf];
        Buttons_InputKey = new string[Buttons_numberOf];

        for (i = 0; i < Buttons_numberOf; i++)
        {
            Buttons[i] = buttons_Dictionary[i];
            Buttons_Script[i] = Buttons[i].GetComponent<aRPG_GuiButton_Action>();
            Buttons_Image[i] = Buttons[i].transform.Find("Icon").GetComponent<Image>();
            Buttons_AmmoText[i] = Buttons[i].transform.Find("Text_Counter_@").GetComponent<Text>();
            Buttons_InputText[i] = Buttons[i].transform.Find("Text_InputKey_@").GetComponent<Text>();
            Buttons_InputKey[i] = Buttons_Script[i].thisButton_InputKey;
        }
    }
    
    void CreateSkillsSoDatabase()
    {
        /*______________SkillsDB__________*/
        // create skills dictionary from scriptable objects
        i = 0;
        skills_NamesArray = new string[Resources.LoadAll("aRPG_SkillsDB", typeof(ScriptableObject)).Length];
        foreach (Object o in Resources.LoadAll("aRPG_SkillsDB", typeof(ScriptableObject)))
        {
            aRPG_DB_MakeSkillSO oo = o as aRPG_DB_MakeSkillSO;
            skills_Dictionary.Add(oo.UNIQUE_skillName, o as aRPG_DB_MakeSkillSO);
            skills_NamesArray[i] = oo.UNIQUE_skillName;
            i++;
        }

        // play OnAwake for each skill scriptable object
        foreach (Object o in Resources.LoadAll("aRPG_SkillsDB", typeof(ScriptableObject)))
        {
            aRPG_DB_MakeSkillSO oo = o as aRPG_DB_MakeSkillSO;
            oo.OnAwake();
        }
        /*______________SkillsDB__________*/
    }
    
    void CreateItemsSoDatabase()
    {
        /*___________ItemsDB________*/

        // create items dictionary from scriptable objects

        i = 0;
        foreach (Object o in Resources.LoadAll("aRPG_WeaponDB", typeof(ScriptableObject)))
        {
            aRPG_DB_MakeItemSO oo = o as aRPG_DB_MakeItemSO;
            items_Dictionary.Add(oo.weaponModelName , oo);
            i++;
        }

        // weapon availability is determined by whether weapon button is active or not

        /*___________ItemsDB________*/
    }

    void CreateWeaponsPanelArray()
    {
        /*________________WeaponsPanel__________*/
        
        Buttons_WeaponSelect = new GameObject[weaponSelectionPanel.transform.childCount];
        Buttons_WeaponSelect_Script = new aRPG_GuiButton_Selection[weaponSelectionPanel.transform.childCount];

        for (i = 0; i < weaponSelectionPanel.transform.childCount; i++)
        {
            Buttons_WeaponSelect[i] = weaponSelectionPanel.transform.GetChild(i).gameObject;
            Buttons_WeaponSelect_Script[i] = Buttons_WeaponSelect[i].GetComponent<aRPG_GuiButton_Selection>();
        }

        /*________________WeaponsPanel__________*/
    }

    // for Mobile/WASD input
    public void PointerHeld()
    {
        ms.psInput.PointerHeld();
    }

    // for Mobile/WASD input
    public void PointerDown(GameObject pressedButtonToPass)
    {
        ms.psInput.PointerDown(pressedButtonToPass);
    }

    // for Mobile/WASD input 
    public void PointerUp()
    {
        ms.psInput.PointerUp();
    }
    
    // is used in aRPG_Skills for icon sprite cooldown visualization
    public void ButtonTextureCooldown()
    {
        for (i = 0; i < Buttons_numberOf; i++)
        {
            if(Buttons_ActiveSkill[i].delayIsOn && Buttons_Image[i].fillAmount == 1f)
            {
                Buttons_Image[i].fillAmount = 0f;
            }

            if (Buttons_ActiveSkill[i].delayIsOn)
            {
                Buttons_Image[i].fillAmount = Buttons_ActiveSkill[i].currentSpriteFill;
            }

            if (Buttons_ActiveSkill[i].delayIsOn == false)
            {
                Buttons_Image[i].fillAmount = 1f;
            }
        }

        // mouse left
        if(buttonMouseLeftActiveSkill.delayIsOn && buttonMouseLeftImage.fillAmount == 1f)
        {
            buttonMouseLeftImage.fillAmount = 0f;
        }
        if (buttonMouseLeftActiveSkill.delayIsOn)
        {
            buttonMouseLeftImage.fillAmount = buttonMouseLeftActiveSkill.currentSpriteFill;
        }
        if(buttonMouseLeftActiveSkill.delayIsOn == false)
        {
            buttonMouseLeftImage.fillAmount = 1f;
        }
        // mouse right
        if (buttonMouseRightActiveSkill.delayIsOn && buttonMouseRightImage.fillAmount == 1f)
        {
            buttonMouseRightImage.fillAmount = 0f;
        }
        if (buttonMouseRightActiveSkill.delayIsOn)
        {
            buttonMouseRightImage.fillAmount = buttonMouseRightActiveSkill.currentSpriteFill;
        }
        if (buttonMouseRightActiveSkill.delayIsOn == false)
        {
            buttonMouseRightImage.fillAmount = 1f;
        }
    }
    
    // # this takes care of mana based skills icons go monochrome when mana is low.
    public void ChangeTextureBaseOnMana()
    {
        // Action Buttons
        for (i=0; i<Buttons_numberOf; i++)
        {
            // AoE
            if(Buttons_ActiveSkill[i].skillArchetype == archetype.AoE && ms.psStats.curAttr.Mana < Buttons_ActiveSkill[i].AoEmanaCost && Buttons_Image[i].sprite == Buttons_ActiveSkill[i].sprite)
            {
                Buttons_Image[i].sprite = Buttons_ActiveSkill[i].spriteNoMana;
            }
            if (Buttons_ActiveSkill[i].skillArchetype == archetype.AoE && ms.psStats.curAttr.Mana > Buttons_ActiveSkill[i].AoEmanaCost && Buttons_Image[i].sprite == Buttons_ActiveSkill[i].spriteNoMana)
            {
                Buttons_Image[i].sprite = Buttons_ActiveSkill[i].sprite;
            }
            // Projectile
            if (Buttons_ActiveSkill[i].skillArchetype == archetype.Projectile && ms.psStats.curAttr.Mana < Buttons_ActiveSkill[i].manaCostProjectile && Buttons_Image[i].sprite == Buttons_ActiveSkill[i].sprite)
            {
                Buttons_Image[i].sprite = Buttons_ActiveSkill[i].spriteNoMana;
            }
            if (Buttons_ActiveSkill[i].skillArchetype == archetype.Projectile && ms.psStats.curAttr.Mana > Buttons_ActiveSkill[i].manaCostProjectile && Buttons_Image[i].sprite == Buttons_ActiveSkill[i].spriteNoMana)
            {
                Buttons_Image[i].sprite = Buttons_ActiveSkill[i].sprite;
            }
            // Ray
            if (Buttons_ActiveSkill[i].skillArchetype == archetype.DoT && ms.psStats.curAttr.Mana < Buttons_ActiveSkill[i].manaCostPerSecOrUse && Buttons_Image[i].sprite == Buttons_ActiveSkill[i].sprite)
            {
                Buttons_Image[i].sprite = Buttons_ActiveSkill[i].spriteNoMana;
            }
            if (Buttons_ActiveSkill[i].skillArchetype == archetype.DoT && ms.psStats.curAttr.Mana > Buttons_ActiveSkill[i].manaCostPerSecOrUse && Buttons_Image[i].sprite == Buttons_ActiveSkill[i].spriteNoMana)
            {
                Buttons_Image[i].sprite = Buttons_ActiveSkill[i].sprite;
            }

        }

        // Mouse Left
        if (buttonMouseLeftActiveSkill.skillArchetype == archetype.AoE && ms.psStats.curAttr.Mana < buttonMouseLeftActiveSkill.AoEmanaCost && buttonMouseLeftImage.sprite == buttonMouseLeftActiveSkill.sprite)
        {
            buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.spriteNoMana;
        }
        if (buttonMouseLeftActiveSkill.skillArchetype == archetype.AoE && ms.psStats.curAttr.Mana > buttonMouseLeftActiveSkill.AoEmanaCost && buttonMouseLeftImage.sprite == buttonMouseLeftActiveSkill.spriteNoMana)
        {
            buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.sprite;
        }

        if (buttonMouseLeftActiveSkill.skillArchetype == archetype.Projectile && ms.psStats.curAttr.Mana < buttonMouseLeftActiveSkill.manaCostProjectile && buttonMouseLeftImage.sprite == buttonMouseLeftActiveSkill.sprite)
        {
            buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.spriteNoMana;
        }
        if (buttonMouseLeftActiveSkill.skillArchetype == archetype.Projectile && ms.psStats.curAttr.Mana > buttonMouseLeftActiveSkill.manaCostProjectile && buttonMouseLeftImage.sprite == buttonMouseLeftActiveSkill.spriteNoMana)
        {
            buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.sprite;
        }

        if (buttonMouseLeftActiveSkill.skillArchetype == archetype.DoT && ms.psStats.curAttr.Mana < buttonMouseLeftActiveSkill.manaCostPerSecOrUse && buttonMouseLeftImage.sprite == buttonMouseLeftActiveSkill.sprite)
        {
            buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.spriteNoMana;
        }
        if (buttonMouseLeftActiveSkill.skillArchetype == archetype.DoT && ms.psStats.curAttr.Mana > buttonMouseLeftActiveSkill.manaCostPerSecOrUse && buttonMouseLeftImage.sprite == buttonMouseLeftActiveSkill.spriteNoMana)
        {
            buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.sprite;
        }

        // Mouse right
        if (buttonMouseRightActiveSkill.skillArchetype == archetype.AoE && ms.psStats.curAttr.Mana < buttonMouseRightActiveSkill.AoEmanaCost && buttonMouseRightImage.sprite == buttonMouseRightActiveSkill.sprite)
        {
            buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.spriteNoMana;
        }
        if (buttonMouseRightActiveSkill.skillArchetype == archetype.AoE && ms.psStats.curAttr.Mana > buttonMouseRightActiveSkill.AoEmanaCost && buttonMouseRightImage.sprite == buttonMouseRightActiveSkill.spriteNoMana)
        {
            buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.sprite;
        }

        if (buttonMouseRightActiveSkill.skillArchetype == archetype.Projectile && ms.psStats.curAttr.Mana < buttonMouseRightActiveSkill.manaCostProjectile && buttonMouseRightImage.sprite == buttonMouseRightActiveSkill.sprite)
        {
            buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.spriteNoMana;
        }
        if (buttonMouseRightActiveSkill.skillArchetype == archetype.Projectile && ms.psStats.curAttr.Mana > buttonMouseRightActiveSkill.manaCostProjectile && buttonMouseRightImage.sprite == buttonMouseRightActiveSkill.spriteNoMana)
        {
            buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.sprite;
        }

        if (buttonMouseRightActiveSkill.skillArchetype == archetype.DoT && ms.psStats.curAttr.Mana < buttonMouseRightActiveSkill.manaCostPerSecOrUse && buttonMouseRightImage.sprite == buttonMouseRightActiveSkill.sprite)
        {
            buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.spriteNoMana;
        }
        if (buttonMouseRightActiveSkill.skillArchetype == archetype.DoT && ms.psStats.curAttr.Mana > buttonMouseRightActiveSkill.manaCostPerSecOrUse && buttonMouseRightImage.sprite == buttonMouseRightActiveSkill.spriteNoMana)
        {
            buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.sprite;
        }
    }

    // # next two functions are called when a skill/weapon change occurs to display correct texture.
    public void RefreshWeaponButtonMainTexture()
    {
        if (ms.psInventory.startingEquippedWeapon != null) { buttonWeaponSelectionImage.sprite = ms.psInventory.startingEquippedWeapon.weaponIcon; } else { buttonWeaponSelectionImage.sprite = null; }
    }

    public void RefreshButtonMainTexture()
    {
        for (i = 0; i < Buttons_numberOf; i++)
        {
            Buttons_Image[i].sprite = Buttons_ActiveSkill[i].sprite;
        }

        buttonMouseLeftImage.sprite = buttonMouseLeftActiveSkill.sprite;
        buttonMouseRightImage.sprite = buttonMouseRightActiveSkill.sprite;
    }

    // # this function in final stage of development can be moved from update to scripts where ammo is increased/decreased but for development stage I suggest to keep it here(less things to keep in mind ;) ). If you dont use ammo you can remove that.
    public void RefreshAmmoInCounters()
    {
        for(i=0; i<Buttons_numberOf; i++)
        {
            if (Buttons_AmmoText[i].enabled) { Buttons_AmmoText[i].text = Buttons_ActiveSkill[i].ammo_amount.ToString(); }
        }

        if (btt_mLeft_AmmoCounter.enabled) { btt_mLeft_AmmoCounter.text = buttonMouseLeftActiveSkill.ammo_amount.ToString(); }
        if (btt_mRight_AmmoCounter.enabled) { btt_mRight_AmmoCounter.text = buttonMouseRightActiveSkill.ammo_amount.ToString(); }

        if (ms.psInventory.startingEquippedWeapon != null && buttonWeaponSelectAmmoText != null)
        {
            buttonWeaponSelectAmmoText.text = ms.psInventory.startingEquippedWeapon.ammo.ToString();
        }
    }

    // # this F turns on/off ammo counters based on weapons/skills in use. It is called when a weapon/skills change takes place.
    //显示按钮，右下，是否需要子弹的
    public void ResolveAmmoCountersStatus()
    {
        // Action Buttons
        for (i = 0; i < Buttons_numberOf; i++)
        {
            if (Buttons_ActiveSkill[i].hasLimitedNoOfUses) { Buttons_AmmoText[i].enabled = true; }
            else { Buttons_AmmoText[i].enabled = false; }
        }

        if(buttonMouseLeftActiveSkill.hasLimitedNoOfUses) { btt_mLeft_AmmoCounter.enabled = true; } else { btt_mLeft_AmmoCounter.enabled = false; }
        if(buttonMouseRightActiveSkill.hasLimitedNoOfUses) { btt_mRight_AmmoCounter.enabled = true; } else { btt_mRight_AmmoCounter.enabled = false; }
        
        // Weapon Button
        if(ms.psInventory.startingEquippedWeapon == null)
        {
            buttonWeaponSelectAmmoText.enabled = false;
        }
        else
        {
            if (!ms.psInventory.startingEquippedWeapon.hasLimitedNoOfUses)
            {
                buttonWeaponSelectAmmoText.enabled = false;
            }
            else
            {
                buttonWeaponSelectAmmoText.enabled = true;
            }
        }
    }

    // # at start up we call this function to match input keys we set up in input script and display correct key that triggers input of a button. If you have a GUI system that has input management windows you can call this function to refresh text fields.
    public void RefreshInputKeysDisplay()
    {
        for (i = 0; i < Buttons_numberOf; i++)
        {
            Buttons_InputText[i].text = Buttons_Script[i].thisButton_InputKey;
        }
    }

    // # next two functions are used when we click a toolbar button - it opens menu panel.
    public void OpenSkillsPanel(GameObject pressedButtonToPass)
    {
        if(ms.psInput.actionButtonsInput == false)
        { return; }

        pressedButton = pressedButtonToPass;
        skillsSelectionPanel.SetActive(true);

        RectTransform pb_Rect = pressedButton.gameObject.GetComponent<RectTransform>();
        RectTransform sSP_Rect = skillsSelectionPanel.GetComponent<RectTransform>();
        
        sSP_Rect.anchoredPosition = new Vector2(pb_Rect.anchoredPosition.x + skillsSelectionPositionMod_X, pb_Rect.anchoredPosition.y + skillsSelectionPositionMod_Y);
    }

    public void OpenWeaponsPanel(GameObject pressedButtonToPass)
    {
        if (ms.psInput.actionButtonsInput == false)
        { return; }

        CheckWeaponsAvailabilty();

        pressedButton = pressedButtonToPass;
        weaponSelectionPanel.SetActive(true);

        RectTransform pb_Rect = pressedButton.gameObject.GetComponent<RectTransform>();
        RectTransform wSP_Rect = weaponSelectionPanel.GetComponent<RectTransform>();

        wSP_Rect.anchoredPosition = new Vector2(pb_Rect.anchoredPosition.x + weaponSelectionPositionMod_X, pb_Rect.anchoredPosition.y + weaponSelectionPositionMod_Y);
        
    }
    
    // # ... below functions are called when we press a button in menu panel. That triggers a change of skill/weapon.
    public void SkillsPanel_OnButtonPress(GameObject skillsPanel_PressedButton)
    {
        GameObject tempB = skillsPanel_PressedButton;
        
        for(i=0; i < Buttons_numberOf; i++)
        {
            if(pressedButton == Buttons[i])
            {
                Buttons_ActiveSkill[i] = tempB.GetComponent<aRPG_GuiButton_Selection>().skill;
            }
        }
        
        if(pressedButton == buttonMouseLeft) { buttonMouseLeftActiveSkill = tempB.GetComponent<aRPG_GuiButton_Selection>().skill; }
        if(pressedButton == buttonMouseRight) { buttonMouseRightActiveSkill = tempB.GetComponent<aRPG_GuiButton_Selection>().skill; }

        RefreshButtonMainTexture();
        ResolveAmmoCountersStatus();

        skillsSelectionPanel.SetActive(false);
    }

    public void WeaponsPanel_OnButtonPress(GameObject weaponsPanel_PressedButton)
    {
        ms.psInventory.ChangeWeapon(weaponsPanel_PressedButton.GetComponent<aRPG_GuiButton_Selection>().item);
        
        RefreshWeaponButtonMainTexture();
        ResolveAmmoCountersStatus();

        weaponSelectionPanel.SetActive(false);
    }

    public void CheckWeaponsAvailabilty()
    {
        for (i = 0; i < weaponSelectionPanel.transform.childCount; i++)
        {

        }
    }
}
