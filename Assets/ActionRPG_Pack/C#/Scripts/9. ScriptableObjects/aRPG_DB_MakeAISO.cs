using UnityEngine;
using System.Collections;

public class aRPG_DB_MakeAISO : ScriptableObject {

    public AiTypes aiArchetype;
    public int spellToCastID = 4;
    [Header("MeleeAI")]
    public float meleeAttackRange;
    public float meleeAttackDamage;
    [Header("RangedAI")]
    public float rangedRange;
    //public float rangedGetAwayDistance;

	// Use this for initialization
	public SkillData spellToCast{
        get { return DataManager.Instance.GetSkill(spellToCastID); }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
