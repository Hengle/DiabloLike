using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

// This is simplyfied version of Joystick script available in Unity's Standart Assets pack.

public class aRPG_Joystick : MonoBehaviour , IPointerUpHandler, IPointerDownHandler, IDragHandler {

    public int MovementRange = 10;
    
    private Vector3 startPos;

    [HideInInspector] public float joyPosX; 
    [HideInInspector] public float joyPosY;
    
    void Start () {
        startPos = transform.position;
    }
    

    private void UpdateVirtualAxes(Vector3 value)
    {

        var delta = startPos - value;
        delta.y = -delta.y;
        delta /= MovementRange;

        joyPosX  = - delta.x;
        
        joyPosY = delta.y;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = Vector3.zero;

        
            int delta = (int)(eventData.position.x - startPos.x);
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.x = delta;
        

        
            delta = (int)(eventData.position.y - startPos.y);
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.y = delta;
        
        transform.position = new Vector3(startPos.x + newPos.x, startPos.y + newPos.y, startPos.z + newPos.z);
        UpdateVirtualAxes(transform.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = startPos;
        UpdateVirtualAxes(startPos);
    }

}
