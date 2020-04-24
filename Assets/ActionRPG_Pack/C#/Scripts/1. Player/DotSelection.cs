using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSelection : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] beamLineRendererPrefab;
    public GameObject[] beamStartPrefab;
    public GameObject[] beamEndPrefab;

    private int currentBeam = 0;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture

    float m_length;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(beamStart != null){
            line.SetPosition(0, beamStart.transform.position);
            line.SetPosition(1, beamEnd.transform.position);
            line.sharedMaterial.mainTextureScale = new Vector2(m_length / textureLengthScale, 1);
            line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
        }
    }


    public void SetEffect(int index, Vector3 start, Vector3 dir, float width, float length)
    {
        currentBeam = index;
        m_length = length;
        beamStart = Instantiate(beamStartPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEndPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beamLineRendererPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        beamStart.transform.SetParent(this.transform);
        beamEnd.transform.SetParent(this.transform);
        beam.transform.SetParent(this.transform);

        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = start + dir * length;
        //RaycastHit hit;
        //if (Physics.Raycast(start, dir, out hit))
        //    end = hit.point - (dir.normalized * beamEndOffset);
        //else
        //    end = transform.position + (dir * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        
        line.sharedMaterial.mainTextureScale = new Vector2(length / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
