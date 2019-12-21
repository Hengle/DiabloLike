using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// This script need to be attached to a button responsible for sprite of a skill or an item button
// it cannot be either

public enum selectionButton_Character {Skill, Item}
public class aRPG_GuiButton_Selection : MonoBehaviour {

    public selectionButton_Character thisButtonCharacter;
    public aRPG_DB_MakeSkillSO skill;
    public aRPG_DB_MakeItemSO item;

	void Awake () {
        CheckSprites();
    }

    void CheckSprites()
    {
        if(thisButtonCharacter == selectionButton_Character.Skill)
        {
            if (skill != null) { gameObject.transform.Find("Icon").GetComponent<Image>().sprite = skill.sprite; }
        }
        if (thisButtonCharacter == selectionButton_Character.Item)
        {
            if (item != null) { gameObject.transform.Find("Icon").GetComponent<Image>().sprite = item.weaponIcon; }
        }
    }

}
