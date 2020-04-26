using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 相机初始深度
/// </summary>
public enum ECameraDefDepth
{
    MainCamera = -1,
    UguiCamera,
    FguiCamera,
}
/// <summary>
/// Fgui package管理，相机管理
/// </summary>
public class FGuiManager : MonoSingleton<FGuiManager>
{
    private string m_CurrentOpenName = null;
    private bool isAddedPackage = false;

    public FGuiManager()
    {

    }

    public void Initialize()
    {
        //UIConfig.defaultFont = "league_spartan";
        //初始化字体

      
        if (isAddedPackage)
            return;

        //        GRoot.inst.SetContentScaleFactor(750, 1334, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
        FairyGUI.UIConfig.defaultFont = "JianZhunYuan";
#if UNITY_EDITOR
        //if (!ResourcesLoader.LoadOriginalAssets)
        //    AddPackageFromAb();
        //else
        //{

            AddPackageFromLocal();

          
            Font myFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/JianZhunYuan.ttf");
            FontManager.RegisterFont(new DynamicFont("JianZhunYuan", myFont), "JianZhunYuan");
            FontManager.GetFont("League Spartan").customBold = true;
        //}

#else
        AddPackageFromAb( );
        
        ResourcesLoader.Load<Font>("league_spartan",(myFont)=> {
            FontManager.RegisterFont(new DynamicFont("League Spartan", myFont), "League Spartan");

            FontManager.GetFont("League Spartan").customBold = true;
        });
#endif
        isAddedPackage = true;
    }
    
    private void BindAllScript()
    {
    }

    public void SetStageCameraDepth(bool isLowerThanUICamera)
    {
        int defaultDepth = -1;

        //if(UIMainCamera.Self != null && UIMainCamera.Self.M_Camera != null)
        //{
        //    UIMainCamera.Self.M_Camera.depth= isLowerThanUICamera ? StageCamera.main.depth + 1 : StageCamera.main.depth - 1;
        //    //StageCamera.main.depth = isLowerThanUICamera? UIMainCamera.Self.M_Camera.depth - 1 : UIMainCamera.Self.M_Camera.depth + 1;
        //}
        //else
        //{
            StageCamera.main.depth = (int)ECameraDefDepth.FguiCamera;
        //}
    }



    private void AddPackageFromAb()
    {
        List<string> resList = new List<string>();
        //resList.Add("fariyui/spriteresource");

        int totalCount = resList.Count;
        //foreach (var res in resList)
        //{
        //    ResourcesLoader.Instance.LoadBundle(res, ab =>
        //    {
        //        if (ab != null)
        //        {
        //            if (ab.assetBundle != null)
        //            {
        //                FairyGUI.UIPackage.AddPackage(ab.assetBundle);
        //                totalCount--;

        //                if (totalCount <= 0)
        //                {
        //                    AfterAddPackage();
        //                }
        //            }
        //        }
        //    });
        //}
    }

    private void AddPackageFromLocal()
    {
        FairyGUI.UIPackage.AddPackage("Assets/Resources/FairyGUI/Common/Common");
        FairyGUI.UIPackage.AddPackage("Assets/Resources/FairyGUI/SpriteRes/SpriteRes");                 //图标
        FairyGUI.UIPackage.AddPackage("Assets/Resources/FairyGUI/Bag/Bag");                             //背包
        FairyGUI.UIPackage.AddPackage("Assets/Resources/FairyGUI/ItemDrop/ItemDrop");                   //掉落
        FairyGUI.UIPackage.AddPackage("Assets/Resources/FairyGUI/Main/Main");                           //主界面


        AfterAddPackage();
    }

    private void AfterAddPackage()
    {

        Common.CommonBinder.BindAll();
        //SpriteRes.SpriteResBinder.BindAll();这个只有图片，是不用BindAll的；
        Bag.BagBinder.BindAll();
        ItemDrop.ItemDropBinder.BindAll();
        Main.MainBinder.BindAll();

        GRoot.inst.SetContentScaleFactor(GlobalExpansion.designScreenWidth, GlobalExpansion.designScreenHeight);//设置屏幕缩放比

        //create camera
        StageCamera.CheckMainCamera();
        DontDestroyOnLoad(StageCamera.main.gameObject);
        SetStageCameraDepth(true);
    }

    public void SetFairyTouchable(bool toggle)
    {
        var se = GameObject.Find("Stage").GetComponent<StageEngine>();
        se.enabled = toggle;
    }
    /// <summary>
    /// 判断是否点中FGUI
    /// </summary>
    /// <returns></returns>
    public static bool IsHitFgui()
    {
        return FairyGUI.Stage.isTouchOnUI;
    }

    //public void TogglePanelTouchable(bool enOrDis)
    //{
    //    foreach (Transform tran in transform)
    //    {
    //        tran.gameObject.SetActive(enOrDis);
    //    }

