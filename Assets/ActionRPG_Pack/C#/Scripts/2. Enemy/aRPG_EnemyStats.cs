using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AiTypes { Melee, SpellCaster };
public class aRPG_EnemyStats : MonoBehaviour {
    // this class manages stats, rarity of the monster, health, death functionality, damage calculation(no damage implementation however), name, experience gains.
    GameObject m;
    aRPG_Master ms;

    UnityEngine.AI.NavMeshAgent eAgent;
    aRPG_EnemyMovement esMovement;
    CharacterController charContrl;
    aRPG_EnemyMouseOver esMouseOver;
    Animator eAnimator;
    GameObject mouseCollider;
    [HideInInspector] public bool isDead = false;
    bool rewardGiven = false;
    [HideInInspector] public string thisName;


    // these are the base stats for every enemy you create, have fun with these and create new monsters.
    float dmgToDeal;
    public float expReward;
    internal float currentHealth;
    public float max_health = 100.0f;
    float bonusDamageFloat;
    
    // if you want to create custom monster select Custom in monsterModsDefinition and mark below boolean variables.
    // Float variables below influence manually and randomly spawned monsters.
    [Header("Enchantments")]
    public bool fastAttribute = false;
    public float speed_bonus = 0.25f;
    public bool extra_dmgAttribute = false;
    public float extra_dmg_bonus = 0.33f;
    public float stunChance_multiplier = 4f;
    public bool extra_HPAttribute = false;
    public float extra_HP_bonus = 0.6f;
    
    public bool fireEnchanted = false;
    public float fire_dmg_bonus;
    public float fire_res;
    public bool physicalEnchanted = false;
    public float physical_dmg_bonus;
    public float physical_res;
    public bool magicEnchanted = false;
    public float magic_dmg_bonus;
    public float magic_res;

    public bool stunsTarget = true;
    public float stunChance = 0.05f;
    // if you put mobs on the scene manually, below you can choose what kind of monster it will be.
    public enum modsDefinition { Rare, RareMinion, Champion, Normal, Custom };
    public modsDefinition monsterModsDefinition;
    // when you want to manually place rare minions on the scene, select RareMinions in monsterModsDefinition and put rare object in below variable, spawned minions will inherit its mods.
    public GameObject rareObjForMinions;
    // when placing champions manually on the scene leave this empty for the first champ, and for each next put the first champ in previousChamp var, this will make all the champion inherit mods from the first one and you will have a stack of same champions.
    public GameObject previousChamp;
    [HideInInspector]
    public bool isRandomlySpawned = false;
    public float rareHPmultiplier = 5.0f;
    public float champHPmultiplier = 2.5f;

	void Start () {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

	    eAnimator = gameObject.GetComponent<Animator>();
	    esMovement = gameObject.GetComponent<aRPG_EnemyMovement>();
	    charContrl = gameObject.GetComponent<CharacterController>();
	    mouseCollider = gameObject.transform.Find("ColliderMouse").gameObject;
        esMouseOver = mouseCollider.GetComponent<aRPG_EnemyMouseOver>();
	    eAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (isRandomlySpawned == false)
        {
            if (monsterModsDefinition == modsDefinition.Rare)
            {
                MakeItRare();
            }

            if (monsterModsDefinition == modsDefinition.Champion)
            {
                if (previousChamp == null)
                {
                    MakeItChamp();
                }
                else
                {
                    ClonePreviousChamp();
                }
            }

            if (monsterModsDefinition == modsDefinition.RareMinion && rareObjForMinions != null)
            {
                MakeItMinion();
            }

        }

        
            ImplementPresets();
        

        currentHealth = max_health;
	}

    // update is only used to check if monster should be alive.
	void Update () {
        if (currentHealth > max_health) { currentHealth = max_health; }
	    if(currentHealth <= 0 && !isDead){

		Dead();
		}
	}

    // next four functions are more like a technical functions - called on specific time will make a enemy rare/champ/minion. 
    public void MakeItRare()
    {
        GenerateRandomAttributes(3);
        max_health = max_health * rareHPmultiplier;


        mouseCollider = gameObject.transform.Find("ColliderMouse").gameObject;
        esMouseOver = mouseCollider.GetComponent<aRPG_EnemyMouseOver>();
        esMouseOver.SetMaterials();
        esMouseOver.SetMaterialsColors("rare");
    }

    public void MakeItChamp()
    {
        GenerateRandomAttributes(1);
        max_health = max_health * champHPmultiplier;

        mouseCollider = gameObject.transform.Find("ColliderMouse").gameObject;
        esMouseOver = mouseCollider.GetComponent<aRPG_EnemyMouseOver>();
        esMouseOver.SetMaterials();
        esMouseOver.SetMaterialsColors("blue");

    }

