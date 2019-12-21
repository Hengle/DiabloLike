using UnityEngine;
using System.Collections;

public class aRPG_DB_MakeItemSO : ScriptableObject {
    int ID;
    public Sprite weaponIcon;
    // Name of the GameObject weapon model located at the right hand of the player - it start with "@Weapon_"
    public string weaponModelName;
    public weaponCategories weaponCategory;
    public float damage = 2;
    public damageType damageType;
    public bool hasLimitedNoOfUses = false;
    public int ammo;

}
