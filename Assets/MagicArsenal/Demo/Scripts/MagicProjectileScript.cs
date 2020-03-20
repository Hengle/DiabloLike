using UnityEngine;
using System.Collections;
 
public class MagicProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;//撞击特效
    public GameObject projectileParticle;//投掷物特效
    public GameObject muzzleParticle;//枪口特效
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle. 记录初始位置，最后算撞击特效的法线方向
 
    private bool hasCollided = false;
 
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
		if (muzzleParticle){
        muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
        //碰撞统一用collider.isTrigger
        Collider collider = gameObject.GetComponent<Collider>();
        if(collider != null)
        {
            if (!collider.isTrigger)
            {
                Debug.LogError("Effect collider.isTrigger is false==" + transform.name);
                collider.isTrigger = true;
            }
        }
    }
 
    void OnTriggerEnter(Collider hit)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            //transform.DetachChildren();
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);
 
            if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
            {
                Destroy(hit.gameObject);
            }
 
 
            //yield WaitForSeconds (0.05);
            foreach (GameObject trail in trailParticles)
            {
                GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, 3f);
            }
            Destroy(projectileParticle, 3f);
            Destroy(impactParticle, 5f);
            //投射物删除======================4
            if(gameObject.GetComponent<aRPG_Projectile>() == null)
                Destroy(gameObject);
            //projectileParticle.Stop();
			
			ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
            //Component at [0] is that of the parent i.e. this object (if there is any)
            for (int i = 1; i < trails.Length; i++)
            {
                ParticleSystem trail = trails[i];
                if (!trail.gameObject.name.Contains("Trail"))
                    continue;

                trail.transform.SetParent(null);
                Destroy(trail.gameObject, 2);
            }
        }
    }
}