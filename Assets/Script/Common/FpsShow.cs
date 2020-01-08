using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FpsShow : MonoBehaviour 
{
    private float updateInterval = 1.0f;
    private float lastInterval = 0;
    private int frames = 0;
    private float fps = 0;
    private float ms = 0;
    private string text = null;
	// Use this for initialization
	void Start () {

      
	}
	
	// Update is called once per frame
	void Update ()
    {
        frames++;
        float timenow = Time.realtimeSinceStartup;
        if(timenow>lastInterval+updateInterval)
        {
            fps = frames / (timenow - lastInterval);
            ms = 1000.0f / Mathf.Max(fps,0.00001f);
            frames = 0;
            lastInterval = timenow;
            
        }
     
            text ="FPS:"+ ((int)fps).ToString();
       
       
	}

    void OnGUI()
    {
       
         GUI.TextField(new Rect(5, 5, 80, 30), text);
         
    }
}
