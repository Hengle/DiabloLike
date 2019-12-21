using UnityEngine;
using System.Collections;

public class aRPG_TimedDestroy : MonoBehaviour {
    public float destroyTime=5f;
	void Start () {
	    Destroy (gameObject, destroyTime);
	}
	
}
