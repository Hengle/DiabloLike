using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// # most of the scipts start with gettin this script component and then refer to it to in irder to get access to variables.

// # some general scripting standarts:
// # p - stands for player character
// # ps - stands for player character script, then it is followed by a script name
// # m - stands for object that this script is attached to
// # ms - stands for this script
// # so often in other script you will find something like this: ms.psSkills.x where x is a variable name we are accessing
// #

// # in result often in other script you'll see something like that: ms.psHealth.mana - in order to access mana. 
// # much easier to develop like that plus this is necessary to implement game control features like saving.

// # below enum is outside of class so other scripts could easly access it.

public enum skills { None, BaseAttack, Fireball, Ray, Medkit, Move };
public enum damageType { Physical, Fire, Magic };
public enum buttonCharacter { WeaponSelection, Keyboard_Mobile, MouseRight, MouseLeft};

/// <summary>
/// 和player相关的脚本，一部分在player预制上（可随player删除），一部分在SCRIPTS上（常驻）
/// 这样设计是为了，便于控制和其他操作，就像保存。暂时没看出来
/// </summary>
public class aRPG_Master : MonoBehaviour {
    internal GameObject mc;
    internal GameObject s;

    internal aRPG_GuiDeathMenu mcsDeath;
    internal aRPG_GuiEnemyInfo mcsEnemyInfo;
    internal aRPG_GuiHealth mcsHealth;
    internal aRPG_GuiTbManagement mcsManagement;

    internal GameObject cam;
    internal aRPG_CameraMovement camMove;

    public GameObject playerPrefab;
    internal GameObject player;

    internal GameObject respawnPoint;
    internal GameObject deathMenu;

    internal GameObject waypointMenu;

    //player核心脚本
    internal aRPG_Input psInput;
    internal aRPG_Health psHealth;
    internal aRPG_ItemPickup psItemPick;
    internal aRPG_PlayerAnimatorEvents psEvents;
    internal aRPG_PlayerMovement psMovement;
    internal aRPG_PlayerSpawn psSpawn;
    internal aRPG_Skills psSkills;
    internal aRPG_CharacterStats psStats;
    internal aRPG_Inventory psInventory;

    internal Animator pAnimator;
    internal CharacterController pCharacterController;
    internal UnityEngine.AI.NavMeshAgent pNavAgent;

    internal aRPG_GuiTbManagement gsManagement;
    internal aRPG_GuiHealth gsHealth;
    internal aRPG_GuiEnemyInfo gsEnemyInfo;

    internal GameObject joystick;
    internal aRPG_Joystick gsJoystick;

    //层的详细说明可以看 readme
    internal int layerPlayer = 1 << 20;
    internal int layerEnemies = 1 << 21;
    internal int layerEnemyMouseCollider = 1 << 22; // this layer is for enemies - more precisly it is for ColliderMouse child game object of the enemy prefab. By changing the size of the collider you can change mouse precision for targeting enemies
    internal int layerTargetingPlaneToMove = 1 << 23; // this layer is for registering movement. Targeting plane prefab should be placed just beneath the ground mesh used to bake nav mesh.
    internal int layerObstacles = 1 << 24;
    internal int layerInteractiveObject = 1 << 25; // this is doors mostly.
    internal int layerBullets = 1 << 26;
    internal int layerTargetingPlaneToShoot = 1 << 27;
    internal int layerGround = 1 << 28;
    internal int layerEnemiesProjectiles = 1 << 29;

    // combined layer masks:
    internal int maskMoveEnemiesDoors = 0;
    internal int maskMoveEnemies = 0;
    internal int maskShootEnemies = 0;
    
    void Awake()
    {
        OnAwake();

        DataManager.Instance.Init();

        //UI初始化
        UIManager.Instance.Init();

    }
    

    public void OnAwake()
    {
        s = gameObject;

        mc = GameObject.Find("MainCanvas");

        mcsEnemyInfo = mc.GetComponent<aRPG_GuiEnemyInfo>();
        mcsHealth = mc.GetComponent<aRPG_GuiHealth>();
        mcsManagement = mc.GetComponent<aRPG_GuiTbManagement>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        respawnPoint = GameObject.Find("PlayerSpawnPoint");


        cam = GameObject.Find("Camera");
        camMove = cam.GetComponent<aRPG_CameraMovement>();

        if (deathMenu == null)
        {
            deathMenu = GameObject.Find("MainCanvas/DeathMenu_@");
            mcsDeath = deathMenu.GetComponent<aRPG_GuiDeathMenu>();
        }

        waypointMenu = GameObject.Find("MainCanvas/WaypointMenu_@");

        psInput = s.GetComponent<aRPG_Input>();
        psHealth = player.GetComponent<aRPG_Health>();
        psItemPick = player.GetComponent<aRPG_ItemPickup>();
        psEvents = player.GetComponent<aRPG_PlayerAnimatorEvents>();
        psMovement = player.GetComponent<aRPG_PlayerMovement>();
        psSpawn = player.GetComponent<aRPG_PlayerSpawn>();
        psSkills = player.GetComponent<aRPG_Skills>();
        psStats = s.GetComponent<aRPG_CharacterStats>();
        psInventory = s.GetComponent<aRPG_Inventory>();

        pAnimator = player.GetComponent<Animator>();
        pNavAgent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();

        //用于射线时，选择性忽略的层
        maskMoveEnemiesDoors =  layerTargetingPlaneToMove | layerEnemyMouseCollider | layerInteractiveObject;
        maskMoveEnemies = layerTargetingPlaneToMove | layerEnemyMouseCollider;
        maskShootEnemies = layerTargetingPlaneToShoot | layerEnemyMouseCollider;

        gsManagement = mc.GetComponent<aRPG_GuiTbManagement>();
        gsHealth = mc.GetComponent<aRPG_GuiHealth>();
        gsEnemyInfo = mc.GetComponent<aRPG_GuiEnemyInfo>();

        if (joystick == null) { joystick = GameObject.Find("MainCanvas/MobileJoystick_@"); }
        if (gsJoystick == null && joystick != null) { gsJoystick = joystick.GetComponent<aRPG_Joystick>(); }
    }
    /// <summary>
    /// 重新生成player
    /// </summary>
    public void Respawn()
    {
        Destroy(player);
        StartCoroutine("RespawnCoroutine", 1f);
    }

    IEnumerator RespawnCoroutine(float wait)
    {
        yield return new WaitForSeconds(wait);

        respawnPoint = GameObject.Find("PlayerSpawnPoint");
        Instantiate(playerPrefab, respawnPoint.transform.position, respawnPoint.transform.rotation);
        
        waypointMenu.SetActive(true);
        OnAwake();
        deathMenu.SetActive(false);
        waypointMenu.SetActive(false);

        psInput.enabled = true;
        camMove.enabled = true;
        camMove.target = player.transform;
    }


}
