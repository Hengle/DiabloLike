using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Objects for picking up need to be tagged "PickupItem"
// Ammo to pickup for skills needs to be named exactly as UNIQUE_skillName of that skill(Scriptable object in the resources) example: @medkit
// Weapons to pickup need to be named exactly the same as weaponModelName of that item(Scriptable object in the resources) example: @Weapon_gun
// Ammo for weapons to pick up needs to be the same as weaponModelName of that item + "_Ammo", example: @Weapon_gun_Ammo

public class aRPG_ItemPickup : MonoBehaviour {
    GameObject m;
    aRPG_Master ms;

    // # objects that need to be assigned to below variables can be found at the right hand of the player rig.
    public GameObject[] playerWeaponRenderers = new GameObject[4];
    public Dictionary<string, GameObject> WeaponRenderersDictionary = new Dictionary<string, GameObject>();


    void Awake()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

    }

	void Start ()
    {
        WeaponModelsDatabase();
        EnableWeaponRenderer();
    }
    //碰撞到掉落物，捡起，原来的代码里没走到过这里。暂时是点击捡起，注释里边的内容
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (hit.gameObject.tag == "PickupItem")
        //{
        //    // pickup misc
        //    if (hit.gameObject.name == "KeyItem")
        //    {
        //        Destroy(hit.gameObject);
        //        ms.psInventory.key1 = true;
        //    }

        //    // pickup ammo for items
        //    if (hit.gameObject.name.EndsWith("Ammo"))
        //    {
        //        string hitString = hit.gameObject.name.Substring(0, hit.gameObject.name.Length - 5);
        //        if (ms.gsManagement.items_Dictionary.ContainsKey(hitString))
        //        {
        //            for (var i = 0; i < ms.gsManagement.Buttons_WeaponSelect.Length; i++)
        //            {
        //                if (ms.gsManagement.Buttons_WeaponSelect_Script[i].item.weaponModelName == hitString)
        //                {
        //                    ms.gsManagement.Buttons_WeaponSelect_Script[i].item.ammo += 11;
        //                    Destroy(hit.gameObject);
        //                    return;
        //                }
        //            }
        //        }
        //        Debug.Log("There is no item with unique name " + hitString + ". Pickup failed.");
        //        return;
        //    }

        //    // pickup weapons
        //    if (ms.gsManagement.items_Dictionary.ContainsKey(hit.gameObject.name))
        //    {
        //        Debug.Log(ms.gsManagement.Buttons_WeaponSelect.Length);
        //        for (var i = 0; i < ms.gsManagement.Buttons_WeaponSelect.Length; i++)
        //        {
        //            if (ms.gsManagement.Buttons_WeaponSelect_Script[i].item.weaponModelName == hit.gameObject.name)
        //            {
        //                ms.gsManagement.Buttons_WeaponSelect[i].SetActive(true);
        //                Destroy(hit.gameObject);
        //                return;
        //            }
        //        }
        //        Debug.Log("There is no item with unique name " + hit.gameObject.name + ". Pickup failed.");
        //        return;
        //    }
            
        //    // pickup ammo(including medkit) for skills
        //    if (ms.gsManagement.skills_Dictionary.ContainsKey(hit.gameObject.name))
        //    {
        //        ms.gsManagement.skills_Dictionary[hit.gameObject.name].ammo_amount += 3;
        //        Destroy(hit.gameObject);
        //        return;
        //    }
        //    Debug.Log("There is no skill with unique name " + hit.gameObject.name + ". Pickup failed.");
            
        //}
    }


    public void WeaponModelsDatabase()
    {
        for (var i = 0; i < playerWeaponRenderers.Length; i++)
        {
            if (playerWeaponRenderers[i] != null)
            {
                WeaponRenderersDictionary.Add(playerWeaponRenderers[i].name, playerWeaponRenderers[i]);
            }
        }
    }

    public void DisableWeaponRenderer()
    {
        if (ms.psInventory.equippedWeaponModelName != null && ms.psInventory.equippedWeaponModelName != "")
        {
            WeaponRenderersDictionary[ms.psInventory.equippedWeaponModelName].GetComponent<Renderer>().enabled = false;
        }
    }

    public void EnableWeaponRenderer()
    {
        if (ms.psInventory.equippedWeaponModelName != null && ms.psInventory.equippedWeaponModelName != "")
        {
            WeaponRenderersDictionary[ms.psInventory.equippedWeaponModelName].GetComponent<Renderer>().enabled = true;
        }
    }

}
