using UnityEngine;
using System.Collections;


// this script holds variables that should belong to the game inventory. It also holds a function that performs all necessary steps to change weapon.

// below enums are outside of class to easily access then in other scripts.
// weapon category is usefull when you want to introduce additional weapons that look different and have different stats but behave same as present ones. For example a baseball bat, that behaves as a crowbar.
public enum weaponCategories { None, Melee1H, Gun1H, Shotgun, AssaultRifle };
public enum weaponItem { None, Crowbar_BY, Sword_Zombie, Gun_Basic, Shotgun_Basic, AssRif_M4 }

public class aRPG_Inventory : MonoBehaviour {
    
    GameObject m;
    aRPG_Master ms;

    public aRPG_DB_MakeItemSO startingEquippedWeapon;
    internal string equippedWeaponModelName = "";
    internal weaponCategories equippedWeaponCategory = weaponCategories.None;
    GameObject weaponModel;
    
    public bool keyBasement = false;
    public bool key1 = false;
    public bool key2 = false;
    
    
    void Awake()
    {
        m = gameObject;
        ms = m.GetComponent<aRPG_Master>();

        if(startingEquippedWeapon != null)
        {
            equippedWeaponModelName = startingEquippedWeapon.weaponModelName;
            equippedWeaponCategory = startingEquippedWeapon.weaponCategory;
        }
        else { Debug.Log("No starting weapon is selected. Set it up in aRPG_Inventory script"); }

    }

    // # this functions should be called every time you want to change weapon. It is followed by functions that set up weapons renderers and weapon category
    public void ChangeWeapon(aRPG_DB_MakeItemSO weaponToEquip)
    {
        ms.psItemPick.DisableWeaponRenderer();
        startingEquippedWeapon = weaponToEquip;
        equippedWeaponModelName = startingEquippedWeapon.weaponModelName;
        equippedWeaponCategory = startingEquippedWeapon.weaponCategory;
        
        ms.psItemPick.EnableWeaponRenderer();
        ms.pAnimator.SetTrigger("EquipTr");
    }


}
