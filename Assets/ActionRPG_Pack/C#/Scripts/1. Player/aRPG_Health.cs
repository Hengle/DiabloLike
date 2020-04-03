using UnityEngine;
using System.Collections;

public class aRPG_Health : MonoBehaviour {

    GameObject m;
    aRPG_Master ms;

    // maximum health and mana are in aRPG_CharacterStats script. Below stats are only for scripting operations. You can play with manaRegen however.
    //[HideInInspector] public float health = 100.0f;
    //[HideInInspector] public float mana = 100.0f;
    //float manaRegen = 0.0035f;

    [HideInInspector] 
    public float degenAmount;//退化数量？
    [HideInInspector] 
    public bool playerIsDead = false;

    void Start()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
        
        //health = ms.psStats.maxHealth;
        //mana = ms.psStats.maxMana;

        InvokeManaRegen();
    }
	


	void Update () {
        if (ms.psStats.curAttr.Health <= 0 && ms.pAnimator.GetBool("DeadBool") == false)
        {
            PlayerDies();
        }
	}
    
    public void InvokeManaRegen()
    {
        InvokeRepeating("ManaRegen", 0.03f, 0.03f);
    }
    
    void ManaRegen()
    {
        if (ms.psStats.curAttr.Mana < ms.psStats.baseAttr.Mana)
        {
            //mana = mana + (ms.psStats.maxMana * manaRegen) + (ms.psStats.maxMana * manaRegen * ms.psStats.manaRegenBonus);
            ms.psStats.curAttr.Mana += ms.psStats.curAttr.ManaRegen;
        }
    }
    
    public void SimpleHeal(float healAmount)
    {
        ms.psStats.curAttr.Health += (long)healAmount;
        if (ms.psStats.curAttr.Health > ms.psStats.baseAttr.Health) { ms.psStats.curAttr.Health = ms.psStats.baseAttr.Health; }
	}
    public void ReceiveDamage(damageType dmgType, float dmgAmount)
    {
        //float realDam = dmgAmount;
        //if (dmgType == damageType.Fire)
        //{
        //    realDam = dmgAmount - dmgAmount * fire_res;
        //}
        //if (dmgType == damageType.Magic)
        //{
        //    realDam = dmgAmount - dmgAmount * magic_res;
        //}
        //if (dmgType == damageType.Physical)
        //{
        //    realDam = dmgAmount - dmgAmount * physical_res;
        //}
        //Debug.Log("wrong damage type was passed to ReceiveDamage function");
        ms.psStats.curAttr.Health -= (long)dmgAmount;
    }
    void PlayerDies()
    {
        Destroy(ms.psSkills.DoTClone);
        playerIsDead = true;
        Destroy(ms.pCharacterController, 0.5f);
        ms.pAnimator.SetBool("DeadBool", true);
        ms.psMovement.StopMoveNavAgent();
        ms.psMovement.ResetTriggers();

        ms.psItemPick.enabled = false;
        ms.psMovement.enabled = false;
        ms.psInput.enabled = false;
        ms.psSkills.enabled = false;
        ms.psEvents.enabled = false;
        ms.camMove.enabled = false;

        ms.deathMenu.SetActive(true);
    }


}
