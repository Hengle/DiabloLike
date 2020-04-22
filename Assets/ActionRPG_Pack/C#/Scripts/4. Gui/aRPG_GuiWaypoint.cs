using UnityEngine;
using System.Collections;

// It is required to add scenes to the build to use this script and waypoint functionality. Also mind the strings.
// It holds a function for Waypoint Menu buttons.

public class aRPG_GuiWaypoint : MonoBehaviour {
 //   GameObject m;
 //   aRPG_Master ms;

 //   GameObject buttonLoadSmall;
 //   GameObject buttonLoadLarge;
 //   GameObject buttonLoadDoor;

	//void Start () {
 //       m = GameObject.Find("SCRIPTS");
 //       ms = m.GetComponent<aRPG_Master>();

 //       ms.waypointMenu.SetActive(false);
 //       buttonLoadSmall = ms.waypointMenu.transform.Find("LevelSmall_@#").gameObject;
 //       buttonLoadLarge = ms.waypointMenu.transform.Find("LevelLarge_@#").gameObject;
 //       buttonLoadDoor = ms.waypointMenu.transform.Find("LevelDoor_@#").gameObject;
	//}
	
	//void Update () {
 //       if (ms.waypointMenu.activeInHierarchy)
 //       {
 //           if (Application.loadedLevelName == "1.Level_Small")
 //           {
 //               buttonLoadSmall.SetActive(false);
 //           }
 //           else { buttonLoadSmall.SetActive(true); }

 //           if (Application.loadedLevelName == "3.Level_Large")
 //           {
 //               buttonLoadLarge.SetActive(false);
 //           }
 //           else { buttonLoadLarge.SetActive(true); }

 //           if (Application.loadedLevelName == "2.Level_Doors")
 //           {
 //               buttonLoadDoor.SetActive(false);
 //           }
 //           else { buttonLoadDoor.SetActive(true); }

 //           if (ms.psSkills.wp != null)
 //           {
 //               if (Vector3.Distance(ms.psSkills.wp.transform.position, ms.player.transform.position) > ms.psSkills.meleeRange)
 //               {
 //                   ms.waypointMenu.SetActive(false);
 //               }
 //           }
 //       }


	//}

 //   // # is called by the Waypoint Menu buttons on press. Mind the strings.
 //   public void OnWpButtonPress(GameObject wpButtonPressed)
 //   {
 //       if (wpButtonPressed.name == "LevelSmall_@#")
 //       {
 //           ms.waypointMenu.SetActive(false);
 //           Application.LoadLevel("1.Level_Small");
 //       }
 //       if (wpButtonPressed.name == "LevelLarge_@#")
 //       {
 //           ms.waypointMenu.SetActive(false);
 //           Application.LoadLevel("3.Level_Large");
 //       }
 //       if (wpButtonPressed.name == "LevelDoor_@#")
 //       {
 //           ms.waypointMenu.SetActive(false);
 //           Application.LoadLevel("2.Level_Doors");
 //       }
 //   }

}
