using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Action Button is a button that you press to execute skill/that represent skills bound to certain input skill/
// this script has to be attached to the GUI Action Button

public class aRPG_GuiButton_Action : MonoBehaviour {
    
    public buttonCharacter thisButton_Character;

    public aRPG_DB_MakeSkillSO startingSkill;
    [HideInInspector] public GameObject thisButton_AmmoCounter;
    [HideInInspector] public Text thisButton_AmmoCounterText;
    [HideInInspector] public GameObject thisButton_InputDisplay;
    public string thisButton_InputKey;
    
}
