using UnityEngine;
using System.Collections;

public class aRPG_DB_MakeAISO : ScriptableObject {

    public AiTypes aiArchetype;
    public aRPG_DB_MakeSkillSO spellToCast;
    [Header("MeleeAI")]
    public float meleeAttackRange;
    public float meleeAttackDamage;
    [Header("RangedAI")]
    public float rangedRange;
    //public float rangedGetAwayDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
