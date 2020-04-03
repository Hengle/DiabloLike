using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// this script manages enemy health bar and text below it - it does not decides how much health a monster has or what text exaclty should be displayed.
// visibility of enemy health bar is managed in aRPG_EnemyMouseOver.

public class aRPG_GuiEnemyInfo : MonoBehaviour {

    [HideInInspector] public GameObject enemyHPpanel;

    GameObject hpBar;
    Image hpBar_slider;

    GameObject textMods;
    Text textMods_text;

    GameObject enemy;
    aRPG_EnemyStats enemyHealthScript;
    string enemyName;

	void Awake () {
        enemyHPpanel = GameObject.Find("MainCanvas/EnemyHPpanel_@");
        hpBar = GameObject.Find("MainCanvas/EnemyHPpanel_@/HPbar_@");
        textMods = GameObject.Find("MainCanvas/EnemyHPpanel_@/Text_Mods_@");

        hpBar_slider = hpBar.GetComponent<Image>();
        textMods_text = textMods.GetComponent<Text>();
	}

    void Start()
    {
        enemyHPpanel = GameObject.Find("MainCanvas/EnemyHPpanel_@");
        hpBar = GameObject.Find("MainCanvas/EnemyHPpanel_@/HPbar_@");
        textMods = GameObject.Find("MainCanvas/EnemyHPpanel_@/Text_Mods_@");

        hpBar_slider = hpBar.GetComponent<Image>();
        textMods_text = textMods.GetComponent<Text>();
        enemyHPpanel.SetActive(false);
    }

	void Update () {
        if (enemyHealthScript != null)
        {
            hpBar_slider.fillAmount = enemyHealthScript.curAttr.Health / enemyHealthScript.baseAttr.Health;
            if (enemyHealthScript.isDead)
            {
                enemyHPpanel.SetActive(false);
            }
        }
        
	}

    // # this function is called when a cursor is over an enemy by aRPG_EnemyMouseOver. It gets the information on the enemy and sets variables based on that.
    public void GetTargetEnemy(GameObject receivedEnemy)
    {
        enemy = receivedEnemy;
        enemyHealthScript = enemy.GetComponent<aRPG_EnemyStats>();
        enemyName = enemyHealthScript.thisName;
        textMods_text.text = enemyName;
        textMods_text.color = Color.white;
        if (enemyHealthScript.monsterModsDefinition == aRPG_EnemyStats.modsDefinition.Rare)
        {
            textMods_text.color = Color.yellow;
        }
        if (enemyHealthScript.monsterModsDefinition == aRPG_EnemyStats.modsDefinition.Champion)
        {
            textMods_text.color = Color.cyan;
        }
    }


}
