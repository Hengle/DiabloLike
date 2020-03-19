using UnityEngine;
using System.Collections;

public class aRPG_EnemyMovement : MonoBehaviour {
    GameObject m;
    aRPG_Master ms;

    UnityEngine.AI.NavMeshAgent eAgent;
    aRPG_EnemyStats esStats;
    Transform triggerStuffObj;
    Animator eAnimator;
    [HideInInspector] public bool playerInRange = false;
    bool playerDead = false;
    RaycastHit hitEnemy;

    int layerPlayer = 1 << 20;
    int layerObstacle = 1 << 24;
    int layerInterObj = 1 << 25;
    int maskCombinedEnemySight = 0;

    float angle;
    public float rotationSpeed = 5.1f;
    public float deadZone = 0.1f;
    
    GameObject spellCastPoint;
    float distanceToKeep;
    float outOfMeleeRange;
    bool aiReady = false;
    bool getAwayTimeRunning = false;
    bool canGetAway = true;
    bool customDestRangedSet = false;

    //==============
    public aRPG_DB_MakeAISO ai;
    GameObject dotColliderClone;
    //==============

    void Start ()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

        spellCastPoint = transform.Find("SpellCastPoint").gameObject;
	    eAnimator = gameObject.GetComponent<Animator>();
	    eAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	    esStats = gameObject.GetComponent<aRPG_EnemyStats>();
        eAgent.updateRotation = false;
        eAgent.updatePosition = false;
        eAgent.stoppingDistance = ai.meleeAttackRange *0.8f;
        distanceToKeep = ai.rangedRange - eAgent.stoppingDistance;
        outOfMeleeRange = ms.psSkills.meleeRange;
	    triggerStuffObj = gameObject.transform.Find("ColliderSightRange");
	    maskCombinedEnemySight = layerPlayer | layerObstacle | layerInterObj;
	}
	
	void Update ()
    {
        //Debug.DrawLine(transform.position, eAgent.destination);

        // Stop when Player is dead
        if (ms.psHealth.playerIsDead == true)
        {
            eAnimator.SetFloat("Move", 0.0f);
            return;
        }

        // Move NavMesh Agent along with the player
        Vector3 worldDeltaPosition = eAgent.nextPosition - transform.position;
        if (worldDeltaPosition.magnitude > eAgent.radius)
        {
            eAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
        }

        // Custom LookAt implementation for navigation
        angle = FindAngle(transform.forward, eAgent.desiredVelocity, transform.up);
        if (Mathf.Abs(angle) > deadZone && !DestinationReached())
        {
            LookAtCustom();
        }

        // Check whether enemy has reached its NavMesh destination or not...
        if (DestinationReached())
        {
            eAnimator.SetFloat("Move", 0.0f);
        }
        // ...if destination has not been reached enemy start moving. 
        // Here and only here actuall movement is implemented!
        else
        {
            eAnimator.SetFloat("Move", 0.5f);
            ResetTriggers();
        }


        // Check if Player is in sight and start AI
        if (playerInRange && Physics.Raycast(triggerStuffObj.transform.position, ms.player.transform.position - transform.position, out hitEnemy, 55f, maskCombinedEnemySight))
        {
            if (hitEnemy.transform.tag == "Player")
            {
                if (ai.aiArchetype == AiTypes.Melee)
                {
                    aiMelee();
                }

                if (ai.aiArchetype == AiTypes.SpellCaster) 
                {
                    aiRanged();
                }
            }

            // Rotates enemy while an attack animation is playing and player is moving
            //if (animator.GetFloat("Move") == 0.0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("atack2"))
            //{
            //    LookAtTarget();
            //}
        }

	}

    //===========Melee===============

    void aiMeleeAttack()
    {
        eAnimator.SetTrigger("Atack");
        
    }

    void aiMelee()
    {
        //eAgent.SetDestination(ms.player.transform.position);
        if (DestinationReached() && CanAttackTarget())
        {
            aiMeleeAttack();
        }
        else
        {
            eAgent.SetDestination(ms.player.transform.position);
        }
    }

    //===========Melee===============

    //===========SpellCast===============

        /*Events - called by animations*/
    public void EventEnemySpellCast()
    {
        // below condition is required to avoid instantiation during transition, below statement make animator state Fireball case/name sensitive, keep that in mind while working with animator. 
        if (eAnimator.GetCurrentAnimatorStateInfo(0).IsName("SpellCast"))
        {
            if (ai.spellToCast.skillArchetype == archetype.Projectile)
            {
                // First Projectile
                GameObject instantiatedProjectile = Instantiate(ai.spellToCast.prefabFireballVFX, new Vector3(spellCastPoint.transform.position.x, spellCastPoint.transform.position.y, spellCastPoint.transform.position.z), spellCastPoint.transform.rotation) as GameObject;
                instantiatedProjectile.GetComponent<aRPG_Projectile>().GetObject(m, ai.spellToCast, gameObject.tag);

                // Additional Projectiles
                float projRotation = 0f;
                for (int i = 0; i < ai.spellToCast.addtionalProjectiles; i++)
                {
                    if (i % 2 == 0)
                    {
                        instantiatedProjectile = Instantiate(ai.spellToCast.prefabFireballVFX, new Vector3(spellCastPoint.transform.position.x, spellCastPoint.transform.position.y, spellCastPoint.transform.position.z), spellCastPoint.transform.rotation) as GameObject;
                        instantiatedProjectile.GetComponent<aRPG_Projectile>().GetObject(m, ai.spellToCast, gameObject.tag);

                        // Projectile rotation
                        projRotation = projRotation + 30f / Mathf.Sqrt(ai.spellToCast.addtionalProjectiles);
                        instantiatedProjectile.transform.Rotate(Vector3.up, projRotation);
                    }
                    else
                    {
                        instantiatedProjectile = Instantiate(ai.spellToCast.prefabFireballVFX, new Vector3(spellCastPoint.transform.position.x, spellCastPoint.transform.position.y, spellCastPoint.transform.position.z), spellCastPoint.transform.rotation) as GameObject;
                        instantiatedProjectile.GetComponent<aRPG_Projectile>().GetObject(m, ai.spellToCast, gameObject.tag);

                        instantiatedProjectile.transform.Rotate(Vector3.up, projRotation * -1);
                    }
                }
            }

            if (ai.spellToCast.skillArchetype == archetype.DoT)
            {
                dotColliderClone = Instantiate(ai.spellToCast.instantiatePrefab, new Vector3(ms.player.transform.position.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z), Quaternion.identity) as GameObject;
                dotColliderClone.transform.LookAt(new Vector3(ms.player.transform.position.x, ms.player.transform.position.y + 2f, ms.player.transform.position.z));
                
                dotColliderClone.GetComponent<aRPG_DoT>().SendObjects(ms, ai.spellToCast, gameObject.tag);
            }

            if (ai.spellToCast.skillArchetype == archetype.AoE)
            {
                ms.psSkills.AoE(gameObject.tag, ai.spellToCast, ms.player.transform.position);
            }
        }
    }
    
    public void EventEnemy_SpellCastContinuous_Begin()
    {
        if (ai.spellToCast.skillArchetype == archetype.DoT)
        {
            if (dotColliderClone != null) { Destroy(dotColliderClone); }
            
            dotColliderClone = Instantiate(ai.spellToCast.instantiatePrefab, spellCastPoint.transform.position, spellCastPoint.transform.rotation) as GameObject;
            dotColliderClone.transform.parent = gameObject.transform;
            
            dotColliderClone.GetComponent<aRPG_DoT>().SendObjects(ms, ai.spellToCast, gameObject.tag);
        }
    }

    public void EventEnemy_SpellCastContinuous_End()
    {
        if(ai.spellToCast.skillArchetype == archetype.DoT)
        {
            Destroy(dotColliderClone);
        }
    }
        /*Events*/
    
    void aiCastProjectile()
    {
        StopEnemyMoveNavAgent();
        LookAtTarget();
        eAnimator.SetBool("SpellCastBool", true);
    }

    void aiCastDotCollider()
    {
        StopEnemyMoveNavAgent();
        LookAtTarget();
        eAnimator.SetBool("SpellCastContBool", true);
    }

    void aiCastSpell()
    {
        // Continouos casting
        if (ai.spellToCast.skillArchetype == archetype.DoT && ai.spellToCast.spawnAtCastPoint)
        {
            aiCastDotCollider();
        }
        // One-by-one casting
        if (ai.spellToCast.skillArchetype == archetype.DoT && !ai.spellToCast.spawnAtCastPoint)
        {
            aiCastProjectile();
        }
        if (ai.spellToCast.skillArchetype == archetype.Projectile)
        {
            aiCastProjectile();
        }
        if (ai.spellToCast.skillArchetype == archetype.AoE)
        {
            aiCastProjectile();
        }
    }

    public void aiRanged()
    {
        //if (!aiReady)
        //{
        if (Vector3.Distance(ms.player.transform.position, transform.position) >= ai.rangedRange)
        {
            ResetTriggers();
            SetDestinationRanged();
        }
        else
        {
            ResetTriggers();
            if (Vector3.Distance(ms.player.transform.position, transform.position) < outOfMeleeRange)
            {

                // coroutine below will switch canGetAway variable between true or false every 6 seconds...
                if (!getAwayTimeRunning) { StartCoroutine(GetAwayTime(1f)); }
                // ...this means that when monster is being chased, every 6 seconds he will change his behaviore between "FB_GetAway" and "FB_CastFB". That's a game design decision to use such mechanism, for example; you can delete above coroutine and below condition leaving only "FB_GetAway" state - this way monster will always run away.
                if (canGetAway)
                {
                    SetDestinationRanged();
                }
                else
                {
                    aiCastSpell();
                }
            }
            else
            {
                if (!customDestRangedSet)
                {
                    aiCastSpell();
                }
            }
        }
        //}
        //StartCoroutine(AIDelay(1f));
    }

    //===========SpellCast===============

    public void SetDestinationRanged()
    {
        customDestRangedSet = true;
        Vector3 playerVector = (transform.position - ms.player.transform.position).normalized;
        Vector3 positionToReach = ms.player.transform.position + playerVector * distanceToKeep;
        Vector3 positionCorrected = new Vector3(positionToReach.x, transform.position.y, positionToReach.z);
        eAgent.SetDestination(positionCorrected);
    }

    public void StopEnemyMoveNavAgent()
    {
        eAnimator.SetFloat("Move", 0.0f);
        eAgent.ResetPath();
        eAgent.SetDestination(transform.position);
    }

    // works like LookAt function already in unity, but it is slightly smoother, refers only to XZ coordinates. It makes transform look at NavMesh point so this function should be called only when you know that there is a destination in NavMesh.
    void LookAtCustom()
    {
        var targetRotationShotEnemy = Quaternion.LookRotation(eAgent.steeringTarget - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationShotEnemy, Time.deltaTime * rotationSpeed);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        angle = 0f;

        // rotation alternative
        //var targetRotation = Quaternion.LookRotation(agent.steeringTarget -transform.position, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // works like LookAt function already in unity, but it is slightly smoother. Is used to look at the player.
    void LookAtTarget()
    {
        var targetRotation = Quaternion.LookRotation(ms.player.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);        
    }

    float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;
        angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;
        return angle;
    }

    // below function is checking if player has reached NavMesh destination.
    bool DestinationReached()
    {
        if (Vector3.Distance(transform.position, eAgent.destination) < eAgent.stoppingDistance) 
        {
            customDestRangedSet = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    // below we check if player is close enought to perform melee attack. attackRange variable sets the distance.
    bool CanAttackTarget()
    {
        if (Vector3.Distance(transform.position, ms.player.transform.position) < ai.meleeAttackRange)
        {
            return true;
        }
        else { return false; }
    }

    // Event functions are called by animations, check animation properities to set desired timing.
    void EventZombieAtack()
    {
        DoDamage();
    }

    // here we actually deal damage to the player and stun.
    //在这里我们实际上会对玩家造成伤害和眩晕。
    void DoDamage()
    {
        var targetScript = ms.player.GetComponent<aRPG_Health>();
        var targetScript2 = ms.player.GetComponent<aRPG_PlayerMovement>();
        if (CanAttackTarget())
        {
            targetScript.health -= esStats.DealDamage();
            if (esStats.stunsTarget && Random.value < esStats.stunChance)
            {
                targetScript2.Stunned();
            }
        }
        if (targetScript.health <= 0)
        {
            playerDead = true;
        }
    }

    public void CastProjectile()
    {
        StopEnemyMoveNavAgent();
        LookAtTarget();
        eAnimator.SetBool("SpellCastBool", true);
    }



    // animations runing are stopped and TakeDamage animation is ran. Here is where Stun is implemented.
    public void DamageTaken()
        {
            eAnimator.SetTrigger("TakeDamage");

            if (eAnimator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack"))
            {
                eAnimator.ResetTrigger("Atack");
                eAnimator.Play("Idle", 0);
            }
            if (eAnimator.GetAnimatorTransitionInfo(0).IsName("zombieAtakTrans"))
            {
                eAnimator.ResetTrigger("Atack");
                eAnimator.Play("Idle", 0);
            }
        }

    public void ResetTriggers()
    {
        eAnimator.ResetTrigger("Atack");
        eAnimator.SetBool("SpellCastBool", false);        
        eAnimator.SetBool("SpellCastContBool", false);

    }

    IEnumerator AIDelay(float time)
    {
        aiReady = true;
        yield return new WaitForSeconds(time);
        aiReady = false;
    }

    IEnumerator GetAwayTime(float time)
    {
        getAwayTimeRunning = true;
        yield return new WaitForSeconds(time);
        if (canGetAway == true) {canGetAway = false; } else { canGetAway = true; }
        getAwayTimeRunning = false;
    }

}
