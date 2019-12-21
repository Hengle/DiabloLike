using UnityEngine;
using System.Collections;

public class aRPG_OpenDoor : MonoBehaviour {
    GameObject m;
    aRPG_Master ms;

    SphereCollider colliderSphere;
    [HideInInspector] public bool onceOpened = false;
    RaycastHit hit;

    public GameObject FoWroof;
    public GameObject FoWroofSecondary;
    public GameObject FoWroofTexture;
    public Texture FoWroofTextureLocked;
    bool destroyDoorCollider = true;
    public enum lockedWith{none, key1, key2, key3, keyBasement};
    public lockedWith keyRequired = lockedWith.none;

    public bool openOnLoad = false;
	
	void Start () {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

	    colliderSphere = gameObject.GetComponent<SphereCollider>();
	
	    if(openOnLoad){OpenDoorOnLoad();}
	}
	
	void OnTriggerStay (Collider other) {
        if (other.tag == "Player")
        {
            ms.psMovement.nearDoorId = gameObject;
            ms.psMovement.isNearDoor = true;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (ms.psInput.joystickMovement == false && ms.psInput.wasdMovement == false)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (Physics.Raycast(ray, out hit, 60.0f, ms.layerInteractiveObject))
                    {
                        if (hit.transform.tag == "door" && onceOpened == false)
                        {
                            OpenDoor();
                        }
                    }
                }

            }
            else { OpenDoor(); }
        }
	}
	
    void OnTriggerExit (Collider other) {
	    if(other.tag == "Player"){
            ms.psMovement.isNearDoor = false;
	    }
    }

    // # is called by below OpenDoor() to check if we have a key that is required.
    bool Keys () {
	    if(keyRequired == lockedWith.none){
	    return true;
	    }
        if (keyRequired == lockedWith.key1 && ms.psInventory.key1)
        {
	    return true;
	    }
	    if(keyRequired == lockedWith.key2 && ms.psInventory.key2){
	    return true;
	    }
	
	    print("You need a key");
	    FoWroofTexture.GetComponent<Renderer>().material.mainTexture = FoWroofTextureLocked;
        return false;
	    }

    // # is cast when player is in doors sphere collider. It manages all door opening options and opens door. 
    public void OpenDoor () {
        ms.psMovement.pendingOpenDoor = false;
	    if(gameObject.name == "Door"){
		    if(Keys()){}else{return;}
			
		    transform.GetComponent<Animation>().Play("doorOpen");
		    onceOpened = true;
            ms.psMovement.isNearDoor = false;

            ms.psMovement.DoorOpen();
		    colliderSphere.enabled = false;
		
		    if(FoWroof){Destroy(FoWroof);}
		    if(FoWroofSecondary){Destroy(FoWroofSecondary);}
		    if(FoWroofTexture){Destroy(FoWroofTexture);}
            if (destroyDoorCollider) {Destroy(gameObject.GetComponent<BoxCollider>());}
		    }

	    if(gameObject.name == "DoorX"){
			    // If you want to introduce another type of doors, like a locked door or with different animation, change the name of the "Door" and work here on scripting that
		    }
		
	    }
	// # call this function on Start if you want doors to look like they are open from the begining of the scene.
    void OpenDoorOnLoad () {
	    transform.GetComponent<Animation>().Play("doorOpen");
	    onceOpened = true;
	    colliderSphere.enabled = false;
	    if(FoWroof){Destroy(FoWroof);}
	    if(FoWroofSecondary){Destroy(FoWroofSecondary);}
	    if(FoWroofTexture){Destroy(FoWroofTexture);}
	    }


    void OnMouseEnter () {
	    GetComponent<Renderer>().material.color = Color.green;
	    }
	
    void OnMouseExit () {
	    GetComponent<Renderer>().material.color = Color.white;
	    }

}
