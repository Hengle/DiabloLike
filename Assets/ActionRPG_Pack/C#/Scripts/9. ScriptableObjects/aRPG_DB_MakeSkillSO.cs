using UnityEngine;
using System.Collections;

/// <summary>
/// Projectile，投射物，有碰撞
/// DOT - Damage over time，在一段时间内不断对目标造成伤害。持续伤害。非动画驱动，有碰撞
/// AOE - Area Effect Damage，区域作用魔法。指的是一个可以伤害一个区域中的一群怪物的魔法。一次性伤害。非动画驱动。无碰撞直接算范围的
/// MeleeSweep 近战扫荡？无碰撞直接算范围的
/// </summary>
public enum archetype { BuffDebuff, Projectile, Bullet, DoT, AoE, BasicAttack, Move, MeleeSweep}
public enum sttChange { Strenght, Int, Health, Mana}

public class aRPG_DB_MakeSkillSO : ScriptableObject
{

    [Header("General")]
    // skill archetype defines the skill. If you choose a Melee Sweep only variables from under Melee Sweep header will affect skill.
    //技能原型定义了技能。如果你选择一个近战扫荡只有变量从下面的近战扫荡头将影响技能。
    public archetype skillArchetype;
    public string UNIQUE_skillName;
    public Sprite sprite;//技能图标
    public Sprite spriteNoMana;//没蓝时的技能图标
    public bool hasLimitedNoOfUses = false;//投掷物，是否消耗子弹
    public int ammo_amount;//弹药消耗

    [Header("Melee Sweep近战扫荡")]
    //=======Melee Sweep=======
    public float damageModifierPercent;//伤害是武器伤害的，加成比 1是无加成
    public float arcWidth;//身前宽度，这是一半，左右各一半
    public float arcLength;//身前长度，
    //=======Melee Sweep=======

    //=======Consumable=======
    [Header("Consumable消耗品，")]
    public sttChange sttChange;//要改变的属性
    // if sttChangeDuration is 0 then change will be permanent.
    //如果sttChangeDuration为0，则更改将是永久的。
    public float sttChangeDuration;//改变的时间
    public float sttChangeAmount;//改变的数值
    public float sttChangeUseDelay;//使用间隔，即CD
    //public int sttNoOfUses;
    //=======Consumable=======

    //=======Projectile=======
    [Header("Projectile投掷物")]
    public GameObject prefabFireballVFX;//特效
    [Range(0, 7)]
    public int addtionalProjectiles = 0;//附加的投射物
    public float damageProjectile;//投射物的伤害
    public damageType damageTypeProjectile;//伤害类型
    public float manaCostProjectile;//蓝量消耗
    public Vector3 castPointLocalPosProjectile;//相对于投掷点的位置
    public float speedProjectile;//投掷物速度
    public float lifetimeProjectile;//投掷物生命周期
    public bool rigidbodyMovement = true;//移动方式，rigidBody或Translate
    [Range(0f, 100f)]
    public float piercing = 0f;//穿透概率，0-100，不在这个范围内，就是一定能穿透
    // linked skill will be executed on contact with and enemy or at the end of life, not all skills can be linked, for now AoE and DoT archetypes work.
    //连环技能将在与敌人接触或生命结束时执行，并不是所有的技能都可以作为连接技能，现在AoE和DoT原型可以工作。
    public aRPG_DB_MakeSkillSO linkedSkillProjectile1;
    // below bool determines whether projectile will execute link when 
    //下面bool决定投射物是否执行link，技能结束时是否连接技能
    public bool linkOnEndOfLife = true;
    //=======Projectile=======

    //=======AoE=======
    [Header("AoE")]
    public float AoEdamageDelay;//延迟后算伤害
    public float AoEdamage;//伤害
    public damageType AoEdamageType;//伤害类型
    public float AoEradius;//伤害范围
    public GameObject AoEprefabVFX;//特效
    public float AoEmanaCost;//蓝量消耗
    //=======AoE=======

    //=======Collider-DoT=======
    [Header("Collider-DoT")]
    public GameObject instantiatePrefab;//特效
    public float dps = 11f;//伤害
    public float damageFrequency = 0.02f;//伤害频率，每次伤害11*0.02，每0.02秒再伤害一次
    // below var is mana cost per second for single DoT's that are spawned at player hands. Or is a mana cost per single use in case of DoT spawned at certain location.
    //下面的var是玩家手上产生的单点每秒法力消耗。或者是在某个地点产生点的情况下每次使用的法力消耗。
    public float manaCostPerSecOrUse = 11f;//蓝量消耗，每秒或者一次
    public float lifetime = 1.5f;//技能生命周期
    public bool spawnAtCastPoint = true;//是否产生在发射点，如果是，就是射线哪样的特效持续的。不然就是直接出现在鼠标位置。移动端不支持出现在鼠标位置
    //=======Collider-DoT=======

    //=======Other=======
    [Header("Other")]
    /*variables for scripting operations*/
    /*脚本操作的变量*/
    internal bool delayIsOn = false;//技能已经开始了只是在延迟状态
    internal float currentSpriteFill;//转CD用的比例
    internal bool manaDegenIsOn = false;//持续消耗蓝
                                        /*variables for scripting operations*/

    /*Don't change these values unless you really need to*/
    // below define speed of visual updates of things like cooldown sprite fill or mana drain.
    /*不要改变这些值，除非你真的需要*/
    //下面定义了像冷却精灵填充或法力消耗之类的东西的视觉更新速度。
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
