using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    //("General")]
    // skill archetype defines the skill. If you choose a Melee Sweep only variables from under Melee Sweep header will affect skill.
    //技能原型定义了技能。如果你选择一个近战扫荡只有变量从下面的近战扫荡头将影响技能。
    public int Id;
    public int skillArchetype;
    public string UNIQUE_skillName;
    public string sprite;//技能图标
    public string spriteNoMana;//没蓝时的技能图标
    public bool hasLimitedNoOfUses = false;//投掷物，是否消耗子弹
    public int ammo_amount;//弹药消耗

    //=======Melee Sweep=======
    public double damageModifierPercent;//伤害是武器伤害的，加成比 1是无加成
    public double arcWidth;//身前宽度，这是一半，左右各一半
    public double arcLength;//身前长度，
    //=======Melee Sweep=======

    //=======Consumable=======
    public int sttChange;//要改变的属性
    public double sttChangeDuration;//改变的时间//如果sttChangeDuration为0，则更改将是永久的。
    public double sttChangeAmount;//改变的数值
    public double sttChangeUseDelay;//使用间隔，即CD
    //public int sttNoOfUses;
    //=======Consumable=======

    //=======Projectile=======
    public string prefabFireballVFX;//特效
    public int addtionalProjectiles = 0;//附加的投射物
    public double damageProjectile;//投射物的伤害
    public int damageTypeProjectile;//伤害类型
    public double manaCostProjectile;//蓝量消耗
    public string castPointLocalPosProjectile;//相对于投掷点的位置
    public double speedProjectile;//投掷物速度
    public double lifetimeProjectile;//投掷物生命周期
    public bool rigidbodyMovement = true;//移动方式，rigidBody或Translate
    public double piercing = 0f;//穿透概率，0-100，不在这个范围内，就是一定能穿透
    public int linkedSkillProjectile1;
    public bool linkOnEndOfLife = true;
    //=======Projectile=======

    //=======AoE=======
    public double AoEdamageDelay;//延迟后算伤害
    public double AoEdamage;//伤害
    public int AoEdamageType;//伤害类型
    public double AoEradius;//伤害范围
    public string AoEprefabVFX;//特效
    public double AoEmanaCost;//蓝量消耗
    //=======AoE=======

    //=======Collider-DoT=======
    public string instantiatePrefab;//特效
    public double dps = 11f;//伤害
    public double damageFrequency = 0.02f;//伤害频率，每次伤害11*0.02，每0.02秒再伤害一次
    public double manaCostPerSecOrUse = 11f;//蓝量消耗，每秒或者一次
    public double lifetime = 1.5f;//技能生命周期
    public bool spawnAtCastPoint = true;//是否产生在发射点，如果是，就是射线哪样的特效持续的。不然就是直接出现在鼠标位置。移动端不支持出现在鼠标位置
    //=======Collider-DoT=======

     //=======Other=======
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

    private Object prefab;
    /// <summary>
    /// 获取prefab
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Object GetPrefab()
    {
        string name = prefabFireballVFX;
        if (skillArchetype == (int)archetype.AoE)
        {
            name = AoEprefabVFX;
        }else if (skillArchetype == (int)archetype.DoT)
        {
            name = instantiatePrefab;
        }
        if (prefab == null)
        {
            prefab = Resources.Load("C# Prefabs/InstantiatedByScript/" + name);
        }
        return prefab;
    }
    Vector3 castPointLocalPos = Vector3.positiveInfinity;
    public Vector3 CastPointLocalPos
    {
        get
        {
            if(castPointLocalPos == Vector3.positiveInfinity)
            {
                string[] arr = castPointLocalPosProjectile.Split(',');
                castPointLocalPos = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]));
            }
            return castPointLocalPos;
        }
    }
}
