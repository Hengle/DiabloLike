using UnityEngine;
using System.Collections;

// # this script needs to be attached to player because player animations use events from this script.

public class aRPG_PlayerAnimatorEvents : MonoBehaviour {

    GameObject m;
    aRPG_Master ms;

    int i;
    float distanceToMouse;
    RaycastHit raycastHit;
    float cameraHeight;
    Vector3 mousePosition;

    [HideInInspector] public int meleeAttackTypeCode = 1;

    GameObject instantiatedBullet;

    void Start()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
    }
    
    void EventPlayerHandWeapon()
    {
        // mouse melee
        if (meleeAttackTypeCode == 0)
        {
            ms.psSkills.meleeTargetScript.currentHealth -= ms.psSkills.meleeTargetScript.ReceiveDamage(ms.psInventory.startingEquippedWeapon.damageType, ms.psInventory.startingEquippedWeapon.damage);
            ms.psSkills.meleeTargetNavScript.DamageTaken();

            var impactEffectposition = ms.psSkills.meleeTarget.transform.Find("shotEffectFront");
            Instantiate(ms.psSkills.impactEffect, impactEffectposition.transform.position, impactEffectposition.transform.rotation);
        }
        // mobile melee
        if (meleeAttackTypeCode == 1)
        {
            Vector3 p1 = new Vector3(ms.player.transform.position.x + ms.psStats.meleeArcSweep_arcWidth * ms.player.transform.right.x + 0.05f * ms.player.transform.forward.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z + 0.05f * ms.player.transform.forward.z + ms.psStats.meleeArcSweep_arcWidth * ms.player.transform.right.z);
            Vector3 p2 = new Vector3(ms.player.transform.position.x - ms.psStats.meleeArcSweep_arcWidth * ms.player.transform.right.x + 0.05f * ms.player.transform.forward.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z + 0.05f * ms.player.transform.forward.z - ms.psStats.meleeArcSweep_arcWidth * ms.player.transform.right.z);
            RaycastHit[] enemies = Physics.CapsuleCastAll(p1, p2, 0.6f, ms.player.transform.forward, ms.psStats.meleeArcSweep_arcLength, ms.layerEnemies);

            foreach (RaycastHit enemy in enemies)
            {
                ms.psSkills.meleeTarget = enemy.collider.gameObject.transform;
                ms.psSkills.meleeTargetScript = enemy.collider.gameObject.GetComponent<aRPG_EnemyStats>();
                ms.psSkills.meleeTargetNavScript = enemy.collider.gameObject.GetComponent<aRPG_EnemyMovement>();
                if (ms.psSkills.meleeTargetScript.isDead == false)
                {
                    ms.psSkills.meleeTargetScript.currentHealth -= ms.psSkills.meleeTargetScript.ReceiveDamage(ms.psInventory.startingEquippedWeapon.damageType, ms.psInventory.startingEquippedWeapon.damage);
                    ms.psSkills.meleeTargetNavScript.DamageTaken();

                    var impactEffectposition = ms.psSkills.meleeTarget.transform.Find("shotEffectFront");
                    Instantiate(ms.psSkills.impactEffect, impactEffectposition.transform.position, impactEffectposition.transform.rotation);
                }
            }
        }
        // melee sweep skill
        if (meleeAttackTypeCode == 2)
        {
            Vector3 p1 = new Vector3(ms.player.transform.position.x + ms.psSkills.lastMeleeSkillUsed.arcWidth * ms.player.transform.right.x + 0.05f * ms.player.transform.forward.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z + 0.05f * ms.player.transform.forward.z + ms.psSkills.lastMeleeSkillUsed.arcWidth * ms.player.transform.right.z);
            Vector3 p2 = new Vector3(ms.player.transform.position.x - ms.psSkills.lastMeleeSkillUsed.arcWidth * ms.player.transform.right.x + 0.05f * ms.player.transform.forward.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z + 0.05f * ms.player.transform.forward.z - ms.psSkills.lastMeleeSkillUsed.arcWidth * ms.player.transform.right.z);
            RaycastHit[] enemies = Physics.CapsuleCastAll(p1, p2, 0.6f, ms.player.transform.forward, ms.psSkills.lastMeleeSkillUsed.arcLength, ms.layerEnemies);

            foreach (RaycastHit enemy in enemies)
            {
                ms.psSkills.meleeTarget = enemy.collider.gameObject.transform;
                ms.psSkills.meleeTargetScript = enemy.collider.gameObject.GetComponent<aRPG_EnemyStats>();
                ms.psSkills.meleeTargetNavScript = enemy.collider.gameObject.GetComponent<aRPG_EnemyMovement>();
                if (ms.psSkills.meleeTargetScript.isDead == false)
                {
                    ms.psSkills.meleeTargetScript.currentHealth -= ms.psSkills.meleeTargetScript.ReceiveDamage(ms.psInventory.startingEquippedWeapon.damageType, ms.psInventory.startingEquippedWeapon.damage*ms.psSkills.lastMeleeSkillUsed.damageModifierPercent);
                    ms.psSkills.meleeTargetNavScript.DamageTaken();

                    var impactEffectposition = ms.psSkills.meleeTarget.transform.Find("shotEffectFront");
                    Instantiate(ms.psSkills.impactEffect, impactEffectposition.transform.position, impactEffectposition.transform.rotation);
                }
            }
        }
    }

    void EventPlayerShoot()
    {
        if (ms.psInventory.startingEquippedWeapon.ammo > 0)
        {
            instantiatedBullet = Instantiate(ms.psSkills.gunShot, ms.psSkills.gunBulletPoint.transform.position, ms.psSkills.gunBulletPoint.transform.rotation) as GameObject;
            instantiatedBullet.GetComponent<aRPG_BulletBehaviore>().GetObject(m);
            ms.psInventory.startingEquippedWeapon.ammo--;
        }
    }

    void ResetGunTr()
    {ms.pAnimator.ResetTrigger("GunFireTr");}
    void ResetShotgunTr()
    { ms.pAnimator.ResetTrigger("ShotgunFireTr"); }
    void ResetRifleTr()
    { ms.pAnimator.ResetTrigger("RifleFireTr"); }

    void EventCastFireball()
    {
        ms.psSkills.EventProjectile();
    }
    

}