    //}

    //public void ClosePanel(string name)
    //{
    //    //var panel = transform.Find(pkgName + "_" + name);
    //    var panel = transform.Find(name);
    //    if (panel != null)
    //        panel.gameObject.SetActive(false);
    //    UIBase fUIBase = panel.GetComponent<UIBase>();
    //    fUIBase.Fwindow.Hide();
    //    //检测是否是Fgui
    //   CheckOutFgui();
    //    this.m_CurrentOpenName = null;
    //}
    ///// <summary>
    ///// 隐藏所有Fui
    ///// </summary>
    //public void CloseAll()
    //{
    //    foreach(Transform panel in transform)
    //    {
    //        if (panel != null)
    //            panel.gameObject.SetActive(false);
    //        UIBase fUIBase = panel.GetComponent<UIBase>();
    //        fUIBase.Fwindow.Hide();
    //    }
    //    //检测是否是Fgui
    //    CheckOutFgui();
    //}

    //public GameObject OpenPanel(string name)
    //{
    //    var go = new GameObject(name);
    //    go.transform.SetParent(transform);
    //    go.transform.SetAsLastSibling();
    //    go.transform.localScale = Vector3.one;
    //    go.transform.localPosition = Vector3.zero;
    //    go.AddComponent<Canvas>();
    //    go.AddComponent<UnityEngine.UI.GraphicRaycaster>();
    //    go.AddComponent<UIElement>();
    //    System.Type t = System.Reflection.Assembly.Load("Assembly-CSharp").GetType(name);
    //    UIBase uibase = go.AddComponent(t) as UIBase;
      
    //    var gochild = new GameObject();
    //    gochild.transform.SetParent(go.transform);
    //    uibase.SetUIBaseCommonBtn(gochild);

    //    uibase.Bg_DontTouch = AddFguiBg(go.transform);

    //    this.m_CurrentOpenName = name;
    //    return go;

    //}

    ////
    //public void OpenUIMain()
    //{
    //    var go = new GameObject("UIMain");
    //    go.AddComponent<UIMain>();
    //    go.transform.SetParent(transform);
    //    var UIMainAlways = new GameObject("UIMainAlways");
    //    UIMainAlways.transform.SetParent(transform);
    //    UIMainAlways.AddComponent<UIMainAlways>();

    //}

    ////家在PVE战斗界面
    //public void OpenBattleUIPVEMain()
    //{
    //    var UIPVEMain = new GameObject("BattleUIPVEMain");
    //    UIPVEMain.transform.SetParent(transform);
    //    UIPVEMain.AddComponent<BattleUIPVEMain>();
    //    //UIPVEMain.AddComponent<TouchController>();
    //    //UIPVEMain.AddComponent<JoystickController>();
    //}

    ////加载PVP战斗界面
    //public void OpenBattleUIPVPMain()
    //{
    //    var UIPVPMain = new GameObject("BattleUIPVEMain");
    //    UIPVPMain.transform.SetParent(transform);
    //    UIPVPMain.AddComponent<UIBattlePvpPanelMain>();
    //}
    ///// <summary>
    ///// 加一个不透的底板，防止fgui到ugui的穿透
    ///// </summary>
    ///// <param name="parent"></param>
    ///// <returns></returns>
    //public GameObject AddFguiBg(Transform parent)
    //{
    //    var bg_dnotTouch = new GameObject();
    //    bg_dnotTouch.transform.SetParent(parent);
    //    bg_dnotTouch.transform.localScale = Vector3.one;
    //    bg_dnotTouch.transform.localPosition = Vector3.zero;
    //    UnityEngine.UI.Image bgImg = bg_dnotTouch.AddComponent<UnityEngine.UI.Image>();
    //    bgImg.raycastTarget = true;
    //    bgImg.color = new Color(0, 0, 0, 100/(float)255);
    //    bgImg.rectTransform.sizeDelta = new Vector2(2000, 2000);
    //    return bg_dnotTouch;
    //}

    
    //public void closeCurrentPanel() {
    //    if(this.m_CurrentOpenName != null) {
    //        this.ClosePanel(this.m_CurrentOpenName);
    //    }
    //}

    /// <summary>
    /// 检查是否还有FGUI在队列里，如果是，根据操作，显示隐藏主界面
    /// </summary>
    //public void CheckOutFgui()
    //{
    //    bool isActive = false;
    //    foreach (Transform child in transform)
    //    {
    //        if (child != null)
    //        {
    //            if (child.gameObject.activeInHierarchy)
    //            {
    //                isActive = true;
    //                break;
    //            }
    //        }
    //    }

    //    //临时解决 多界面问题 等FGUI换完，需要注掉。。。。。add by  dong
    //    //UIManager.Instance.CloseOneUI();


    //}
}
