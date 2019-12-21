using UnityEngine;
using System.Collections;

public class aRPG_EnemySpawnMaster : MonoBehaviour {
    // this script should be attached to the simple plane mesh created in unity(or just use prefab).
    // the way this spawner works is that every simple plane mesh created in unity has a lenght and wideness of 5 units, changing scale of the mesh will allow you to change spawn area. 
    // Example: if you place a plane in 0,0,0 position and a plane has a x and z scale(y scale is not important) equal to 2.0 - this means that it can spawn in rectangular radius which corners are in positions of X(-10,10) Z(-10,10).
    // So by changing position and scale of the plane/prefab you can decide where spawns will take place. Dont change rotation. Y position of the plane is where the spawn will happen so keep it slightly above the ground.
    // the graphic attached to the spawner prefab marks the spawn area.

    // by default rare has 3 mods and increased base stats + comes with minions that inherit his mods, champions come in packs and have 1 mod and increased base stats. But that can be changes as you like.


    public enum nodeTypeOptions { Random, Rare, Champions, Normal };
    public nodeTypeOptions nodeType = nodeTypeOptions.Rare;
    public int rareWeight = 1;
    public int champWeight = 1;
    public int normalWeight = 3;

    public GameObject monsterPrefab1;
    [Range(1, 20)] public int noOfMonsters1 = 5;
    [Range(0, 10)] public int noOfMonstersDeviation1 = 1;
    public GameObject monsterPrefab2;
    [Range(1, 20)] public int noOfMonsters2 = 2;
    [Range(0, 10)] public int noOfMonstersDeviation2 = 1;

    Vector3 spawnPosition;
    GameObject rareSpawnObjForMinions;
    GameObject previousChamp;

	void Awake() {
        // when dealing with Random please keep in mind that maximum number is exclusive - meaning if a Range is [1,3] only 1 and 2 can be generated.
        int noOfMonsters1random = Random.Range(noOfMonsters1 - noOfMonstersDeviation1, noOfMonsters1 + noOfMonstersDeviation1);
        int noOfMonsters2random = Random.Range(noOfMonsters2 - noOfMonstersDeviation2, noOfMonsters2 + noOfMonstersDeviation2);

        if (nodeType == nodeTypeOptions.Random)
        {
            // based on ***Weight variables, randomly generated number will determine the monster spawn rarity type
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

        if (nodeType == nodeTypeOptions.Champions)
        {
            // if in inspector you will not assign secondary monster type to the monsterPrefab2 variable Champions of type assigned to monsterPrefab1 will spawn...
            if (monsterPrefab2 == null)
            {
                Spawn(aRPG_EnemyStats.modsDefinition.Champion, noOfMonsters1random, monsterPrefab1);
            }
                // ...but if you will assign a secondary monster, script will randomly select between type one and two and only spawn champions of one type(that's more like a game design decision, you can change that as you like very easly)
            // ... the chance to spawn a specific monster type is determined by the noOfMonsters1 and noOfMonsters2 variables. If in inspector you will decide that you will spawn 6 of type1 and 1 of type2, chance to spawn type1 Champs will be 6 times higher, however in case of rolling less common type of monster their number will be an average of type1 and type2 number of spawns.
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

        if (nodeType == nodeTypeOptions.Rare)
        {
            // it works similar with Rares as with Champs
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

        if (nodeType == nodeTypeOptions.Normal)
        {
            Spawn(aRPG_EnemyStats.modsDefinition.Normal, noOfMonsters1random, monsterPrefab1);
            if (monsterPrefab2 != null) { Spawn(aRPG_EnemyStats.modsDefinition.Normal, noOfMonsters2random, monsterPrefab2); }
        }


        Destroy(gameObject);
	}

    // that function instantiates a monster, you have to input monster rarity, number of monsters to spawn and a prefab that will be spawned. 
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
