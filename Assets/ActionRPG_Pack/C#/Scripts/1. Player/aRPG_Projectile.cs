using UnityEngine;
using System.Collections;

public class aRPG_Projectile : MonoBehaviour {
    
    aRPG_DB_MakeSkillSO skill;
    GameObject m;
    aRPG_Master ms;
    string casterTag;

    aRPG_EnemyStats enemyStatsScript;
    float time;

    int[] contactsArray = new int[44];
    int contactsNo = 0;
    //声音在有这个脚本的特效上==================================
    MagicProjectileScript MagicP;

    public void GetObject(GameObject receivedObject, aRPG_DB_MakeSkillSO passedSkill, string passedCasterTag)
    {
        m = receivedObject;
        ms = m.GetComponent<aRPG_Master>();
        skill = passedSkill;
        casterTag = passedCasterTag;

        MagicP = gameObject.GetComponent<MagicProjectileScript>();
        if(MagicP != null)
        {
            MagicP.impactNormal = transform.position;
        }
    }

	void Start ()
    {
        ProjectileMovement(casterTag, skill, gameObject);
	}

    void OnTriggerEnter(Collider projectileContact)
    {
        if (projectileContact.tag != casterTag)
        {
            //这里没看懂contactsArray 没用上
            if (skill.piercing >= Random.Range(0.01f, 100f))
            {
                int contactID = projectileContact.gameObject.GetInstanceID();
                for (int i = 0; i < contactsNo; i++)
                {
                    if (contactsArray[i] == contactID)
                    {
                        return;
                    }
                }
                contactsArray[contactsNo] = projectileContact.gameObject.GetInstanceID();
                contactsNo++;
                ProjectileOnContact(projectileContact, skill, gameObject, true, casterTag);
            }
            else
            {
                ProjectileOnContact(projectileContact, skill, gameObject, false, casterTag);
            }
        }

    }

    /// <summary>
    /// 投掷物射中的情况
    /// </summary>
    /// <param name="projectileContact"></param>
    /// <param name="skill"></param>
    /// <param name="projectileGameObject"></param>
    /// <param name="piercing"></param>
    /// <param name="casterTag"></param>
    public void ProjectileOnContact(Collider projectileContact, aRPG_DB_MakeSkillSO skill, GameObject projectileGameObject, bool piercing, string casterTag)
    {
        // On contact with enemy
        if (projectileContact.tag == "enemy" && casterTag == "Player")
        {
            if (skill.damageProjectile > 0f)
            {
                // Projectile Damage
                projectileContact.GetComponent<aRPG_EnemyMovement>().DamageTaken();//怪物受击
                enemyStatsScript = projectileContact.GetComponent<aRPG_EnemyStats>();
                enemyStatsScript.currentHealth -= enemyStatsScript.ReceiveDamage(skill.damageTypeProjectile, skill.damageProjectile);//算伤害
            }
            if (skill.linkedSkillProjectile1 != null) {
                ms.psSkills.ExecuteLink(casterTag, skill.linkedSkillProjectile1, projectileGameObject.transform.position);
            }
            //感觉这个ExecuteLink在这个函数中执行了两次，测测=================================================================================================================
        }
        // On contact with player
        if (projectileContact.tag == "Player" && casterTag == "enemy")
        {
            if (skill.damageProjectile > 0f)
            {
                // Projectile Damage
                ms.psHealth.health -= skill.damageProjectile;
            }
            if (skill.linkedSkillProjectile1 != null) {
                ms.psSkills.ExecuteLink(casterTag, skill.linkedSkillProjectile1, projectileGameObject.transform.position);
            }

        }
        // Destroy
        if (projectileContact.tag != "enemy" && projectileContact.tag != "Player")
        {
            if (skill.linkOnEndOfLife == true && skill.linkedSkillProjectile1 != null)
            {
                ms.psSkills.ExecuteLink(casterTag, skill.linkedSkillProjectile1, projectileGameObject.transform.position);
            }
            //投射物删除======================3
            Destroy(projectileGameObject);
        }
        else
        {
            //不能穿透
            if (piercing == false)
            {
                if (skill.linkOnEndOfLife == true && skill.linkedSkillProjectile1 != null)
                {
                    ms.psSkills.ExecuteLink(casterTag, skill.linkedSkillProjectile1, projectileGameObject.transform.position);
                }
                //投射物删除======================2
                Destroy(projectileGameObject);
            }
        }
    }

    /// <summary>
    /// 投射物的移动两种方式Rigidbody和非刚体
    /// 然后用协程做移动
    /// </summary>
    /// <param name="casterTag"></param>
    /// <param name="skill"></param>
    /// <param name="projectileGameObject"></param>
    public void ProjectileMovement(string casterTag, aRPG_DB_MakeSkillSO skill, GameObject projectileGameObject)
    {
        time = Time.time;
        if (projectileGameObject.GetComponent<Rigidbody>() != null && skill.rigidbodyMovement)
        {
            projectileGameObject.GetComponent<Rigidbody>().velocity = projectileGameObject.transform.forward * skill.speedProjectile;
            StartCoroutine(ProjectileMovementCoroutine(casterTag, skill, projectileGameObject, true));
        }
        else
        {
            StartCoroutine(ProjectileMovementCoroutine(casterTag, skill, projectileGameObject, false));
        }
    }

    IEnumerator ProjectileMovementCoroutine(string casterTag, aRPG_DB_MakeSkillSO skill, GameObject projectileGameObject, bool isRigidbody)
    {
        while (time + skill.lifetimeProjectile > Time.time && projectileGameObject != null)
        {
            if (isRigidbody == false)
            {
                projectileGameObject.transform.Translate(Vector3.forward * Time.deltaTime * skill.speedProjectile);
            }
            yield return null;

        }
        //投射物的最后链接，是什么意思？  投射物飞到目标点，出效果，爆炸或者其他，不是最后的链接，是结束之后，接着是什么技能！！没有射中目标的情况
        if (skill.linkOnEndOfLife == true && projectileGameObject != null && skill.linkedSkillProjectile1 != null)
        {
            ms.psSkills.ExecuteLink(casterTag, skill.linkedSkillProjectile1, projectileGameObject.transform.position);
        }
        //投射物删除======================1
        Destroy(projectileGameObject);
    }
}
