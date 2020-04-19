using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 此脚本这里实现了技能释放（直接释放，动画驱动的等帧事件），扣蓝操作，没有全部的处理伤害（有的有，有的没）
/// 技能的销毁，有的在这里做了，有的是在技能prefab上挂脚本实现的，比如aRPG_TimedDestroy
/// </summary>
public class aRPG_Skills : MonoBehaviour {
    
    public float meleeRange = 2.7f;
    
    public GameObject gunShot; // for Bullet prefab
    public GameObject gunBulletPoint; // for BulletShootPoint child of player character - from this position a bullet will be shot
    public GameObject spellCastPoint; // same for spells（咒语） - differences are driven by animations, as you will change animations you propably will have to change or add new points to instantiate.
    public GameObject impactEffect; // for BloodExp prefab ，受击效果放置点
    

    // scripting operations variables
    GameObject m;
    aRPG_Master ms;
    Ray rayFire;
    RaycastHit hitFire;

    int i;
    float distanceToMouse;
    RaycastHit raycastHit;
    float cameraHeight;
    Vector3 mousePosition;

    GameObject instantiatedBullet;
    GameObject instantiatedProjectile;
    GameObject instantiatedRay;

    Transform castPoint;
    SkillData projectileSkill;
    internal SkillData lastMeleeSkillUsed;

    aRPG_EnemyStats enemyStatsScript;
    float time;
    // DoT variables
    [HideInInspector] public bool rayContinousCastingManaBreak = false;
    [HideInInspector] public GameObject DoTClone;
    // Melee variables
    [HideInInspector] public Transform meleeTarget;
    [HideInInspector] public aRPG_EnemyStats meleeTargetScript;
    [HideInInspector] public aRPG_EnemyMovement meleeTargetNavScript;
    [HideInInspector] public bool lockedOnMovement = false;
    [HideInInspector] public GameObject target_PreciseMelee;
    [HideInInspector] public GameObject target_FreeTargetMelee;
    [HideInInspector] public float freeTargetMeleeRange;

    [HideInInspector] public GameObject wp;
    Collider[] enemies;
    
    void Start () 
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

