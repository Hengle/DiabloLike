using UnityEngine;
using System.Collections;

public class aRPG_EnemySight : MonoBehaviour {
    // here you can manage sphere collider that is responsible for the enemy awarness.
    // here information is sent to the main script when player crosses a collider.
    GameObject m;
    aRPG_Master ms;

    aRPG_EnemyMovement esMovement;
    SphereCollider sphColl;
    float sphCollBaseRadius = 0.0f;
    // that bonus makes sphere collider larger when player crosses it. It is wise to keep it at least on the 1.1 level to not allow player run easly from enemy just after spotting it.
    public float sphCollRadiusBonus = 1.6f;
    GameObject playerInRange;

	void Start () 
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

	    esMovement = gameObject.transform.parent.GetComponent<aRPG_EnemyMovement>();
	    sphColl = gameObject.GetComponent<SphereCollider>();
	    sphCollBaseRadius = sphColl.radius;

	}
	
	void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            InvokeRepeating("CheckIfPlayerIsAlive", 0.5f, 0.5f);
            esMovement.playerInRange = true;

            sphColl.radius = sphColl.radius*sphCollRadiusBonus;
		}
	}

    
    void OnTriggerExit(Collider otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
		    esMovement.playerInRange = false;
		    sphColl.radius = sphCollBaseRadius;
            CancelInvoke("CheckIfPlayerIsAlive");
		}
	}
    
    void CheckIfPlayerIsAlive()
    {
        if (esMovement.playerInRange)
        {
            if (ms.psHealth.playerIsDead)
            {
                esMovement.playerInRange = false;
                sphColl.radius = sphCollBaseRadius;
                CancelInvoke("CheckIfPlayerIsAlive");
                esMovement.ResetTriggers();
            }
        }
    }

}
