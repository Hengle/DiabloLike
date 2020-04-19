using UnityEngine;
using System.Collections;

public class aRPG_DB_MakeAISO : ScriptableObject {

    public AiTypes aiArchetype;
    public SkillData spellToCast;
    [Header("MeleeAI")]
    public float meleeAttackRange;
    public float meleeAttackDamage;
    [Header("RangedAI")]
    public float rangedRange;
    //public float rangedGetAwayDistance;

	// Use this for initialization
	void Start () {
        spellToCast = DataManager.Instance.GetSkill(4);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
