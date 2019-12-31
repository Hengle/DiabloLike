using UnityEngine;
using System.Collections;

/// <summary>
/// DOT - Damage over time，在一段时间内不断对目标造成伤害。
/// AOE - Area Effect Damage，区域作用魔法。指的是一个可以伤害一个区域中的一群怪物的魔法，例如法师的暴风雪和奥术爆炸。
/// MeleeSweep 近战扫荡？
/// </summary>
public enum archetype { BuffDebuff, Projectile, Bullet, DoT, AoE, BasicAttack, Move, MeleeSweep}
public enum sttChange { Strenght, Int, Health, Mana}

public class aRPG_DB_MakeSkillSO : ScriptableObject
{

    [Header("General")]
    // skill archetype defines the skill. If you choose a Melee Sweep only variables from under Melee Sweep header will affect skill.
    public archetype skillArchetype;
    public string UNIQUE_skillName;
    public Sprite sprite;
    public Sprite spriteNoMana;
    public bool hasLimitedNoOfUses = false;
    public int ammo_amount;

    [Header("Melee Sweep近战扫荡")]
    //=======Melee Sweep=======
    public float damageModifierPercent;
    public float arcWidth;
    public float arcLength;
    //=======Melee Sweep=======

    //=======Consumable=======
    [Header("Consumable消耗品，")]
    public sttChange sttChange;
    // if sttChangeDuration is 0 then change will be permanent.
    public float sttChangeDuration;
    public float sttChangeAmount;
    public float sttChangeUseDelay;
    //public int sttNoOfUses;
    //=======Consumable=======

    //=======Projectile=======
    [Header("Projectile投掷物")]
    public GameObject prefabFireballVFX;
    [Range(0, 7)]
    public int addtionalProjectiles = 0;
    public float damageProjectile;
    public damageType damageTypeProjectile;
    public float manaCostProjectile;
    public Vector3 castPointLocalPosProjectile;
    public float speedProjectile;
    public float lifetimeProjectile;
    public bool rigidbodyMovement = true;
    [Range(0f, 100f)]
    public float piercing = 0f;//穿透
    // linked skill will be executed on contact with and enemy or at the end of life, not all skills can be linked, for now AoE and DoT archetypes work.
    public aRPG_DB_MakeSkillSO linkedSkillProjectile1;
    // below bool determines whether projectile will execute link when 
    public bool linkOnEndOfLife = true;
    //=======Projectile=======

    //=======AoE=======
    [Header("AoE")]
    public float AoEdamageDelay;
    public float AoEdamage;
    public damageType AoEdamageType;
    public float AoEradius;
    public GameObject AoEprefabVFX;
    public float AoEmanaCost;
    //=======AoE=======

    //=======Collider-DoT=======
    [Header("Collider-DoT")]
    public GameObject instantiatePrefab;
    public float dps = 11f;
    public float damageFrequency = 0.02f;
    // below var is mana cost per second for single DoT's that are spawned at player hands. Or is a mana cost per single use in case of DoT spawned at certain location.
    public float manaCostPerSecOrUse = 11f;
    public float lifetime = 1.5f;
    public bool spawnAtCastPoint = true;
    //=======Collider-DoT=======

    //=======Other=======
    [Header("Other")]
    /*variables for scripting operations*/
    internal bool delayIsOn = false;
    internal float currentSpriteFill;
    internal bool manaDegenIsOn = false;
    /*variables for scripting operations*/

    /*Don't change these values unless you really need to*/
    // below define speed of visual updates of things like cooldown sprite fill or mana drain.
    internal float spriteFillSpeed = 0.03f;
    internal float manaDegenInterval = 0.03f;
    /*Don't change these values unless you really need to*/
    
    //=======Functions=======
    public void OnAwake()
    {
        currentSpriteFill = 1f;
        delayIsOn = false;
    }

    public void Stats()
    {

    }

}
