using UnityEngine;
using System.Collections;
/// <summary>
/// 角色属性
/// </summary>
public class aRPG_CharacterStats : MonoBehaviour {
    GameObject m;
    aRPG_Master ms;

    private float exp = 0.0f;
    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            DealExp();
        }
    }
    //below variables decide how much exp you'll need on higher levels.
    public enum expMath{Flat, Additive, Multiplicative};//平的、加法的、乘法的
    public expMath expToLevelUpProgression = expMath.Flat;
    [Range(1,3)] public float exp_ToLevelUp_Multiplier = 2.0f;
    [HideInInspector] public float prevExpToLevelUp = 0.0f;
    public float baseExpToLevelUp = 100.0f;
    [HideInInspector] public float expToLevelUp = 100.0f;
    [HideInInspector] public int level = 1;
    [HideInInspector] public float expBar;

    [SerializeField]
    public Attribute baseAttr = new Attribute();

    public Attribute curAttr = new Attribute();

    //public float maxHealth = 100.0f;
    //public float maxMana = 100.0f;

    //public float strenght = 1.0f;
    //public float constitution = 1.0f;
    //public float perception = 1.0f;
    //public float charisma = 1.0f;
    //public float intelligence = 1.0f;
    //public float luck = 1.0f;

    //public float fire_res;
    //public float magic_res;
    //public float physical_res;//物理防御，或者物理减免

    //public float manaRegenBonus = 0.0f;

    //public float blunt = 1.0f;
    //public float bladed = 1.0f;
    //public float small = 1.0f;
    //public float large = 1.0f;

    public float meleeArcSweep_arcWidth = 0.8f;
    public float meleeArcSweep_arcLength = 1.5f;

    //[HideInInspector] public float small_guns_skill_bonus;//bonus加成
    //[HideInInspector] public float large_guns_skill_bonus;
    //[HideInInspector] public float blunt_melee_skill_bonus;//blunt钝器
    //[HideInInspector] public float bladed_melee_skill_bonus;//bladed刀锋
    //[HideInInspector] public float melee_strenght_bonus;
    //[HideInInspector] public float ranged_perception_bonus;//perception感知

    void Awake()
    {
        expToLevelUp = baseExpToLevelUp;
    }
	
	void Start () {
        m = gameObject;
        ms = m.GetComponent<aRPG_Master>();

        GlobalExpansion.AttributeCopy(curAttr, baseAttr);

        RecalculateDerviedStats();
	}
	
	
	void Update () {
        // (Experience)
        // # expBar is used by GuiHealth script that updates level up progression bar.

	}
    //
    void DealExp()
    {
        expBar = ((exp - prevExpToLevelUp) / (expToLevelUp - prevExpToLevelUp)) * 100;
        if (exp > expToLevelUp)
        {
            if (expToLevelUpProgression == expMath.Flat)
            {
                LevelUpFlat();
}

            if (expToLevelUpProgression == expMath.Additive)
            {
                LevelUpAdditive();
            }

            if (expToLevelUpProgression == expMath.Multiplicative)
            {
                LevelUpMultiplicative();
            }
        }
    }
    void LevelUpFlat()
    // Flat exp progression is like: 100exp, 200exp, 300exp, 400exp and so on...
    {
        prevExpToLevelUp = expToLevelUp;
        expToLevelUp = prevExpToLevelUp + baseExpToLevelUp;
        Debug.Log("levelup");
        level++;
        OnLevelUp();
    }
    void LevelUpAdditive()
    {
        // Additive is like 100, 200, 400, 700...
        prevExpToLevelUp = expToLevelUp;
        expToLevelUp = prevExpToLevelUp + baseExpToLevelUp * level;
        Debug.Log("levelup");
        level++;
        OnLevelUp();
    }
    void LevelUpMultiplicative()
    {
        // in Multiplicative you choose multiplier that decide how much more exp you need compared to previous level. So 100, 300, 700, 1500 ...
        prevExpToLevelUp = expToLevelUp;
        expToLevelUp = prevExpToLevelUp * exp_ToLevelUp_Multiplier + baseExpToLevelUp;
        Debug.Log("levelup");
        level++;
        OnLevelUp();
    }
    void OnLevelUp()
    {
        // here you can code what will happen on leveling up, like UI functions, stats gains
        // Example:
        //maxHealth = maxHealth + constitution * 2;//升级属性增加
        //ms.psHealth.SimpleHeal(constitution * 2);//升级回血

        //strenght++;
        //constitution++;
        //perception++;
        //charisma++;
        //intelligence++;
        //luck++;

        //blunt++;
        //bladed++;
        //small++;
        //large++;
        /*
        强度++；
        体质++；
        感知++；
        魅力++；
        智能++；
        运气++；

        钝器++；
        刀片式++；
        小++；
        大++；
        */

        RecalculateDerviedStats();
    }

    // # Call this function whenever you an event can take place that will influence stats. For example when you put on an armour from backpack to ragdoll in inventory system that you created.
    //重新计算统计数据
    public void RecalculateDerviedStats()
    {
        //small_guns_skill_bonus = small / 20;
        //large_guns_skill_bonus = large / 20;
        //blunt_melee_skill_bonus = blunt / 20;
        //bladed_melee_skill_bonus = bladed / 20;

        //melee_strenght_bonus = strenght / 20;
        //ranged_perception_bonus = perception / 20;
    }

}
