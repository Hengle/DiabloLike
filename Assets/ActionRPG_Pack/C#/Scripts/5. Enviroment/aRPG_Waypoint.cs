using UnityEngine;
using System.Collections;

// This script manages only Waypoint shader outline. 
// It does NOT make player traverse to other scene - this is done in aRPG_GuiWaypoint when you press a gui button
// It does NOT make gui window appear - this is made in aRPG_Skills in WaypointClick function.

public class aRPG_Waypoint : MonoBehaviour {

    GameObject m;
    aRPG_Master ms;

    Ray ray;
    RaycastHit hit;

    bool dataSent = false;

	void Start () 
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
	}

    void Update()
    {
        CustomMouseOver();
    }

    void CustomMouseOver()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 60.0f, ms.layerInteractiveObject))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                if (dataSent == false)
                {
                    SetMaterialOutline(true);
                    dataSent = true;
                }
            }
            else
            {
                dataSent = false;
                SetMaterialOutline(false);
            }
        }
        else
        {
            SetMaterialOutline(false);
            dataSent = false;
        }
    }

    public void SetMaterialOutline(bool smBool)
    {
        if (smBool)
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.0015f);
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.0f);
        }
    }

}
