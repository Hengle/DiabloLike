using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using BigMap;
using Slg.Ecs.UI;
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
public class FGuiManager : BehaviourSingleton<FGuiManager>
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
        if (!ResourcesLoader.LoadOriginalAssets)
            AddPackageFromAb();
        else
        {

            AddPackageFromLocal();

          
            Font myFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/JianZhunYuan.ttf");
            FontManager.RegisterFont(new DynamicFont("JianZhunYuan", myFont), "JianZhunYuan");
            FontManager.GetFont("League Spartan").customBold = true;
        }

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

        if(UIMainCamera.Self != null && UIMainCamera.Self.M_Camera != null)
        {
            UIMainCamera.Self.M_Camera.depth= isLowerThanUICamera ? StageCamera.main.depth + 1 : StageCamera.main.depth - 1;
            //StageCamera.main.depth = isLowerThanUICamera? UIMainCamera.Self.M_Camera.depth - 1 : UIMainCamera.Self.M_Camera.depth + 1;
        }
        else
        {
            StageCamera.main.depth = (int)ECameraDefDepth.FguiCamera;
        }
    }



    private void AddPackageFromAb()
    {
        List<string> resList = new List<string>();
        resList.Add("fariyui/spriteresource");
        resList.Add("fariyui/fuimain");
        resList.Add("fariyui/common");
        resList.Add("fariyui/prison");
        resList.Add("fariyui/lord");
        resList.Add("fariyui/forge");
        resList.Add("fariyui/vip");
        resList.Add("fariyui/story");
        resList.Add("fariyui/bag");
        resList.Add("fariyui/popup");
        resList.Add("fariyui/herobag");
        resList.Add("fariyui/hiddencave");
        resList.Add("fariyui/lord");
        resList.Add("fariyui/cellar");
        resList.Add("fariyui/satellite");
        resList.Add("fariyui/commonbuildingupgrade");
        resList.Add("fariyui/bigmap");
        resList.Add("fariyui/armyinfo");
        resList.Add("fariyui/warhall");
        resList.Add("fariyui/countryfairpack");
        resList.Add("fariyui/embassy");
        resList.Add("fariyui/metallurgyvenue");
        resList.Add("fariyui/metallurgyexplain");
        resList.Add("fariyui/darkelements");
        resList.Add("fariyui/deletedarkelement");

        resList.Add("fariyui/mailui");
        resList.Add("fariyui/mailuiwritemail");
        resList.Add("fariyui/mailuiinformation");
        resList.Add("fariyui/mailuibattlereportdetail");
        resList.Add("fariyui/arena");
        resList.Add("fariyui/arenacommonpanel");
        resList.Add("fariyui/rankinglist");
        resList.Add("fariyui/arenarecurrence");
        resList.Add("fariyui/arenapresentation");
        resList.Add("fariyui/hospital");

        resList.Add("fariyui/showdescribe");
        resList.Add("fariyui/commondescription");
        resList.Add("fariyui/task");
        resList.Add("fariyui/onlinerewards");
        resList.Add("fariyui/upgradeReward");
        resList.Add("fariyui/legioneditor");
        resList.Add("fariyui/selecthero");

        resList.Add("fariyui/commonbuildingleveleffect");
        resList.Add("fariyui/commonbuildingstatistics");
        resList.Add("fariyui/commonbuildingupgrade");
        resList.Add("fariyui/castle");
        resList.Add("fariyui/boxcheck");
        resList.Add("fariyui/buildingresources");
        //resList.Add("fariyui/loading");
        resList.Add("fariyui/college");
        resList.Add("fariyui/buildingwall");
        resList.Add("fariyui/duplicate");
        resList.Add("fariyui/barrack");
        resList.Add("fariyui/alliance");
        resList.Add("fariyui/battle");
        resList.Add("fariyui/herocultivate");
        resList.Add("fariyui/marchinfopanel");
        resList.Add("fariyui/mapbookmark");
        resList.Add("fariyui/sounds");
		resList.Add("fariyui/cityintensify");
		resList.Add("fariyui/chatview");
        resList.Add("fariyui/setting");
        resList.Add("fariyui/qiguan");
        resList.Add("fariyui/buildingtest");

        int totalCount = resList.Count;
        foreach (var res in resList)
        {
            ResourcesLoader.Instance.LoadBundle(res, ab =>
            {
                if (ab != null)
                {
                    if (ab.assetBundle != null)
                    {
                        FairyGUI.UIPackage.AddPackage(ab.assetBundle);
                        totalCount--;

                        if (totalCount <= 0)
                        {
                            AfterAddPackage();
                        }
                    }
                }
            });
        }
    }

    private void AddPackageFromLocal()
    {
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/SpriteResource/SpriteResource");                      //资源
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/FUIMain/FUIMain");                      //主界面通用
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Common/Common");                      //通用
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Prison/Prison");                      //监狱
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Lord/Lord");                          //领主
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Forge/Forge");                        //锻造厂
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Story/Story");                        //剧情
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/VIP/VIP");                        //锻造厂
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Bag/Bag");                            //背包
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/HiddenCave/HiddenCave");                            //藏兵洞
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Lord/Lord");                            //领主图集
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Cellar/Cellar");                            //地窖/军备仓库
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CommonBuildingUpgrade/CommonBuildingUpgrade");                            //通用建筑升级
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Popup/Popup");                      //Popup系列弹窗
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Satellite/Satellite");                      //瞭望塔、卫星
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/HeroBag/HeroBag");                      //英雄背包
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/WarHall/WarHall");                    //战争大厅
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CountryFairPack/CountryFairPack");    //集市
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Embassy/Embassy");                    //大使馆
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/BigMap/BigMap");
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/ArmyInfo/ArmyInfo");
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MetallurgyVenue/MetallurgyVenue");    //炼金所
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MetallurgyExplain/MetallurgyExplain");//炼金所说明界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/DarkElements/DarkElements");//炼金所说明界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/DeleteDarkElement/DeleteDarkElement");//炼金所说明界面

        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MailUI/MailUI");//邮件主界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MailUIWriteMail/MailUIWriteMail");//邮件写邮件界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MailUIInformation/MailUIInformation");//邮件写邮件界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MailUIBattleReportDetail/MailUIBattleReportDetail");//邮件写邮件界面

        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Arena/Arena");//竞技场界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/ArenaCommonPanel/ArenaCommonPanel");//竞技场排名奖励，重置界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/RankingList/RankingList");//竞技场排名奖励，重置界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/ArenaRecurrence/ArenaRecurrence");//竞技场战斗重现界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/ArenaPresentation/ArenaPresentation");//竞技场说明界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Hospital/Hospital");//维修厂

        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/ShowDescribe/ShowDescribe");//描述显示
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CommonDescription/CommonDescription");//描述显示
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Task/Task");//任务
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/OnLineRewards/OnLineRewards");//任务
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/UpgradeReward/UpgradeReward");//领主，指挥中心升级奖励界面
        
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/LegionEditor/LegionEditor");//编辑军团
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/SelectHero/SelectHero");//英雄选择

        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CommonBuildingLevelEffect/CommonBuildingLevelEffect");//建筑通用效果显示
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CommonBuildingStatistics/CommonBuildingStatistics");//统计信息显示
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CommonBuildingUpgrade/CommonBuildingUpgrade");//通用升级
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Castle/Castle");//统计信息显示
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/BoxCheck/BoxCheck");//宝箱道具详情界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/BuildingResources/BuildingResources");//宝箱道具详情界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/College/College");//学院
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/BuildingWall/BuildingWall");//城墙
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Duplicate/Duplicate");//副本
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Barrack/Barrack");//军营
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Alliance/Alliance");//联盟
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Battle/Battle");//PVE战斗
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/HeroCultivate/HeroCultivate");//英雄养成
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MarchInfoPanel/MarchInfoPanel");//行军队列界面
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/MapBookmark/MapBookmark");//大地图书签
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Sounds/Sounds");//UI音频
		FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/CityIntensify/CityIntensify");//基地强化效果
		FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/ChatView/ChatView");//聊天
        //FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Loading/Loading");//加载
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/Setting/Setting");//设置
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/QiGuan/QiGuan");//奇观UI
        FairyGUI.UIPackage.AddPackage("Assets/BuildOnlyAssets/FariyUI/BuildingTest/BuildingTest");//奇观UI

        AfterAddPackage();
    }

    private void AfterAddPackage()
    {

        FUIMain.FUIMainBinder.BindAll();
        Common.CommonBinder.BindAll();
        Prison.PrisonBinder.BindAll();
        Lord.LordBinder.BindAll();
        Forge.ForgeBinder.BindAll();
        Story.StoryBinder.BindAll();
        VIP.VIPBinder.BindAll();
        HeroBag.HeroBagBinder.BindAll();
        Popup.PopupBinder.BindAll();
        Bag.BagBinder.BindAll();
        HiddenCave.HiddenCaveBinder.BindAll();
        Lord.LordBinder.BindAll();
        Cellar.CellarBinder.BindAll();
        Satellite.SatelliteBinder.BindAll();
        CommonBuildingUpgrade.CommonBuildingUpgradeBinder.BindAll();
        WarHall.WarHallBinder.BindAll();
        CountryFairPack.CountryFairPackBinder.BindAll();
        BigMapBinder.BindAll();
        ArmyInfo.ArmyInfoBinder.BindAll();
        Embassy.EmbassyBinder.BindAll();
        MetallurgyVenue.MetallurgyVenueBinder.BindAll();
        MetallurgyExplain.MetallurgyExplainBinder.BindAll();
        DarkElements.DarkElementsBinder.BindAll();
        DeleteDarkElement.DeleteDarkElementBinder.BindAll();
        MailUI.MailUIBinder.BindAll();
        MailUIWriteMail.MailUIWriteMailBinder.BindAll();
        MailUIInformation.MailUIInformationBinder.BindAll();
        MailUIBattleReportDetail.MailUIBattleReportDetailBinder.BindAll();

        Arena.ArenaBinder.BindAll();
        ArenaCommonPanel.ArenaCommonPanelBinder.BindAll();
        RankingList.RankingListBinder.BindAll();
        ArenaRecurrence.ArenaRecurrenceBinder.BindAll();
        ArenaPresentation.ArenaPresentationBinder.BindAll();
        Hospital.HospitalBinder.BindAll();
        //Keypad.KeypadBinder.BindAll();
        ShowDescribe.ShowDescribeBinder.BindAll();
        CommonDescription.CommonDescriptionBinder.BindAll();
        Task.TaskBinder.BindAll();
        OnLineRewards.OnLineRewardsBinder.BindAll();
        UpgradeReward.UpgradeRewardBinder.BindAll();
        BoxCheck.BoxCheckBinder.BindAll();  
        LegionEditor.LegionEditorBinder.BindAll();
        SelectHero.SelectHeroBinder.BindAll();

        CommonBuildingLevelEffect.CommonBuildingLevelEffectBinder.BindAll();
        CommonBuildingStatistics.CommonBuildingStatisticsBinder.BindAll();
        CommonBuildingUpgrade.CommonBuildingUpgradeBinder.BindAll();
        Castle.CastleBinder.BindAll();
        BuildingResources.BuildingResourcesBinder.BindAll();
        //Loading.LoadingBinder.BindAll();
        //Application.targetFrameRate = 60;
        College.CollegeBinder.BindAll();
        BuildingWall.BuildingWallBinder.BindAll();
        Duplicate.DuplicateBinder.BindAll();
        Barrack.BarrackBinder.BindAll();
        Alliance.AllianceBinder.BindAll();
        Battle.BattleBinder.BindAll();
        HeroCultivate.HeroCultivateBinder.BindAll();   
        MarchInfoPanel.MarchInfoPanelBinder.BindAll();
        MapBookmark.MapBookmarkBinder.BindAll();
		CityIntensify.CityIntensifyBinder.BindAll();
        ChatView.ChatViewBinder.BindAll();
        Setting.SettingBinder.BindAll();
        QiGuan.QiGuanBinder.BindAll();
        BuildingTest.BuildingTestBinder.BindAll();
        GRoot.inst.SetContentScaleFactor(1334, 750);

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

    public GameObject OpenPanel(string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(transform);
        go.transform.SetAsLastSibling();
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<Canvas>();
        go.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        go.AddComponent<UIElement>();
        System.Type t = System.Reflection.Assembly.Load("Assembly-CSharp").GetType(name);
        UIBase uibase = go.AddComponent(t) as UIBase;
      
        var gochild = new GameObject();
        gochild.transform.SetParent(go.transform);
        uibase.SetUIBaseCommonBtn(gochild);

        uibase.Bg_DontTouch = AddFguiBg(go.transform);

        this.m_CurrentOpenName = name;
        return go;

    }

    //
    public void OpenUIMain()
    {
        var go = new GameObject("UIMain");
        go.AddComponent<UIMain>();
        go.transform.SetParent(transform);
        var UIMainAlways = new GameObject("UIMainAlways");
        UIMainAlways.transform.SetParent(transform);
        UIMainAlways.AddComponent<UIMainAlways>();

    }

    //家在PVE战斗界面
    public void OpenBattleUIPVEMain()
    {
        var UIPVEMain = new GameObject("BattleUIPVEMain");
        UIPVEMain.transform.SetParent(transform);
        UIPVEMain.AddComponent<BattleUIPVEMain>();
        //UIPVEMain.AddComponent<TouchController>();
        //UIPVEMain.AddComponent<JoystickController>();
    }

    //加载PVP战斗界面
    public void OpenBattleUIPVPMain()
    {
        var UIPVPMain = new GameObject("BattleUIPVEMain");
        UIPVPMain.transform.SetParent(transform);
        UIPVPMain.AddComponent<UIBattlePvpPanelMain>();
    }
    /// <summary>
    /// 加一个不透的底板，防止fgui到ugui的穿透
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject AddFguiBg(Transform parent)
    {
        var bg_dnotTouch = new GameObject();
        bg_dnotTouch.transform.SetParent(parent);
        bg_dnotTouch.transform.localScale = Vector3.one;
        bg_dnotTouch.transform.localPosition = Vector3.zero;
        UnityEngine.UI.Image bgImg = bg_dnotTouch.AddComponent<UnityEngine.UI.Image>();
        bgImg.raycastTarget = true;
        bgImg.color = new Color(0, 0, 0, 100/(float)255);
        bgImg.rectTransform.sizeDelta = new Vector2(2000, 2000);
        return bg_dnotTouch;
    }

    
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
