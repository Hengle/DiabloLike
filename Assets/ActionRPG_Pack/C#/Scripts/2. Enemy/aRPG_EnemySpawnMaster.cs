using UnityEngine;
using System.Collections;

/// <summary>
/// 暂时用这个产怪
/// 一个稀有，带一群小怪，形成类似将军带兵的效果
/// 单一群小怪，就是一群兵
/// </summary>
public class aRPG_EnemySpawnMaster : MonoBehaviour {
    // this script should be attached to the simple plane mesh created in unity(or just use prefab).
    // the way this spawner works is that every simple plane mesh created in unity has a lenght and wideness of 5 units, changing scale of the mesh will allow you to change spawn area. 
    // Example: if you place a plane in 0,0,0 position and a plane has a x and z scale(y scale is not important) equal to 2.0 - this means that it can spawn in rectangular radius which corners are in positions of X(-10,10) Z(-10,10).
    // So by changing position and scale of the plane/prefab you can decide where spawns will take place. Dont change rotation. Y position of the plane is where the spawn will happen so keep it slightly above the ground.
    // the graphic attached to the spawner prefab marks the spawn area.

    // by default rare has 3 mods and increased base stats + comes with minions that inherit his mods, champions come in packs and have 1 mod and increased base stats. But that can be changes as you like.
    //这个脚本应该附加到unity中创建的简单的平面网格(或者只使用预制)。
    //这个衍生工具的工作方式是在unity中创建的每一个简单的平面网格都有5个单位的长度和宽度，改变网格的比例将允许你改变衍生区域。
    //例子:如果你把一个平面放在0,0,0的位置，一个平面有一个x和z的范围(y的范围不重要)等于2.0 -这意味着它可以在矩形半径内衍生出角在x (-10,10) z(-10,10)的位置。
    //因此，通过改变平面/预制板的位置和比例，你可以决定在哪里产卵。不改变旋转。平面的Y位置是产卵发生的地方，所以保持它略高于地面。
    //附在产卵器预置上的图形标志着产卵区。
    //默认情况下rare有3个mod和增加的基础属性+附带的仆从继承了他的mod，而冠军则是一个mod和增加的基础属性。但你可以随心所欲地改变。

    public enum nodeTypeOptions { Random, Rare, Champions, Normal };//产生怪的类型，第一个是随机什么怪
    public nodeTypeOptions nodeType = nodeTypeOptions.Rare;
    public int rareWeight = 1;//稀有权重，这三个参数是为随机准备的
    public int champWeight = 1;
    public int normalWeight = 3;

    public GameObject monsterPrefab1;
    [Range(1, 20)] public int noOfMonsters1 = 5;
    [Range(0, 10)] public int noOfMonstersDeviation1 = 1;//Deviation偏差，在基础数量noOfMonsters1上产生一定偏差，
    public GameObject monsterPrefab2;
    [Range(1, 20)] public int noOfMonsters2 = 2;
    [Range(0, 10)] public int noOfMonstersDeviation2 = 1;

    Vector3 spawnPosition;
    GameObject rareSpawnObjForMinions;//Minion 下属，小卒
    GameObject previousChamp;

