using UnityEngine;
using System.Collections;

// This script makes sure that there is only one copy of key GameObject in the scene. So if in every scene you'll place a player prefab and you'll travel in runtime to the other scene where player prefab also exists
// the other one will be destroyed and the right one will be kept.

public class aRPG_DontDestroyCamera : MonoBehaviour {

    [HideInInspector] public static aRPG_DontDestroyCamera dontDestroyCamera;

    void Awake()
    {
        if (dontDestroyCamera == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroyCamera = this;
        }
        else if (dontDestroyCamera != this)
        {
            Destroy(gameObject);
        }
    }
}