    public void ClonePreviousChamp()
    {
        aRPG_EnemyStats prevChampScript = previousChamp.GetComponent<aRPG_EnemyStats>();
        fastAttribute = prevChampScript.fastAttribute;
        extra_dmgAttribute = prevChampScript.extra_dmgAttribute;
        extra_HPAttribute = prevChampScript.extra_HPAttribute;
        fireEnchanted = prevChampScript.fireEnchanted;
        physicalEnchanted = prevChampScript.physicalEnchanted;
        magicEnchanted = prevChampScript.magicEnchanted;

        max_health = max_health * champHPmultiplier;

        thisName = prevChampScript.thisName;

        mouseCollider = gameObject.transform.Find("ColliderMouse").gameObject;
        esMouseOver = mouseCollider.GetComponent<aRPG_EnemyMouseOver>();
        esMouseOver.SetMaterials();
        esMouseOver.SetMaterialsColors("blue");
    }

    public void MakeItMinion()
    {
        aRPG_EnemyStats rareObjForMinionsScript = rareObjForMinions.GetComponent<aRPG_EnemyStats>();
        fastAttribute = rareObjForMinionsScript.fastAttribute;
        extra_dmgAttribute = rareObjForMinionsScript.extra_dmgAttribute;
        extra_HPAttribute = rareObjForMinionsScript.extra_HPAttribute;
        fireEnchanted = rareObjForMinionsScript.fireEnchanted;
        physicalEnchanted = rareObjForMinionsScript.physicalEnchanted;
        magicEnchanted = rareObjForMinionsScript.magicEnchanted;

        thisName = "Minion";
    }

    // this function randomly generates mods for rare/champions. Here you can add you own mods following the pattern, no need to change any other function to add a new mod.
    void GenerateRandomAttributes(int numberOfModsToGenerate)
    {
        int numberOfModsGenerated = 0;
        int firstGeneratedModNo = 0;
        int secondGeneratedModNo = 0;
        while (numberOfModsGenerated < numberOfModsToGenerate)
        {
            float random_att_no1;
            do{
                random_att_no1 = Random.Range(1, 7);

            } while (random_att_no1 == firstGeneratedModNo || random_att_no1 == secondGeneratedModNo);


            if (random_att_no1 == 1)
            {
                fastAttribute = true;
                GenerateName("Fast");
                if (firstGeneratedModNo == 0)
                {
                    firstGeneratedModNo = 1;
                }
                if (secondGeneratedModNo == 0 && firstGeneratedModNo != 1)
                {
                    secondGeneratedModNo = 1;
                }
                numberOfModsGenerated++;
                //att1
            }
            if (random_att_no1 == 2)
            {
                extra_dmgAttribute = true;
                GenerateName("Extra Damage");
                if (firstGeneratedModNo == 0)
                {
                    firstGeneratedModNo = 2;
                }
                if (secondGeneratedModNo == 0 && firstGeneratedModNo != 2)
                {
                    secondGeneratedModNo = 2;
                }
                numberOfModsGenerated++;
                //att2
            }
            if (random_att_no1 == 3)
            {
                extra_HPAttribute = true;
                GenerateName("Extra Life");
                if (firstGeneratedModNo == 0)
                {
                    firstGeneratedModNo = 3;
                }
                if (secondGeneratedModNo == 0 && firstGeneratedModNo != 3)
                {
                    secondGeneratedModNo = 3;
                }
                numberOfModsGenerated++;
                //att3
            }
            if (random_att_no1 == 4)
            {
                fireEnchanted = true;
                GenerateName("Fire");
                if (firstGeneratedModNo == 0)
                {
                    firstGeneratedModNo = 4;
                }
                if (secondGeneratedModNo == 0 && firstGeneratedModNo != 4)
                {
                    secondGeneratedModNo = 4;
                }
                numberOfModsGenerated++;
                //att4
            }
            if (random_att_no1 == 5)
            {
                physicalEnchanted = true;
                GenerateName("Physical");
                if (firstGeneratedModNo == 0)
                {
                    firstGeneratedModNo = 5;
                }
                if (secondGeneratedModNo == 0 && firstGeneratedModNo != 5)
                {
                    secondGeneratedModNo = 5;
                }
                numberOfModsGenerated++;
                //att5
            }
            if (random_att_no1 == 6)
            {
                magicEnchanted = true;
                GenerateName("Magic");
                if (firstGeneratedModNo == 0)
                {
                    firstGeneratedModNo = 6;
                }
                if (secondGeneratedModNo == 0 && firstGeneratedModNo != 6)
                {
                    secondGeneratedModNo = 6;
                }
                numberOfModsGenerated++;
                //att6
            }

        }
    }

