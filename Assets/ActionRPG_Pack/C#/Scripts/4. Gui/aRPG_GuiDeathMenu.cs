using UnityEngine;
using System.Collections;


public class aRPG_GuiDeathMenu : MonoBehaviour {
    GameObject m;
    aRPG_Master ms;

	void Start () {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

        gameObject.SetActive(false);
	}

    public void RespawnButton()
    {
        ms.Respawn();
    }
	
}
