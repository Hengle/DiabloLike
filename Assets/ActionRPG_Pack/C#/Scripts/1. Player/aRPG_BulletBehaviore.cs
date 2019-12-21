using UnityEngine;
using System.Collections;

public class aRPG_BulletBehaviore : MonoBehaviour {
    public float speed = 1.1f;
    aRPG_EnemyStats EnemyStatsScript;
    aRPG_Inventory psInventory;
    GameObject m;
    public GameObject impactEffect;
    public damageType dmg_type = damageType.Physical;


    public void GetObject(GameObject receivedObject)
    {
        m = receivedObject;
        psInventory = m.GetComponent<aRPG_Inventory>();
    }


	void Start () {
	GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
    
	
	void OnTriggerEnter(Collider bulletContact)
    {
	    if(bulletContact.tag == "enemy")
        {
            EnemyStatsScript = bulletContact.GetComponent<aRPG_EnemyStats>();

            EnemyStatsScript.currentHealth -= EnemyStatsScript.ReceiveDamage(dmg_type, psInventory.startingEquippedWeapon.damage);
            
		    // 	below script is looking for a empty game object named "shotEffectFront", then at it's position(and with it's rotation!) instantiates a particle effect that act as a blood effect from a gun shot.
		    var impactEffectposition = bulletContact.transform.Find("shotEffectFront");
		    Instantiate(impactEffect, impactEffectposition.transform.position, impactEffectposition.transform.rotation);
	        Destroy(gameObject);
        }
    }
}
