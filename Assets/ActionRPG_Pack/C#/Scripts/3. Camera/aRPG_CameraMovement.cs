using UnityEngine;
using System.Collections;

// You can switch between ortho and perspective view by clicking middle mouse button
// at some point in development of your game try experimenting with ortho and perspective(plus field of view) and camera positioning to work out your own style of presentation
// dont be afraid to change below values while doing that.

public class aRPG_CameraMovement : MonoBehaviour {
    GameObject m;
    aRPG_Master ms;
	[HideInInspector] public Transform target;

    public float adjustHeight = 23.0f;
    public float adjustX = 12.0f;
    public float adjustZ = 0.0f;

    public bool useSmoothing = false;
    public float heightDamping = 2.0f;
    public float zxDamping = 33.0f;

    public bool useSlerp = false;
    public float rotationSpeed = 44.0f;

    [HideInInspector] public float targetHeight = 10.0f;
    [HideInInspector] public float currentHeight = 10.0f;
    [HideInInspector] public float targetZ = 10.0f;
    [HideInInspector] public float currentZ = 10.0f;
    [HideInInspector] public float targetX = 10.0f;
    [HideInInspector] public float currentX = 10.0f;

	void Start () {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
        target = ms.player.transform;
	}
	
	void LateUpdate () {
	    
        if(Input.GetAxis("Mouse ScrollWheel")>0 && gameObject.GetComponent<Camera>().orthographic == false){
		adjustHeight += 0.5f;
		}
	
	if(Input.GetAxis("Mouse ScrollWheel")<0 && gameObject.GetComponent<Camera>().orthographic == false){
		adjustHeight -= 0.5f;
		}
	if(Input.GetMouseButtonDown(2)){
		if(gameObject.GetComponent<Camera>().orthographic == true){
		gameObject.GetComponent<Camera>().orthographic = false;
		adjustHeight = 22;
		}else{gameObject.GetComponent<Camera>().orthographic = true;}
		
		}
	
    targetHeight = target.position.y + adjustHeight;
    currentHeight = transform.position.y;
    currentHeight = Mathf.Lerp (currentHeight, targetHeight, heightDamping * Time.deltaTime);
    
    targetX = target.position.x + adjustX;
    currentX = transform.position.x;
    currentX = Mathf.Lerp (currentX, targetX, zxDamping * Time.deltaTime);
    
    targetZ = target.position.z + adjustZ;
    currentZ = transform.position.z;
    currentZ = Mathf.Lerp (currentZ, targetZ, zxDamping * Time.deltaTime);
    if(useSmoothing){
        transform.position = new Vector3(currentX, currentHeight, currentZ); 
	}else{
        transform.position = new Vector3(targetX, targetHeight, targetZ); 
	}

    if(useSlerp){
	    LookAtPlayer();
	}else{
	    transform.LookAt (target);
	}


	}

    void LookAtPlayer(){
	var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
	transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
	}
}
