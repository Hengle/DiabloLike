using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class aRPG_PlayerMovement : MonoBehaviour {

    GameObject m;
    aRPG_Master ms;

    Collider colliderHit;

    [HideInInspector] public Transform trackingEnemyObj;
    [HideInInspector] public bool trackingEnemy = false;
    [HideInInspector] public bool pendingEnemyMeleeAtack = false;
    [HideInInspector] public bool pendingOpenDoor = false;
    [HideInInspector] public bool isNearDoor = false;
    [HideInInspector] public GameObject door;
    [HideInInspector] public GameObject nearDoorId;
    Transform doorPos;
    [HideInInspector] public bool newPathPending = false;
    [HideInInspector] float angle;

    float stoppingDistance;

    // deadZone and rotationSpeed controls player character rotations
    public float deadZone = 8.0f;
    public float rotationSpeed = 22.0f;

    void Awake()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
    }

	void Start ()
    {

        ms.pNavAgent.updatePosition = false;
        ms.pNavAgent.updateRotation = false;
        deadZone *= Mathf.Deg2Rad;
        stoppingDistance = ms.pNavAgent.stoppingDistance;
    }
	

	void Update () {
        if(ms.psInput.wasdMovement == true || ms.psInput.joystickMovement == true)
        { return;}

        // here we make sure that navmeshAgent follows player
        Vector3 worldDeltaPosition = ms.pNavAgent.nextPosition - transform.position;
        if (worldDeltaPosition.magnitude > ms.pNavAgent.radius/3)
        {
            ms.pNavAgent.nextPosition = transform.position + 0.6f * worldDeltaPosition;
        }

        // here we calculate angle difference between current player direction a a direction to go, without it player will "LookAt" so often that it will look unnatural.
        angle = FindAngle(transform.forward, ms.pNavAgent.desiredVelocity, transform.up);
        if (Mathf.Abs(angle) > deadZone)
        {
            LookAtCustom();
        }

        // this enables tracking. Tracking makes character follow a monster, without it player would go to the old monster position.
        if (trackingEnemy)
        {
            if (trackingEnemyObj != null)
            { 
                SetDestinationCustom(trackingEnemyObj.position); 
            }
            stoppingDistance = ms.psSkills.meleeRange*0.5f;
        }
        else { stoppingDistance = ms.pNavAgent.stoppingDistance; }

        // this is related to tracking. This enables character to make an attack after he reaches destination(while no button is being held. It is not required to use it to make an attack when you hold button) 
        if (pendingEnemyMeleeAtack && DestinationReached())
        {
            pendingEnemyMeleeAtack = false;
            ms.psSkills.SimpleMeleeAttack(trackingEnemyObj);
        }

        // this allows player to open a door when a button is NOT being held.
        if (pendingOpenDoor == true && isNearDoor == true && door != null && door.GetInstanceID() == nearDoorId.GetInstanceID())
        {
            if (door.GetComponent<aRPG_OpenDoor>().onceOpened == false)
            {
                door.GetComponent<aRPG_OpenDoor>().OpenDoor();
                return;
            }
        }

        // here we stop movement when a destination is reached
        if (DestinationReached())
        {
            trackingEnemy = false;
            StopMoveNavAgent();
        }
        // and finally most important part. Here we initiate movement.
        if (newPathPending)
        {
            ms.pAnimator.SetFloat("Move", 0.5f);
            ResetTriggers();
        }
        else
        {
            StopMoveNavAgent();
        }
	}

    // # this is custom version of "LookAt" it should be used in update or while button is being held, cause it rotates player over time, it also allows to control the speed of rotation with "rotationSpeed". It is designed to look at point rather then an object.
    public void LookAtCustom()
    {
        var targetRotation = Quaternion.LookRotation(ms.pNavAgent.desiredVelocity, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        angle = 0f;
    }

    // # look at enemy
    public void LookAtThisEnemy(GameObject thisEnemy)
    {
        var targetRotationShotEnemy = Quaternion.LookRotation(thisEnemy.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationShotEnemy, Time.deltaTime * rotationSpeed);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
    }

    public void LookAtPoint(Vector3 point)
    {
        var targetRotation = Quaternion.LookRotation(new Vector3(point.x, transform.position.y, point.z) - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        angle = 0f;
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

    public void SetDestinationCustom(Vector3 destination)
    {
        newPathPending = true;
        ms.pNavAgent.SetDestination(destination);
        ResetTriggers();
    }
    
    // # call this function whenever you want to check if a player has reached its navmesh destination. It usually used in update.
    bool DestinationReached()
    {
        if (Vector3.Distance(gameObject.transform.position, ms.pNavAgent.destination) < stoppingDistance)
        {
            return true;
        }
        else { return false; }
    }

    // That is general function used by many scripts to stop the nav mesh agent. 
    // While it may seem that this function is doing too much and simple Stop() would be enought in some rare cases it necessary do go throught most of these steps.
    public void StopMoveNavAgent()
    {
        newPathPending = false;
        ms.pAnimator.SetFloat("Move", 0.0f);
        ms.pNavAgent.ResetPath();
        ms.pNavAgent.Stop();
        ms.pNavAgent.enabled = false;
        ms.pNavAgent.enabled = true;
    }

    // # it is usually called when script wants to run an animation that is not in layer0 of animator: like door open, stun or equip weapon. It tries to reset the state of the character so that any next animation ran will go in nice and smooth.
    public void ResetTriggers()
    {
        ms.pAnimator.ResetTrigger("ShotgunFireTr");
        ms.pAnimator.ResetTrigger("WeaponTr");
        ms.pAnimator.ResetTrigger("GunFireTr");
        ms.pAnimator.ResetTrigger("RifleFireTr");
        ms.pAnimator.SetBool("ShotgunBool", false);
        ms.pAnimator.SetBool("FireballBool", false);
        ms.pAnimator.SetBool("rayBool", false);
    }

    void OnControllerColliderHit(ControllerColliderHit colliderHit)
    {
        if (colliderHit.gameObject.tag == "door")
        {
            StopMoveNavAgent();
        }
    }

    public void DoorOpen()
    {
        ms.pAnimator.SetTrigger("DoorInTr");
        //soundDoor.Play();
    }

    // # call it whenever you think player character should receive stun animation state. It's functionality is driven by ResetTriggers function so pay attention to both when working with stun.
    public void Stunned()
    {
        //in Animator change "damage" layer weigth to enable/disable player character reaction to hits received
        
        ResetTriggers();
        Destroy(ms.psSkills.DoTClone);
        ms.pAnimator.SetTrigger("DamageTr");
        ms.pAnimator.Play("Idle", 0);
    }




}
