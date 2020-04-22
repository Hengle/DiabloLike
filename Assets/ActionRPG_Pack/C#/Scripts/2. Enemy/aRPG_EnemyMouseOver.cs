using UnityEngine;
using System.Collections;

/// <summary>
/// 该函数处理鼠标在敌人上方移动的视觉表示-它确保显示正确的健康栏和着色器显示轮廓。
/// 它还具有一个功能，可以根据怪物的稀有性更改着色器。
/// 此处不管理鼠标输入。
/// 效率不怎么样
/// </summary>
public class aRPG_EnemyMouseOver : MonoBehaviour {
    // That function takes care of visual presentation of mouse movement over enemies - it makes sure that proper health bar is displayed and shader displays an outline.
    // It also has a function that changes shader based on monster rarity.
    // No mouse input is managed here.
    GameObject m;
    aRPG_Master ms;

    GameObject hpBar;
    GameObject textField;

    Ray ray;
    RaycastHit hit;
    bool dataSent = false;
    GameObject model;
    Material material1;
    Material material2;
    Material material3;
    Material material4;
    int noOfmats;
    float colliderRadius;
    CapsuleCollider collider;

    public bool increaseColliderRadiusOnMouseOver = true;

    void Start()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();

        collider = gameObject.GetComponent<CapsuleCollider>();
        colliderRadius = collider.radius;
        SetMaterials();
    }

    void Update()
    {
        CustomMouseOver();
    }

    void CustomMouseOver()
    {
        if (!model.GetComponent<Renderer>().isVisible) { return; }
        //if (ms.mcsEnemyInfo == null || ms.mcsEnemyInfo.enemyHPpanel == null) { return; }//屏幕上部中间怪物血条

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 60.0f, ms.layerEnemyMouseCollider))
        {
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                if (dataSent == false)
                {
                    //ms.mcsEnemyInfo.enemyHPpanel.SetActive(true);
                    SetMaterialOutline(true);
                    SpecialUIManager.Instance.main.GetTargetEnemy(gameObject.transform.parent.gameObject);
                    if (increaseColliderRadiusOnMouseOver) { collider.radius = colliderRadius * 1.4f; }
                    dataSent = true;
                }
            }
            else
            {
                dataSent = false;
                SetMaterialOutline(false);
                collider.radius = colliderRadius;
            }
        }
        else
        {
            //ms.mcsEnemyInfo.enemyHPpanel.SetActive(false);
            SetMaterialOutline(false);
            dataSent = false;
            collider.radius = colliderRadius;
        }
    }

    // technical function, has to be called very early to connect materials to the variables.
    public void SetMaterials()
    {
        model = gameObject.transform.parent.gameObject.transform.Find("3DModel").gameObject;
        noOfmats = model.GetComponent<Renderer>().materials.Length;
        if (noOfmats > 0) { material1 = model.GetComponent<Renderer>().materials[0]; }
        if (noOfmats > 1) { material2 = model.GetComponent<Renderer>().materials[1]; }
        if (noOfmats > 2) { material3 = model.GetComponent<Renderer>().materials[2]; }
        if (noOfmats > 3) { material4 = model.GetComponent<Renderer>().materials[3]; }
    }

    // sets the outline on and off, here you can change outline size.
    public void SetMaterialOutline(bool smBool)
    {
        if (smBool)
        {

            if (noOfmats > 0) {material1.SetFloat("_Outline", 0.0015f);}
            if (noOfmats > 1) {material2.SetFloat("_Outline", 0.0015f);}
            if (noOfmats > 2) {material3.SetFloat("_Outline", 0.0015f);}
            if (noOfmats > 3) {material4.SetFloat("_Outline", 0.0015f);}
        }
        else
        {
            if (noOfmats > 0) {material1.SetFloat("_Outline", 0.00f);}
            if (noOfmats > 1) {material2.SetFloat("_Outline", 0.00f);}
            if (noOfmats > 2) {material3.SetFloat("_Outline", 0.00f);}
            if (noOfmats > 3) {material4.SetFloat("_Outline", 0.00f);}

        }

    }

    // here materials colors and shaders are managed for Rare and Champion monsters.
    public void SetMaterialsColors(string colorToSet)
    {
        Shader shader1 = Shader.Find("aRPG/OutlinedRimBumpSpec");
        Shader shader2 = Shader.Find("aRPG/OutlinedBumpSpec");
        if (colorToSet == "blue")
        {
            if (noOfmats > 0)
            {
                material1.shader = shader1;
                material1.SetColor("_RimColor", Color.cyan);
            }
            if (noOfmats > 1)
            {

                material2.shader = shader1;
                material2.SetColor("_RimColor", Color.cyan);
            }
            if (noOfmats > 2)
            {

                material3.shader = shader1;
                material3.SetColor("_RimColor", Color.cyan);
            }
            if (noOfmats > 3)
            {

                material4.shader = shader1;
                material4.SetColor("_RimColor", Color.cyan);
            }
        }
        if (colorToSet == "rare")
        {
            if (noOfmats > 0)
            {
                material1.shader = shader1;
                material1.SetColor("_RimColor", Color.yellow);
            }
            if (noOfmats > 1)
            {

                material2.shader = shader1;
                material2.SetColor("_RimColor", Color.yellow);
            }
            if (noOfmats > 2)
            {

                material3.shader = shader1;
                material3.SetColor("_RimColor", Color.yellow);
            }
            if (noOfmats > 3)
            {

                material4.shader = shader1;
                material4.SetColor("_RimColor", Color.yellow);
            }
        }
    }
}
