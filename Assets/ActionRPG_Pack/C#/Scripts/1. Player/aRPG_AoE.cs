using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AoE伤害的触发，技能到时间销毁
/// </summary>
public class aRPG_AoE : MonoBehaviour {

    SkillData skill;
    string casterTagThis;
    aRPG_Master ms;

    //int enemyID;
    //aRPG_EnemyStats enemyStats;
    ////bool do_DotDmg = true;
    //bool doDamageToPlayer = false;
    aRPG_EnemyStats enemyStatsScript;

    Dictionary<int, bool> dot_enemy_Dictionary = new Dictionary<int, bool>();

    void Start()
    {
        if (skill != null)
        {
            Destroy(gameObject, (float)skill.lifetime);//回收
        }
    }
    
    public void SendObjects(aRPG_Master receivedScript , SkillData passedSkill, string casterTag)
    {
        skill = passedSkill;
        casterTagThis = casterTag;
        ms = receivedScript;

        if (casterTag == "Player") { ms.psStats.curAttr.Mana -= (long)skill.AoEmanaCost; }
        StartCoroutine(AoE_Damage(casterTag, skill, this.transform.position));
    }

    IEnumerator AoE_Damage(string casterTag, SkillData skill, Vector3 center)
    {
        yield return new WaitForSeconds((float)skill.AoEdamageDelay);

        if (casterTag == "Player")
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, (float)skill.AoEradius, ms.layerEnemies);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "enemy")
                {
                    hitColliders[i].gameObject.GetComponent<aRPG_EnemyMovement>().DamageTaken();
                    enemyStatsScript = hitColliders[i].gameObject.GetComponent<aRPG_EnemyStats>();
                    enemyStatsScript.ReceiveDamage(skill.AoEdamageType, skill.AoEdamage);
                }
                i++;
            }
        }

        if (casterTag == "enemy")
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, (float)skill.AoEradius, ms.layerPlayer);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Player")
                {
                    ms.psStats.curAttr.Health -= (long)skill.AoEdamage;
                }
                i++;
            }
        }

    }
}
