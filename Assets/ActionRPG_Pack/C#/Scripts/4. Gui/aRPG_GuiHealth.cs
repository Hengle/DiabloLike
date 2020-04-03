using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// this script only updates the fill of health and mana globe and exp bar. 

public class aRPG_GuiHealth : MonoBehaviour {

    GameObject m;
    aRPG_Master ms;

    Image hbBar;
    Image manaBar;
    Slider slider;
    
	void Start () 
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

        hbBar = GameObject.Find("MainCanvas/HealthGlobe_@").GetComponent<Image>();
        manaBar = GameObject.Find("MainCanvas/ManaGlobe_@").GetComponent<Image>();
        slider = GameObject.Find("MainCanvas/ExpBar_@").GetComponent<Slider>();
	}
	
	void Update () 
    {
        hbBar.fillAmount = ms.psStats.curAttr.Health / ms.psStats.baseAttr.Health;
        manaBar.fillAmount = ms.psStats.curAttr.Mana / ms.psStats.baseAttr.Mana;
        slider.value = ms.psStats.expBar;
	}
}
