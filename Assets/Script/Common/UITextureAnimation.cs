using UnityEngine;
using System.Collections;


public class UITextureAnimation : MonoBehaviour 
{

    public int uvAnimationTileX = 24; 
 //Here you can place the number of columns of your sheet.                            
 //The above sheet has 24 
public int uvAnimationTileY = 1; 
 //Here you can place the number of rows of your sheet.                          
 //The above sheet has 1 
public float framesPerSecond = 10.0f;  
public int loopTime   = 1;  
public int currentLoopTime   = 0;  
int lastIndex = -1; 
public float timer   = 0;
public Material m_mat = null;

	// Use this for initialization
	void Start () 
    {
       // timer = 0;
        // Size of every tile    
        Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);
  
        Vector2 offset =new  Vector2(0, 1.0f - size.y);

      //  m_mat = gameObject.GetComponent<RawImage>().material;
        m_mat.SetTextureOffset("_MainTex", offset);
        m_mat.SetTextureScale("_MainTex", size); 
	}
 
	// Update is called once per frame
	void Update () 
    {
	// Calculate index    
	timer += Time.deltaTime;    
	int index = (int)(timer * framesPerSecond);    
	// repeat when exhausting all frames    
	index = index % (uvAnimationTileX * uvAnimationTileY);        
	if (lastIndex == index)    	
		return;        
	if (loopTime > 0)    
	{    	
		if (index < lastIndex)        
		{            
			currentLoopTime ++;        
		}                 
		if ( currentLoopTime >= loopTime )        
		{        	
			Destroy(gameObject);         
		}    
	}

    lastIndex = index;        
    // Size of every tile    
    Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY); 
           
    // split into horizontal and vertical index    
    float uIndex = index % uvAnimationTileX;
    float vIndex = index / uvAnimationTileX;    
    // build offset    
    // v coordinate is the bottom of the image in opengl so we need to invert.    
    Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);        
    m_mat.SetTextureOffset ("_MainTex", offset);
    Vector2 size1 = new Vector2(1f, 1.0f / uvAnimationTileY);
    m_mat.SetTextureScale("_MainTex", size1);
	}
}