	void Awake() {
        // when dealing with Random please keep in mind that maximum number is exclusive - meaning if a Range is [1,3] only 1 and 2 can be generated.
        //在处理随机数时，请记住，最大值是唯一的，也就是说，如果一个范围是[1,3]，那么只能生成1和2。
        int noOfMonsters1random = Random.Range(noOfMonsters1 - noOfMonstersDeviation1, noOfMonsters1 + noOfMonstersDeviation1);
        int noOfMonsters2random = Random.Range(noOfMonsters2 - noOfMonstersDeviation2, noOfMonsters2 + noOfMonstersDeviation2);

        if (nodeType == nodeTypeOptions.Random)
        {
            // based on ***Weight variables, randomly generated number will determine the monster spawn rarity type
            //根据***的权重变量，随机生成的数量将决定怪物产卵的稀有性类型
            //根据权重，随机产什么怪，
            int weightsSum = rareWeight + champWeight + normalWeight;
            int randomNumber1 = Random.Range(1,weightsSum+1);
            if (randomNumber1 <= normalWeight + champWeight + rareWeight)
            {
                nodeType = nodeTypeOptions.Normal;
            }
            if (randomNumber1 <= champWeight + rareWeight)
            {
                nodeType = nodeTypeOptions.Champions;
            }
            if (randomNumber1 <= rareWeight)
            {
                nodeType = nodeTypeOptions.Rare;
            }
        }
        //冠军,就是几个Champions
        if (nodeType == nodeTypeOptions.Champions)
        {
            // if in inspector you will not assign secondary monster type to the monsterPrefab2 variable Champions of type assigned to monsterPrefab1 will spawn...
            //如果在inspector中你不给怪物类型分配二级怪物，那么分配给怪物prefab1类型的变量冠军将会衍生…
            if (monsterPrefab2 == null)
            {
                Spawn(aRPG_EnemyStats.modsDefinition.Champion, noOfMonsters1random, monsterPrefab1);
            }
                // ...but if you will assign a secondary monster, script will randomly select between type one and two and only spawn champions of one type(that's more like a game design decision, you can change that as you like very easly)
            // ... the chance to spawn a specific monster type is determined by the noOfMonsters1 and noOfMonsters2 variables. If in inspector you will decide that you will spawn 6 of type1 and 1 of type2, chance to spawn type1 Champs will be 6 times higher, however in case of rolling less common type of monster their number will be an average of type1 and type2 number of spawns.
            //……但是如果你要分配一个次级怪物，脚本会在类型1和类型2之间随机选择，并且只产生一个类型的冠军(这更像是一个游戏设计决策，你可以很容易地改变它)
            //……生成特定怪物类型的机会由noOfMonsters1和noOfMonsters2变量决定。如果在检查器中你决定你将衍生出6个1型和1个2型，那么衍生出1型冠军的几率将是6倍，然而在滚动不太常见的怪物时，它们的数量将是1型和2型怪物数量的平均值。
            else
            {
                int rChamps = Random.Range(1, noOfMonsters1 + noOfMonsters2 + 1);
                if (rChamps <= noOfMonsters1)
                {
                    Spawn(aRPG_EnemyStats.modsDefinition.Champion, (noOfMonsters1random + noOfMonsters2random)/2, monsterPrefab1);
                }
                else
                {
                    Spawn(aRPG_EnemyStats.modsDefinition.Champion, (noOfMonsters1random + noOfMonsters2random)/2, monsterPrefab2);
                }
            }
        }
        //稀有的是，一个稀有的，几个小弟，或者几个小弟几个一般的。
        if (nodeType == nodeTypeOptions.Rare)
        {
            // it works similar with Rares as with Champs
            //它对稀有和冠军的作用是相似的
            if (monsterPrefab2 == null)
            {
                Spawn(aRPG_EnemyStats.modsDefinition.Rare, 1, monsterPrefab1);
                Spawn(aRPG_EnemyStats.modsDefinition.RareMinion, noOfMonsters1random - 1, monsterPrefab1);
            }
            else
            {
                int rRares = Random.Range(1, noOfMonsters1 + noOfMonsters2 + 1);
                if (rRares <= noOfMonsters1)
                {
                    Spawn(aRPG_EnemyStats.modsDefinition.Rare, 1, monsterPrefab1);
                    Spawn(aRPG_EnemyStats.modsDefinition.RareMinion, (noOfMonsters1random + noOfMonsters2random) / 2, monsterPrefab1);
                    Spawn(aRPG_EnemyStats.modsDefinition.Normal, (noOfMonsters1random + noOfMonsters2random) / 4, monsterPrefab2);
                }
                else
                {
                    Spawn(aRPG_EnemyStats.modsDefinition.Rare, 1, monsterPrefab2);
                    Spawn(aRPG_EnemyStats.modsDefinition.RareMinion, (noOfMonsters1random + noOfMonsters2random) / 2, monsterPrefab2);
                    Spawn(aRPG_EnemyStats.modsDefinition.Normal, (noOfMonsters1random + noOfMonsters2random) / 4, monsterPrefab1);
                }
            }
        }
        //一般的，就是几个
        if (nodeType == nodeTypeOptions.Normal)
        {
            Spawn(aRPG_EnemyStats.modsDefinition.Normal, noOfMonsters1random, monsterPrefab1);
            if (monsterPrefab2 != null) { Spawn(aRPG_EnemyStats.modsDefinition.Normal, noOfMonsters2random, monsterPrefab2); }
        }

        //怪物生成就删除产怪点
        Destroy(gameObject);
	}

    // that function instantiates a monster, you have to input monster rarity, number of monsters to spawn and a prefab that will be spawned. 
    //该函数实例化一个怪物，你必须输入怪物的稀有性，怪物的数量产卵和预制将被产卵。
    void Spawn(aRPG_EnemyStats.modsDefinition spawnedMonstertype, int numberOfSpawns, GameObject monsterPrefabToSpawn)
    {
        float nodeScaleX = Mathf.Abs(transform.localScale.x);
        float nodeScaleZ = Mathf.Abs(transform.localScale.z);

        for (int i = 0; i < numberOfSpawns; i++)
        {
            float randomModX = Random.Range(nodeScaleX * (5), nodeScaleX * (-5));
            float randomModZ = Random.Range(nodeScaleZ * (5), nodeScaleZ * (-5));
            float randomX = transform.position.x + randomModX;
            float randomZ = transform.position.z + randomModZ;
            spawnPosition = new Vector3(randomX, transform.position.y, randomZ);
            GameObject spawn = Instantiate(monsterPrefabToSpawn, spawnPosition, Quaternion.identity) as GameObject;
            aRPG_EnemyStats spawnStatsScript = spawn.GetComponent<aRPG_EnemyStats>();
            spawnStatsScript.isRandomlySpawned = true;
            spawnStatsScript.monsterModsDefinition = spawnedMonstertype;

            if (spawnedMonstertype == aRPG_EnemyStats.modsDefinition.Rare)
            {
                spawnStatsScript.MakeItRare();
                rareSpawnObjForMinions = spawn;
            }
            if (spawnedMonstertype == aRPG_EnemyStats.modsDefinition.RareMinion && rareSpawnObjForMinions != null)
            {
                spawnStatsScript.rareObjForMinions = rareSpawnObjForMinions;
                spawnStatsScript.MakeItMinion();
            }
            if (spawnedMonstertype == aRPG_EnemyStats.modsDefinition.Champion)
            {
                if (previousChamp == null)
                {
                    spawnStatsScript.MakeItChamp();
                }
                else
                {
                    spawnStatsScript.previousChamp = previousChamp;
                    spawnStatsScript.ClonePreviousChamp();
                }

                previousChamp = spawn;


            }


        }

    }



}
