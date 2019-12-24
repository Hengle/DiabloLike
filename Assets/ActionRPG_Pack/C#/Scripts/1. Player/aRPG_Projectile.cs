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

    public void GetObject(GameObject receivedObject, aRPG_DB_MakeSkillSO passedSkill, string passedCasterTag)
    {
        m = receivedObject;
        ms = m.GetComponent<aRPG_Master>();
        skill = passedSkill;
        casterTag = passedCasterTag;
    }

	void Start ()
    {
        ms.psSkills.ProjectileMovement(casterTag, skill, gameObject);
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
                ms.psSkills.ProjectileOnContact(projectileContact, skill, gameObject, true, casterTag);
            }
            else
            {
                ms.psSkills.ProjectileOnContact(projectileContact, skill, gameObject, false, casterTag);
            }
        }

    }
}