        freeTargetMeleeRange = meleeRange;
    }

    // ===== MOUSE AND KEYBOARD =====

    // Every skill has three functions - on button down, on button being held and on button up. In many cases, skill only need execution from one state, in such case leave other functions empty, but please create one to avoid bugs(later in development you will find such scripting practice usefull because it will be easier for you to make changes in skills you created.)
    //每一项技能都有三个功能：按下按钮、按住按钮和向上按钮。在许多情况下，技能只需要从一个状态执行，在这种情况下，将其他函数留空，但请创建一个以避免出现错误（在开发后期，您会发现这样的脚本编写实践非常有用，因为您可以更容易地更改创建的技能）
    // # please note that DoT_Collider skill damage to the enemy is dealt in aRPG_EnemyDoT script attached to an enemy.
    //请注意，对敌人的点阵对撞机技能伤害是用附在敌人身上的aRPG_enemy DoT脚本造成的。

    public void CastDoT_ColliderDown(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0) { return; }
        if (skill.hasLimitedNoOfUses == true) { skill.ammo_amount -= 1; }

        InstantiateDoT_Collider(skill);
    }
    public void CastDoT_ColliderHeld(SkillData skill)
    {
        //是否需要持续施法的
        if (skill.spawnAtCastPoint == false) { return; }
        //弹药射击类的，需要子弹数量
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0)
        {
            CastDoT_ColliderUp(skill);
            rayContinousCastingManaBreak = true;
            return;
        }
        //没蓝了，但是*0.04没看太懂？应该是不够这一帧的消耗了
        if (ms.psStats.curAttr.Mana < skill.manaCostPerSecOrUse*0.04f)
        {
            CastDoT_ColliderUp(skill);
            rayContinousCastingManaBreak = true;
            return; 
        }
        //rayContinousCastingManaBreak参数的实际用处，没理解。应该是没蓝或者没子弹原因打断了，按键没松开，然后蓝又够了。
        if (ms.psStats.curAttr.Mana > skill.manaCostPerSecOrUse && rayContinousCastingManaBreak)
        {
            CastDoT_ColliderDown(skill);
            rayContinousCastingManaBreak = false;
        }
        //施法中可以转向
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            ms.player.transform.LookAt(new Vector3(hitFire.point.x, ms.player.transform.position.y, hitFire.point.z));
        }
    }
    public void CastDoT_ColliderUp(SkillData skill)
    {
        ms.pAnimator.SetBool("rayBool", false);
        if (skill.spawnAtCastPoint) { Destroy(DoTClone); }//持续施法结束，直接在这里删除。非持续的，由各自处理
        ManaDegen_Stop(skill);
    }
    
    public void CastFireballDown(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0) { return; }

        projectileSkill = skill;

        if (ms.psStats.curAttr.Mana < skill.manaCostProjectile) { return; }
        
        ms.psMovement.StopMoveNavAgent();
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        //转向鼠标方向，做动作，，动作帧驱动发射
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
            ms.pAnimator.SetBool("FireballBool", true);
        }
    }
    public void CastFireballHeld(SkillData skill) 
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0)
        {
            CastFireballUp();
            return;
        }

        if (ms.psStats.curAttr.Mana < skill.manaCostProjectile)
        {
            CastFireballUp();
            return;
        }
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            ms.pAnimator.SetBool("FireballBool", true);
            gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
        }
    }
    public void CastFireballUp() 
    {
        ms.pAnimator.SetBool("FireballBool", false);
    }

    public void CastAoEDown(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0) { return; }
        if (skill.hasLimitedNoOfUses == true) { skill.ammo_amount -= 1; }

        if (ms.psStats.curAttr.Mana < skill.AoEmanaCost) { return; }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            AoE(ms.player.tag, skill, new Vector3(hitFire.point.x, hitFire.point.y, hitFire.point.z));
        }
    }
    public void CastAoEHeld() { }
    public void CastAoEUp() { }


    /*Mana Degen*///持续减篮
    public void ManaDegen_Start(SkillData skill)
    {
        skill.manaDegenIsOn = true;
        StartCoroutine(ManaDegeneration_Coroutine(skill));
    }
    //持续减篮停止
    public void ManaDegen_Stop(SkillData skill)
    {
        skill.manaDegenIsOn = false;
    }
    
    IEnumerator ManaDegeneration_Coroutine(SkillData skill)
    {
        float time = Time.time;
        while (time + skill.manaDegenInterval > Time.time && skill.manaDegenIsOn)
        {
            ms.psStats.curAttr.Mana -= (long)(skill.manaCostPerSecOrUse * skill.manaDegenInterval);
            yield return new WaitForSeconds(skill.manaDegenInterval);
            time = Time.time + skill.manaDegenInterval;
        }
    }
    /*Mana Degen*/

    /* Projectile */
    //投掷技能的帧事件触发
    public void EventProjectile()
    {
        if (projectileSkill.hasLimitedNoOfUses == true) { projectileSkill.ammo_amount -= 1; }

        // set projectile instantiation position
        castPoint = spellCastPoint.transform;
        castPoint.localPosition = projectileSkill.CastPointLocalPos;

        // First Projectile
        InstantiateProjectile();
        ms.psStats.curAttr.Mana -= (long)projectileSkill.manaCostProjectile;

        if (projectileSkill.addtionalProjectiles == 0) { return; }
        // Additional Projectiles
        //多个投掷物的处理
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 60.0f, ms.layerTargetingPlaneToMove);
        mousePosition = new Vector3(raycastHit.point.x, ms.player.transform.position.y, raycastHit.point.z);
        distanceToMouse = Vector3.Distance(ms.player.transform.position, mousePosition);
        //鼠标距离越远，扇形角度越小，
        cameraHeight = ms.cam.transform.position.y - ms.player.transform.position.y;
        float projRotation = cameraHeight - distanceToMouse;
        float projRotationNeg = (cameraHeight - distanceToMouse) * -1f;

        for (i = 0; i < projectileSkill.addtionalProjectiles; i++)
        {

            if (i % 2 == 0)
            {
                InstantiateProjectile();

                instantiatedProjectile.transform.Rotate(Vector3.up, projRotation);
                projRotation = projRotation + cameraHeight - distanceToMouse;
            }
            else
            {
                InstantiateProjectile();

                instantiatedProjectile.transform.Rotate(Vector3.up, projRotationNeg);
                projRotationNeg = projRotationNeg + (cameraHeight - distanceToMouse) * -1f;
            }
        }
    }

    void InstantiateProjectile()
    {
        instantiatedProjectile = Instantiate(projectileSkill.GetPrefab(), castPoint.position, castPoint.rotation) as GameObject;
        aRPG_Projectile projectile = instantiatedProjectile.GetComponent<aRPG_Projectile>();
        if (projectile == null)
        {
            projectile = instantiatedProjectile.AddComponent<aRPG_Projectile>();
        }
        projectile.GetObject(m, projectileSkill, ms.player.tag);
    }

    /// <summary>
    /// 投射物结束后，执行接着的技能
    /// 只能是AOE、DOT
    /// </summary>
    /// <param name="casterTag"></param>
    /// <param name="linkedSkill"></param>
    /// <param name="contactPosition"></param>
    public void ExecuteLink(string casterTag, int linkedId, Vector3 contactPosition)
    {
        SkillData linkedSkill = DataManager.Instance.GetSkill(linkedId);
        if (linkedSkill.skillArchetype == (int)archetype.AoE)
        {
            AoE(casterTag , linkedSkill, contactPosition);
        }
        if(linkedSkill.skillArchetype == (int)archetype.DoT)
        {
            InstantiateDoT_Collider_fromLink(casterTag, linkedSkill, contactPosition); 
        }
        // debug note on linking
        if(linkedSkill.skillArchetype != (int)archetype.DoT && linkedSkill.skillArchetype != (int)archetype.AoE)
        {
            Debug.Log("NOTE: skill " + linkedSkill.UNIQUE_skillName + " cannot be linked");
        }
    }
    /* Projectile */

    
    /* DoT Collider */
    //DoT技能直接生成的，不是动画驱动的
    public void InstantiateDoT_Collider(SkillData skill)
    {
        if (ms.psStats.curAttr.Mana < skill.manaCostPerSecOrUse) { return; }
        ms.psMovement.StopMoveNavAgent();
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            ms.player.transform.LookAt(new Vector3(hitFire.point.x, ms.player.transform.position.y, hitFire.point.z));
            if (DoTClone != null && skill.spawnAtCastPoint) { Destroy(DoTClone); }
            if (skill.spawnAtCastPoint)
            {
                DoTClone = Instantiate(skill.GetPrefab(), spellCastPoint.transform.position, spellCastPoint.transform.rotation) as GameObject;
                DoTClone.transform.parent = ms.player.transform;
                ManaDegen_Start(skill);
            }
            else
            {
                DoTClone = Instantiate(skill.GetPrefab(), new Vector3(hitFire.point.x, ms.player.transform.position.y + 2f, hitFire.point.z), Quaternion.identity) as GameObject;
                DoTClone.transform.LookAt(new Vector3(ms.player.transform.position.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z));
                ms.psStats.curAttr.Mana -= (long)skill.manaCostPerSecOrUse;
            }
            //dot技能脚本初始化
            DoTClone.GetComponent<aRPG_DoT>().SendObjects(ms, skill, ms.player.tag);
        }
        ms.pAnimator.SetBool("rayBool", true);
    }
    //被链接的DoT生成
    public void InstantiateDoT_Collider_fromLink(string casterTag, SkillData skill, Vector3 contactPosition)
    {
        GameObject DoT_FromLink = Instantiate(skill.GetPrefab(), contactPosition, Quaternion.identity) as GameObject;
        DoT_FromLink.GetComponent<aRPG_DoT>().SendObjects(ms, skill, casterTag);

    }
    /* DoT Collider */


    /* Area Damage */
    //处理AoE技能伤害， 但是没有处理技能消失
    public void AoE(string casterTag, SkillData skill, Vector3 center)
    {
        if (skill.AoEradius > 0f)
        {
            GameObject aoeGo = null;
            //看特效有没有角度
            if ( (skill.GetPrefab() as GameObject).transform.rotation.eulerAngles.magnitude > 10)
            {
                aoeGo = Instantiate(skill.GetPrefab(), center, (skill.GetPrefab() as GameObject).transform.rotation) as GameObject;
            }
            else
            {
                aoeGo = Instantiate(skill.GetPrefab(), center, Quaternion.identity) as GameObject;
            }
            aRPG_AoE aoe = aoeGo.GetComponent<aRPG_AoE>();
            if(aoe == null)
            {
                aoe = aoeGo.AddComponent<aRPG_AoE>();
            }
            aoe.SendObjects(ms, skill, casterTag);
            //if (casterTag == "Player") { ms.psStats.curAttr.Mana -= skill.AoEmanaCost; }
            //StartCoroutine(AoE_Damage(casterTag, skill, center));

        }
    }
    //把AoE逻辑转移到aRPG_AoE脚本中
    //IEnumerator AoE_Damage(string casterTag, SkillData skill, Vector3 center)
    //{
    //    yield return new WaitForSeconds(skill.AoEdamageDelay);

    //    if (casterTag == "Player")
    //    {
    //        Collider[] hitColliders = Physics.OverlapSphere(center, skill.AoEradius, ms.layerEnemies);
    //        int i = 0;
    //        while (i < hitColliders.Length)
    //        {
    //            if (hitColliders[i].tag == "enemy")
    //            {
    //                hitColliders[i].gameObject.GetComponent<aRPG_EnemyMovement>().DamageTaken();
    //                enemyStatsScript = hitColliders[i].gameObject.GetComponent<aRPG_EnemyStats>();
    //                enemyStatsScript.currentHealth -= enemyStatsScript.ReceiveDamage(skill.AoEdamageType, skill.AoEdamage);
    //            }
    //            i++;
    //        }
    //    }

    //    if(casterTag == "enemy")
    //    {
    //        Collider[] hitColliders = Physics.OverlapSphere(center, skill.AoEradius, ms.layerPlayer);
    //        int i = 0;
    //        while (i < hitColliders.Length)
    //        {
    //            if (hitColliders[i].tag == "Player")
    //            {
    //                ms.psHealth.health -= skill.AoEdamage;
    //            }
    //            i++;
    //        }
    //    }

    //}
    /* Area Damage */
    //以上是技能，法师居多，后边是枪和近战的技能，当然都是可以混用的，看怎么设计，但是后边的好像没有在示例中用上。或者应该是换上武器以后才行

    /*_________Guns & Bullets_______*/


    // (Gun)
    public void ShootGunDown(){ }
    // aiming causes player character to always look at enemy when a cursor is over enemy. That is a slight change, takes some time spent playing to notice, but gameplay-wise this may be important feature.
    public void ShootGunHeld(bool aiming)
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { return; }

        ms.psMovement.StopMoveNavAgent();
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (aiming) 
        {
            if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskShootEnemies))
            {
                if (hitFire.transform.tag == "enemy")
                {
                    ms.psMovement.LookAtThisEnemy(hitFire.transform.gameObject);
                    ms.pAnimator.SetTrigger("GunFireTr");
                }
                if (hitFire.transform.tag == "targetingPlane")
                {
                    gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
                    ms.pAnimator.SetTrigger("GunFireTr");
                }
            }
        }
        else
        {
            if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
            {
                gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
                ms.pAnimator.SetTrigger("GunFireTr");

            }
        }
    }
    public void ShootGunUp(){ms.pAnimator.ResetTrigger("GunFireTr");}

    // (Shotgun)
    public void ShootShotgunDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        ms.psMovement.StopMoveNavAgent();
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            if (hitFire.transform.tag == "targetingPlane")
            {
                gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
                ms.pAnimator.SetBool("ShotgunBool", true);
            }
        }
    }
    public void ShootShotgunHeld() 
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            if (hitFire.transform.tag == "targetingPlane")
            {
                gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
            }
        }
    }
    public void ShootShotgunUp()
    {
        ms.pAnimator.SetBool("ShotgunBool", false);
    }

    // (Rifle)
    public void ShootRifleDown() {}
    public void ShootRifleHeld() 
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { return; }

        ms.psMovement.StopMoveNavAgent();
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToShoot))
        {
            gameObject.transform.LookAt(new Vector3(hitFire.point.x, gameObject.transform.position.y, hitFire.point.z));
            ms.pAnimator.SetTrigger("RifleFireTr");

        }
    }
    public void ShootRifleUp() { ms.pAnimator.ResetTrigger("RifleFireTr"); }

    /*_________Guns & Bullets_______*/


    /*_________Melee________________*/
    // (MeleeOld)
    /*
    public void CrowbarDown() {}
    public void CrowbarHeld()
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, maskCombinedMoveEnemies))
        {
            if (hitFire.transform.tag == "enemy")
            {
                var fwd = new Vector3(hitFire.transform.position.x - transform.position.x, shotOrigin.transform.TransformDirection(Vector3.forward).y, hitFire.transform.position.z - transform.position.z);
                if (Physics.Raycast(shotOrigin.transform.position, fwd, out hitFireLine, meleeRange, layerMaskEnemies))
                {
                    lockedOnEnemy = true;

                    meleeTarget = hitFireLine.transform;
                    meleeTargetScript = meleeTarget.GetComponent<aRPG_EnemyStats>();
                    meleeTargetNavScript = meleeTarget.GetComponent<aRPG_EnemyMovement>();
                    LookAtEnemy();
                    playerAnimator.SetTrigger("WeaponTr");
                    playerMoveScript.pendingEnemyMeleeAtack = false;

                }
                else 
                {
                    if (lockedOnEnemy == false)
                    {
                        Debug.Log("not in melee range");
                        // follow targeted enemy
                        playerMoveScript.trackingEnemy = true;
                        playerMoveScript.trackingEnemyObj = hitFire.transform.parent.transform;
                        playerMoveScript.SetDestinationCustom(hitFire.transform.parent.transform.position);
                        playerMoveScript.pendingEnemyMeleeAtack = true;
                    }
                }
            }
            if (hitFire.transform.tag == "targetingPlane" && lockedOnEnemy == false)
            {
                // move to the pointer on plane
                playerMoveScript.SetDestinationCustom(hitFire.point);
            }
        }
    }
    public void CrowbarUp()
    {
        playerAnimator.ResetTrigger("WeaponTr");
        lockedOnEnemy = false;
    }
    */

    // # below you have 3 types of approaches to the melee combat:

    // # in PreciseMelee player character will react to only what is under the cursor.
    // # in PreciseWithLockOn player character will react to only what was under the cursor when button was pressed DOWN - moving mouse while button is HELD wont change reaction.
    // # in FreeTargetMelee player character will act like in PreciseMelee but if an enemy is in range player character will auto-attack it.

    // which type of skill to use is a game design decision - I prefer having PreciseWithLockOn under leftMouseButton and FreeTargetMelee under any other button.
    //#下面有3种近战方法：



    //#在PreciseMelee中，玩家角色只对光标下的内容做出反应。

    //在PreciseWithLockOn中，玩家角色只会对按下按钮时光标下的内容做出反应-按住按钮时移动鼠标不会改变反应。

    //#在FreeTargetMelee中，玩家角色的行为类似于在PreciseMelee中，但如果敌人在射程内，玩家角色将自动攻击它。



    //使用哪种技能是一个游戏设计决定-我更喜欢在左鼠标按钮下使用PreciseWithLockOn，在任何其他按钮下使用FreeTargetMelee。

    // (PreciseMelee)
    public void PreciseMeleeDown() {ms.psEvents.meleeAttackTypeCode = 0;}
    public void PreciseMeleeHeld() 
    {
        if (!ms.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemies))
            {
                if (hitFire.transform.tag == "enemy")
                {
                    target_PreciseMelee = hitFire.transform.gameObject.transform.parent.gameObject;
                    if (Vector3.Distance(target_PreciseMelee.transform.position, gameObject.transform.position) <= meleeRange)
                    {
                        SimpleMeleeAttack(target_PreciseMelee.transform);
                    }
                    else
                    {
                        ms.psMovement.trackingEnemy = true;
                        ms.psMovement.trackingEnemyObj = target_PreciseMelee.transform;
                        ms.psMovement.SetDestinationCustom(target_PreciseMelee.transform.position);
                        ms.psMovement.pendingEnemyMeleeAtack = true;
                    }
                }
                if (hitFire.transform.tag == "targetingPlane")
                {
                    ms.psMovement.SetDestinationCustom(hitFire.point);
                }
            }
        }
    }
    public void PreciseMeleeUp() 
    {
        ms.pAnimator.ResetTrigger("WeaponTr");
        target_PreciseMelee = null;
    }


    // (PreciseWithLockOn)
    /// <summary>
    /// 精确锁定近战，单目标攻击meleeAttackTypeCode = 0;
    /// </summary>
    public void PreciseWithLockOnMeleeDown() 
    {
        ms.psEvents.meleeAttackTypeCode = 0;
        if (!ms.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemies))
            {
                if (hitFire.transform.tag == "enemy")
                {
                    target_PreciseMelee = hitFire.transform.gameObject.transform.parent.gameObject;
                    if (Vector3.Distance(target_PreciseMelee.transform.position, gameObject.transform.position) <= meleeRange)
                    {
                        SimpleMeleeAttack(target_PreciseMelee.transform);
                    }
                    else
                    {
                        ms.psMovement.trackingEnemy = true;
                        ms.psMovement.trackingEnemyObj = target_PreciseMelee.transform;
                        ms.psMovement.SetDestinationCustom(target_PreciseMelee.transform.position);
                        ms.psMovement.pendingEnemyMeleeAtack = true;
                    }
                }
                if (hitFire.transform.tag == "targetingPlane")
                {
                    lockedOnMovement = true;
                    ms.psMovement.SetDestinationCustom(hitFire.point);
                }
            }
        }
    }
    public void PreciseWithLockOnMeleeHeld()
    {
        if (!ms.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            if (target_PreciseMelee != null && lockedOnMovement == false)
            {
                rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemies))
                {
                    if (Vector3.Distance(target_PreciseMelee.transform.position, gameObject.transform.position) <= meleeRange)
                    {
                        SimpleMeleeAttack(target_PreciseMelee.transform);
                    }
                    else
                    {
                        ms.psMovement.trackingEnemy = true;
                        ms.psMovement.trackingEnemyObj = target_PreciseMelee.transform;
                        ms.psMovement.SetDestinationCustom(target_PreciseMelee.transform.position);
                        ms.psMovement.pendingEnemyMeleeAtack = true;
                    }
                }
            }
            if (target_PreciseMelee == null && lockedOnMovement == true)
            {
                rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemies))
                {
                    ms.psMovement.SetDestinationCustom(hitFire.point);
                }
            }
        }
    }
    public void PreciseWithLockOnMeleeUp() 
    {
        ms.pAnimator.ResetTrigger("WeaponTr");
        target_PreciseMelee = null;
        lockedOnMovement = false;
    }

    // (FreeTargetMelee)
    public void FreeTargetMeleeDown(){ ms.psEvents.meleeAttackTypeCode = 0; }

    public void FreeTargetMeleeHeld()
    {
        if (!ms.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemies))
            {
                if (hitFire.transform.tag == "enemy")
                {
                    target_PreciseMelee = hitFire.transform.gameObject.transform.parent.gameObject;
                    if (Vector3.Distance(target_PreciseMelee.transform.position, gameObject.transform.position) <= meleeRange)
                    {
                        SimpleMeleeAttack(target_PreciseMelee.transform);
                    }
                    else
                    {
                        ms.psMovement.trackingEnemy = true;
                        ms.psMovement.trackingEnemyObj = target_PreciseMelee.transform;
                        ms.psMovement.SetDestinationCustom(target_PreciseMelee.transform.position);
                        ms.psMovement.pendingEnemyMeleeAtack = true;
                    }
                }
                else
                {

                    if (target_FreeTargetMelee == null)
                    {
                        if (!Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), freeTargetMeleeRange, ms.layerEnemies))
                        {
                            //ms.pMeleeColliderScript.collider.radius = meleeRange * 0.4f;
                            MoveSkillHeld();
                            //ms.pMeleeCollider.SetActive(true);
                            freeTargetMeleeRange = meleeRange * 0.5f;
                            return;
                        }
                        else
                        {
                            freeTargetMeleeRange = meleeRange;
                            enemies = Physics.OverlapSphere(ms.player.transform.position, freeTargetMeleeRange, ms.layerEnemies);
                            if (enemies != null && enemies.Length > 0) 
                            {
                                target_FreeTargetMelee = enemies[0].gameObject;
                            }
                            else { return; }
                        }
                    }
                    if (Vector3.Distance(gameObject.transform.position, target_FreeTargetMelee.transform.position) > meleeRange)
                    {
                        target_FreeTargetMelee = null;
                        return;
                    }

                    ms.psMovement.StopMoveNavAgent();
                    SimpleMeleeAttack(target_FreeTargetMelee.transform);

                }
            }
        }
    }

    public void FreeTargetMeleeUp()
    {
        target_FreeTargetMelee = null;
        target_PreciseMelee = null;
        ms.pAnimator.ResetTrigger("WeaponTr");
    }
    /// <summary>
    /// 近战技能释放接口，meleeAttackTypeCode = 2;使用技能计算范围和伤害
    /// </summary>
    /// <param name="skill"></param>
    public void MeleeSweep_Down(SkillData skill)
    {
        lastMeleeSkillUsed = skill;
        ms.psEvents.meleeAttackTypeCode = 2;
    }

    public void MeleeSweep_Held(SkillData skill)
    {
        if (!ms.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Melee"))
        {
            rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemies))
            {
                if(Vector3.Distance(ms.player.transform.position, new Vector3(hitFire.point.x, ms.player.transform.position.y, hitFire.point.z)) > (skill.arcLength + ms.pNavAgent.stoppingDistance)*1.2f)
                {
                    ms.psMovement.SetDestinationCustom(hitFire.point);
                    return;
                }
            }
            ms.psMovement.LookAtPoint(hitFire.point);

            ms.psMovement.StopMoveNavAgent();
            ms.pAnimator.SetTrigger("WeaponTr");
        }
    }

    public void MeleeSweep_Up()
    {
        ms.pAnimator.ResetTrigger("WeaponTr");
    }

    // # simple melee attack used by some scripts and functions, it is not called directly by input.
    public void SimpleMeleeAttack(Transform meleeTargetToHit)
    {
        meleeTarget = meleeTargetToHit;
        meleeTargetScript = meleeTarget.GetComponent<aRPG_EnemyStats>();
        meleeTargetNavScript = meleeTarget.GetComponent<aRPG_EnemyMovement>();
        if (meleeTargetScript.isDead == false)
        {
            ms.psMovement.LookAtThisEnemy(meleeTargetToHit.gameObject);
            ms.pAnimator.SetTrigger("WeaponTr");
        }
        ms.psMovement.pendingEnemyMeleeAtack = false;

    }

    /*_________Melee________________*/


    /*_________Other________________*/

    /*MoveSkill*/
    public void MoveSkillDown() { }
    public void MoveSkillHeld() 
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToMove))
        {
            ms.psMovement.trackingEnemy = false; 
            ms.psMovement.pendingEnemyMeleeAtack = false;
            ms.psMovement.SetDestinationCustom(hitFire.point);
        }
        
    }
    public void MoveSkillUp() { }
    /*MoveSkill*/

    /*Consumable*/
    public void Consumable_Down(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0) { return; }
        if (skill.hasLimitedNoOfUses == true) { skill.ammo_amount -= 1; }

        Consumable(skill);
    }

    public void Consumable(SkillData skill)
    {
        if (skill.delayIsOn) { return; }
        if (skill.sttChangeUseDelay > 0f)
        {
            StartCoroutine("ConsumableCoroutine", skill);
        }

        Consumable_Variants(skill);
    }
    /// <summary>
    /// 消耗品变化，影响属性的药品。回血回蓝。加体力什么的，到时间又变回原属性
    /// </summary>
    /// <param name="skill"></param>
    void Consumable_Variants(SkillData skill)
    {
        if (skill.sttChange == sttChange.Health)
        {
            MedkitHeal(skill);
        }
        if (skill.sttChange == sttChange.Mana)
        {
            ManaPotion(skill);
        }
        
        if (skill.sttChange == sttChange.Strenght)
        {
            //ms.psStats.strenght += skill.sttChangeAmount;
            // run Cooldown if has Delay;
            if (skill.sttChangeDuration > 0)
            {
                StartCoroutine(RemoveConsumableBuff(skill));
            }
        }

        if (skill.sttChange == sttChange.Int)
        {
            //ms.psStats.intelligence += skill.sttChangeAmount;
            // run Cooldown if has Delay;
            if (skill.sttChangeDuration > 0)
            {
                StartCoroutine(RemoveConsumableBuff(skill));
            }
        }
    }

    public void MedkitHeal(SkillData skill)
    {
        ms.psStats.curAttr.Health += (long)skill.sttChangeAmount;
        if (ms.psStats.curAttr.Health > ms.psStats.baseAttr.Health) { ms.psStats.curAttr.Health = ms.psStats.baseAttr.Health; }
    }

    public void ManaPotion(SkillData skill)
    {
        ms.psStats.curAttr.Mana += (long)skill.sttChangeAmount;
        if (ms.psStats.curAttr.Mana > ms.psStats.baseAttr.Mana) { ms.psStats.curAttr.Mana = ms.psStats.baseAttr.Mana; }
    }

    IEnumerator RemoveConsumableBuff(SkillData skill)
    {
        yield return new WaitForSeconds((float)skill.sttChangeDuration);
        
        //if (skill.sttChange == sttChange.Strenght) { ms.psStats.strenght -= skill.sttChangeAmount; }
        //if (skill.sttChange == sttChange.Int) { ms.psStats.intelligence -= skill.sttChangeAmount; }

        // recalculate derived stats
    }

    IEnumerator ConsumableCoroutine(SkillData skill)
    {
        skill.delayIsOn = true;
        skill.currentSpriteFill = 0f;
        
        StartCoroutine("SpriteFillCoroutine", skill);
        yield return new WaitForSeconds((float)skill.sttChangeUseDelay);

        skill.currentSpriteFill = 1f;
        skill.delayIsOn = false;
    }

    IEnumerator SpriteFillCoroutine(SkillData skill)
    {
        float time = Time.time;
        while (time + skill.spriteFillSpeed > Time.time && skill.currentSpriteFill < 1f)
        {
            skill.currentSpriteFill = skill.currentSpriteFill + (1 / (float)skill.sttChangeUseDelay * skill.spriteFillSpeed);
            yield return new WaitForSeconds(skill.spriteFillSpeed);
            time = Time.time + skill.spriteFillSpeed;
        }
    }
    /*Consumable*/
    

    // OpenDoor

    // # this is designed to be used as a "skill", it is executed by inputMouse skill when a pointer is over doors.
    public void PlayerOpensDoor()
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerInteractiveObject))
        {
            Transform doorPos = hitFire.transform.parent;
            ms.psMovement.SetDestinationCustom(doorPos.transform.position);
            ms.psMovement.door = hitFire.transform.gameObject;
            ms.psMovement.pendingOpenDoor = true;
        }
    }

    // # this is to open/close Waypoint menu.
    public void WaypointClick(GameObject waypoint)
    {
        wp = waypoint;
        if (Vector3.Distance(waypoint.transform.position, ms.player.transform.position) > meleeRange)
        {
            ms.psMovement.SetDestinationCustom(waypoint.transform.position);
        }
        else
        {
            if (ms.waypointMenu.activeInHierarchy)
            {
                ms.waypointMenu.SetActive(false);
            }
            else
            {
                ms.waypointMenu.SetActive(true);
            }
        }
    }

    /*_________Other________________*/

    /*========= MOBILE ===========*/

    public void Mobile_DoT_Collider_Down(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0) { return; }
        if (skill.hasLimitedNoOfUses == true) { skill.ammo_amount -= 1; }

        Mobile_InstantiateDoT_Collider(skill);
    }
    public void Mobile_DoT_Collider_Held(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0)
        {
            Mobile_DoT_Collider_Up(skill);
            rayContinousCastingManaBreak = true;
            return;
        }

        if (ms.psStats.curAttr.Mana < skill.manaCostPerSecOrUse * 0.04f)
        {
            Mobile_DoT_Collider_Up(skill);
            rayContinousCastingManaBreak = true;
            return;
        }
        if (ms.psStats.curAttr.Mana > skill.manaCostPerSecOrUse && rayContinousCastingManaBreak)
        {
            Mobile_DoT_Collider_Down(skill);
            rayContinousCastingManaBreak = false;
        }

    }
    public void Mobile_DoT_Collider_Up(SkillData skill)
    {
        ms.pAnimator.SetBool("rayBool", false);
        Destroy(DoTClone);
        ManaDegen_Stop(skill);
    }

    /* DoT Collider */
    public void Mobile_InstantiateDoT_Collider(SkillData skill)
    {
        if (ms.psStats.curAttr.Mana < skill.manaCostPerSecOrUse) { return; }

        //block wasd input

        if (DoTClone != null && skill.spawnAtCastPoint) { Destroy(DoTClone); }

        if (skill.spawnAtCastPoint)
        {
            DoTClone = Instantiate(skill.GetPrefab(), spellCastPoint.transform.position, spellCastPoint.transform.rotation) as GameObject;
            DoTClone.transform.parent = ms.player.transform;
            ManaDegen_Start(skill);
        }
        else
        {
            Debug.Log("NOTE: In mobile/wasd input mode DoT_Collider skills can be spawned only at cast point, fix: mark spawnAtCastPoint true in skill scriptableObject ");
        }
        DoTClone.GetComponent<aRPG_DoT>().SendObjects(ms, skill, ms.player.tag);

        ms.pAnimator.SetBool("rayBool", true);
    }
    /* DoT Collider */

    /* (Fireball) */
    public void Mobile_CastFireball_Down(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0) { return; }

        if (ms.psStats.curAttr.Mana < skill.manaCostProjectile) { return; }

        projectileSkill = skill;
        ms.pAnimator.SetBool("FireballBool", true);
    }
    public void Mobile_CastFireball_Held(SkillData skill)
    {
        if (skill.hasLimitedNoOfUses == true && skill.ammo_amount <= 0)
        {
            Mobile_CastFireball_Up();
            return;
        }

        if (ms.psStats.curAttr.Mana < skill.manaCostProjectile)
        {
            Mobile_CastFireball_Up();
            return;
        }
        ms.pAnimator.SetBool("FireballBool", true);
    }
    public void Mobile_CastFireball_Up() { ms.pAnimator.SetBool("FireballBool", false); }

    /* (Gun) */
    public void Mobile_ShootGun_Down() {  }
    // aiming causes player character to always look at enemy when a cursor is over enemy. That is a slight change, takes some time spent playing to notice, but gameplay-wise this may be important feature.
    public void Mobile_ShootGun_Held() { ms.pAnimator.SetTrigger("GunFireTr"); }
    public void Mobile_ShootGun_Up() { ms.pAnimator.ResetTrigger("GunFireTr"); }

    /* (Shotgun) */
    public void Mobile_ShootShotgun_Down() { ms.pAnimator.SetBool("ShotgunBool", true); }
    public void Mobile_ShootShotgun_Held() { }
    public void Mobile_ShootShotgun_Up() { ms.pAnimator.SetBool("ShotgunBool", false); }

    /* (Rifle) */
    public void Mobile_ShootRifle_Down() { }
    public void Mobile_ShootRifle_Held(){ ms.pAnimator.SetTrigger("RifleFireTr"); }
    public void Mobile_ShootRifle_Up() { ms.pAnimator.ResetTrigger("RifleFireTr"); }
    
    /* (Melee) */
    public void Mobile_Melee_Down() { ms.psEvents.meleeAttackTypeCode = 1; }
    public void Mobile_Melee_Held() { ms.pAnimator.SetTrigger("WeaponTr");  }
    public void Mobile_Melee_Up() { ms.pAnimator.ResetTrigger("WeaponTr"); }

    /*========= MOBILE ===========*/

}
