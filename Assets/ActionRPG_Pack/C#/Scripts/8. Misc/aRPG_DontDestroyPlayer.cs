using UnityEngine;
using System.Collections;
// This script makes sure that there is only one copy of key GameObject in the scene. So if in every scene you'll place a player prefab and you'll travel in runtime to the other scene where player prefab also exists
// the other one will be destroyed and the right one will be kept.

// The player version of this script also makes sure that player will be placed where PlayerSpawnPoint is.
public class aRPG_DontDestroyPlayer : MonoBehaviour {

    [HideInInspector] public static aRPG_DontDestroyPlayer dontDestroyPlayer;

    aRPG_PlayerMovement playerMovement;

    void OnLevelWasLoaded(int level)
    {
        GameObject playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
        gameObject.transform.position = playerSpawnPoint.transform.position;
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(playerSpawnPoint.transform.position);
        
    }

    void Awake()
    {
        if (dontDestroyPlayer == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroyPlayer = this;
        }
        else if (dontDestroyPlayer != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (dontDestroyPlayer == this)
        {
            gameObject.GetComponent<aRPG_PlayerMovement>().StopMoveNavAgent();
        }
    }
}