    // it implements mods to the enemy(simple check boolean in inspector is not enought, this function has to be ran).
    public void ImplementPresets()
    {
        if (fastAttribute == false){ speed_bonus = 0f;}
        else 
        {
            esMovement.rotationSpeed = esMovement.rotationSpeed + (2 * esMovement.rotationSpeed * speed_bonus);
            eAnimator.speed = eAnimator.speed + speed_bonus;
        }

        if (extra_dmgAttribute == false){ extra_dmg_bonus = 0f; }
        else 
        { 
            bonusDamageFloat = esMovement.ai.meleeAttackDamage + esMovement.ai.meleeAttackDamage * extra_dmg_bonus;
            stunChance = stunChance * stunChance_multiplier;
        }

        if (extra_HPAttribute == false) { extra_HP_bonus = 0f;}
        else { max_health = max_health + max_health * extra_HP_bonus;
        
        }

        if (fireEnchanted == false)
        {
            fire_res = 0f;
            fire_dmg_bonus = 0f;
        }
        if (physicalEnchanted == false)
        {
            physical_res = 0f;
            physical_dmg_bonus = 0f;
        }
        if (magicEnchanted == false)
        {
            magic_res = 0f;
            magic_dmg_bonus = 0f;
        }
    }

    // it is called by many function that generate mods, it then gives a monster a proper sub-name that is displayed below health bar.
    void GenerateName(string addName)
    {
        thisName = thisName + addName + " ";
    }

    // calculates damage dealt to the player based on his resistances and enemy bonuses.
    public float DealDamage()
    {
        dmgToDeal = esMovement.ai.meleeAttackDamage + bonusDamageFloat;

        dmgToDeal += ((esMovement.ai.meleeAttackDamage + bonusDamageFloat) * fire_dmg_bonus) - (((esMovement.ai.meleeAttackDamage + bonusDamageFloat) * fire_dmg_bonus) * ms.psStats.fire_res);

        dmgToDeal += ((esMovement.ai.meleeAttackDamage + bonusDamageFloat) * physical_dmg_bonus) - (((esMovement.ai.meleeAttackDamage + bonusDamageFloat) * physical_dmg_bonus) * ms.psStats.physical_res);

        dmgToDeal += ((esMovement.ai.meleeAttackDamage + bonusDamageFloat) * magic_dmg_bonus) - (((esMovement.ai.meleeAttackDamage + bonusDamageFloat) * magic_dmg_bonus) * ms.psStats.magic_res);
        
        return dmgToDeal;
    }

    public float FireballDealDamage(float fireballBaseDmg)
    {
        float dmgToDealFb = fireballBaseDmg + (fireballBaseDmg * fire_dmg_bonus);

        dmgToDealFb = dmgToDealFb - (dmgToDealFb * ms.psStats.fire_res);

        return dmgToDealFb;
    }
    // called by the skills on collision with enemies to calculate how much damage should be dealt, based on different player and enemy mods
    public float ReceiveDamage(damageType dmgType, float dmgAmount)
    {
        if (dmgType == damageType.Fire)
        {
            return dmgAmount - dmgAmount*fire_res ;
        }
        if (dmgType == damageType.Magic)
        {
            return dmgAmount - dmgAmount * magic_res;
        }
        if (dmgType == damageType.Physical)
        {
            return dmgAmount - dmgAmount * physical_res;
        }
        Debug.Log("wrong damage type was passed to ReceiveDamage function");
        return 0;
    }

    // is called when enemy HP reaches 0. It turns off all important components.
    void Dead(){
	    charContrl.enabled = false;
        Destroy(charContrl);
        Destroy(mouseCollider);

        GiveExp();
        esMouseOver.SetMaterialOutline(false);
	    eAnimator.SetBool("DeadBool", true);
	    eAnimator.SetFloat("Move", 0.0f);
        esMovement.ResetTriggers();

	    eAgent.enabled = false;
	    esMovement.enabled = false;
	    
	    Destroy(gameObject.GetComponent<Rigidbody>());
	    isDead = true;

        StartCoroutine(TimedDestroy(3f));
	}

    // it is called by Dead function to get rid of corpses.
    IEnumerator TimedDestroy(float time)
    {

        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
	
    // is called by Dead function to give experience to the player for killing it.
    void GiveExp(){
	if(rewardGiven == false){
        ms.psStats.exp = ms.psStats.exp + expReward;
		rewardGiven = true;
		}
	
	}

}

