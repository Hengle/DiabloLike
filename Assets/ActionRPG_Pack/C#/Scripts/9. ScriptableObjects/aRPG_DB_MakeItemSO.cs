using UnityEngine;
using System.Collections;

public class aRPG_DB_MakeItemSO : ScriptableObject {
    public int id;
    public Sprite weaponIcon;
    // Name of the GameObject weapon model located at the right hand of the player - it start with "@Weapon_"
    public string weaponModelName;
    public weaponCategories weaponCategory;
    public float damage = 2;
    public int damageType;
    public bool hasLimitedNoOfUses = false;
    public int ammo;

}
