using UnityEngine;
using System.Collections;
/// <summary>
/// 角色出生点
/// </summary>
public class aRPG_PlayerSpawn : MonoBehaviour {
    aRPG_PlayerMovement playerMovement;

    void OnLevelWasLoaded(int level)
    {
        GameObject playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
        gameObject.transform.position = playerSpawnPoint.transform.position;
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(playerSpawnPoint.transform.position);
        gameObject.GetComponent<aRPG_PlayerMovement>().StopMoveNavAgent();
    }

}
