using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 游戏数据管理类
/// </summary>
public partial class ItemDataManager : Singleton<ItemDataManager>
{
    private int sellIdLately = 0;
    //private Common.ITEM_ACTION itemActionLately = Common.ITEM_ACTION.ITEM_ACTION_LIST;
    private int itemIdLately = 0;
    private int buyAddUseItemId = 0;
    private int buyAddUseItemNum = 0;
    private System.Action mBuySuccessCallBack;

    //是否是时间加速道具
    private bool m_IsTimeSpeedUpItem = false;
    //private TimeQueueManager.Type m_TimeQueueType;  //加速类型
    //private long m_TimeQueueUid;                    //加速队列的ID
    //end

    //private Dictionary<int, ShopData> m_SpecialShopData;

    //public Common.ITEM_ACTION ItemActionLately
    //{
    //    get
    //    {
    //        return itemActionLately;
    //    }
    //}

    #region 道具相关
    //private void ItemDataDestroy()
    //{
    //    // 注销网络事件
    //    MessagePublisher.Instance.Unsubscribe<Server.ItemReply>(OnItemReply);
    //    MessagePublisher.Instance.Unsubscribe<Server.FinishOrSpeedUpReply>(OnTimeSpeedUpItemReply);
    //    callbackByItemChangeRefresh = null;
    //    bagItemDict = null;
    //}
    //// 背包二级子标签类型
    //public enum BagItemType
    //{
    //    None = 0,    //不显示
    //    Special,     // 特殊
    //    Recource,    // 资源
    //    Speed,       // 加速
    //    Battle,      //战斗
    //    Box          //宝箱
    //}
    //// 英雄背包子标签类型
    //public enum HeroBagType
    //{
    //    None = 0,           //不显示
    //    Expendable,         // 1.消耗品
    //    Piece,              // 2.碎片
    //    CompositionBook,    // 3.合成书
    //    Medal,              //4.勋章
    //    Spoils              //5.战力品
    //}
    //背包类型
    public enum EBagType
    {
        CommonBag = 0,
        Equiped = 1,
    }

    // Event
    public UnityAction callbackByItemChangeRefresh = null;

    //各个背包数据key为背包ID
    private Dictionary<int, Dictionary<long, ItemVO>> bagItemDict = new Dictionary<int, Dictionary<long, ItemVO>>();
    private Dictionary<int, ItemVO> tempItemDic;
    //private void InitItemList(List<Common.Item> itemList)
    //{
    //    Debug.LogWarning("服务器初始化道具列表，itemList.Count==" + itemList.Count);
    //    ClearBag();
    //    for (int i = 0; i < itemList.Count; i++)
    //    {
    //        if (itemList[i].itemId > 0)
    //        {
    //            ItemVO temp = new ItemVO(itemList[i].itemId, itemList[i].itemAmount);
    //            AddItem(temp);
    //        }
    //    }
    //    DoRefreshEvent();
    //}
    public ItemDataManager()
    {
        bagItemDict.Add((int)EBagType.CommonBag, new Dictionary<long, ItemVO>());//以Uid做key
        bagItemDict.Add((int)EBagType.Equiped, new Dictionary<long, ItemVO>());//以装备的position做key
    }
    public void AddItem(ItemVO item, EBagType bagType = EBagType.CommonBag)
    {
        if (item.Count <= 0)
        {
            return;
        }
        if (bagItemDict.ContainsKey((int)bagType))
        {
            if (bagItemDict[(int)bagType].ContainsKey(item.UId))
            {
                bagItemDict[(int)bagType][item.UId] = item;
            }
            else
            {
                bagItemDict[(int)bagType].Add(item.UId, item);
            }
        }
        else
        {
            bagItemDict.Add((int)bagType, new Dictionary<long, ItemVO>());
            bagItemDict[(int)bagType].Add(item.UId, item);
        }
        //Slg.RedPointManager.Instance.MarkConditionChange(Slg.RedPointCondition.BagItem);

        //DoRefreshEvent();
    }
    //移除
    public void RemoveItem(ItemVO item, EBagType bagType = EBagType.CommonBag)
    {
        if (bagItemDict.ContainsKey((int)bagType))
        {
            if (bagItemDict[(int)bagType].ContainsKey(item.UId))
            {
                bagItemDict[(int)bagType].Remove(item.UId);
                //Slg.RedPointManager.Instance.MarkConditionChange(Slg.RedPointCondition.BagItem);

            }
            else
            {
                Debug.Log("删除不存在的道具");
            }
        }
        else
        {
            Debug.Log("删除不存在的包裹中的道具");
        }

        //DoRefreshEvent();
    }
    //更新道具信息
    public void UpdateItem(ItemVO item, EBagType bagType = EBagType.CommonBag)
    {
        if (bagItemDict.ContainsKey((int)bagType))
        {
            if (bagItemDict[(int)bagType].ContainsKey(item.UId))
            {
                if (item.Count > 0)
                {
                    bagItemDict[(int)bagType][item.UId] = item;
                }
                else
                {
                    //数量为0的移除
                    bagItemDict[(int)bagType].Remove(item.UId);
                }
                //Slg.RedPointManager.Instance.MarkConditionChange(Slg.RedPointCondition.BagItem);
            }
        }


    }
    public void ClearBag(EBagType bagType = EBagType.CommonBag)
    {
        if (bagItemDict.ContainsKey((int)bagType))
        {
            bagItemDict[(int)bagType].Clear();
            //Slg.RedPointManager.Instance.MarkConditionChange(Slg.RedPointCondition.BagItem);
        }
    }
    /// <summary>
    /// 通过物品ID在shop表中查找数据
    /// 只会在存在于Type=1的数据中
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    //public ShopData FindShopDataByItemId(int itemId)
    //{
    //    if (m_SpecialShopData == null)
    //    {
    //        m_SpecialShopData = new Dictionary<int, ShopData>();

    //        foreach (var oneShopItem in DataManager.Instance.ShopDatas.Value)
    //        {
    //            if (oneShopItem.Value.Type == 1)
    //            {
    //                m_SpecialShopData.Add(oneShopItem.Key, oneShopItem.Value);
    //            }
    //        }
    //    }

    //    foreach (var oneShopItem in m_SpecialShopData)
    //    {
    //        if (oneShopItem.Value.ItemId == itemId)
    //        {
    //            return oneShopItem.Value;
    //        }
    //    }
    //    Debug.LogError("FindShopDataByItemId Erro itemId=" + itemId + " 在shop表类型为1中没有找到数据！");
    //    return null;

    //}

    /// <summary>
    /// 通过背包标签获得物品
    /// </summary>
    /// <param name="uiType"></param>
    /// <returns></returns>
    //public List<ItemVO> GetBagItemListByUiBackType(int uiType)
    //{
    //    List<ItemVO> list = new List<ItemVO>();
    //    if (bagItemDict.ContainsKey((int)BagType.CommonBag))
    //    {
    //        var iter = bagItemDict[(int)BagType.CommonBag].GetEnumerator();
    //        while (iter.MoveNext())
    //        {
    //            if (iter.Current.Value.ItemData.UiBackType == uiType)
    //            {
    //                list.Add(iter.Current.Value);
    //            }
    //        }
    //        iter.Dispose();
    //    }
    //    return list;
    //}
    /// <summary>
    /// 通过英雄背包标签获得物品
    /// </summary>
    /// <param name="uiType"></param>
    /// <returns></returns>
    //public List<ItemVO> GetBagItemListByHeroBackType(int uiType)
    //{
    //    List<ItemVO> list = new List<ItemVO>();
    //    if (bagItemDict.ContainsKey((int)BagType.CommonBag))
    //    {
    //        var iter = bagItemDict[(int)BagType.CommonBag].GetEnumerator();
    //        while (iter.MoveNext())
    //        {
    //            if (iter.Current.Value.ItemData.HeroBackType == uiType)
    //            {
    //                list.Add(iter.Current.Value);
    //            }
    //        }
    //        iter.Dispose();
    //    }
    //    return list;
    //}
    /// <summary>
    /// 获得背包所有物品列表
    /// </summary>
    /// <param name="uiType"></param>
    /// <returns></returns>
    public List<ItemVO> GetAllBagItem()
    {
        List<ItemVO> list = new List<ItemVO>();
        if (bagItemDict.ContainsKey((int)EBagType.CommonBag))
        {
            var iter = bagItemDict[(int)EBagType.CommonBag].GetEnumerator();
            while (iter.MoveNext())
            {
                list.Add(iter.Current.Value);
            }
            iter.Dispose();

        }
        return list;
    }
    /// <summary>
    /// 获取背包中的物品信息
    /// </summary>
    /// <param name="id">物品id</param>
    /// <returns></returns>
    public ItemVO GetItemFormBag(int id, EBagType bagType = EBagType.CommonBag)
    {
        if (bagItemDict.ContainsKey((int)bagType))
        {
            if (bagItemDict[(int)bagType].ContainsKey(id))
            {
                return bagItemDict[(int)bagType][id];
            }
        }
        return null;
    }

    private void DoRefreshEvent()
    {
        if (callbackByItemChangeRefresh != null)
        {
            callbackByItemChangeRefresh();
        }
    }
    /// <summary>
    /// 获得道具系统对某个属性的加成值（buff道具，使用后的加成效果）
    /// 可能是数值，可能万分比，取决于属性id
    /// </summary>
    public float GetAttrAdditionByItem(int attributeId)
    {
        float number = 0;

        return number;
    }

    #region 装备
    private Attribute allEquipmentsAttribute = new Attribute();
    public Attribute AllEquipmentsAttribute
    {
        get
        {
            foreach (var item in bagItemDict[(int)EBagType.Equiped])
            {
                if (item.Value != null)
                {
                    allEquipmentsAttribute = GlobalExpansion.GetSumOfAttributes(allEquipmentsAttribute, item.Value.Equipment);
                }
            }
            return allEquipmentsAttribute;
        }
    }

    /// <summary>
    /// 换或者穿
    /// </summary>
    /// <param name="equipment"></param>
    /// <param name="pos"></param>
    public void ChangeEquipment(EquipmentVO equipment, int pos = 0)
    {
        if (equipment == null)
            return;
        if (pos == 0)
            pos = equipment.Position;

        bagItemDict[(int)EBagType.Equiped][pos] = new ItemVO(equipment.UId, equipment.ConfigId, 1, equipment);
        EventCenter.Broadcast(EGameEvent.eEquipmentChange);
    }
    /// <summary>
    /// 卸下装备
    /// </summary>
    /// <param name="pos"></param>
    public void PutOffEquipment(int pos)
    {
        if (bagItemDict[(int)EBagType.Equiped].ContainsKey(pos))
        {
            bagItemDict[(int)EBagType.Equiped].Remove(pos);
        }
        EventCenter.Broadcast(EGameEvent.eEquipmentChange);
    }
    public ItemVO GetEquipedItemByPos(int pos)
    {
        if (bagItemDict[(int)EBagType.Equiped].ContainsKey(pos))
        {
            return bagItemDict[(int)EBagType.Equiped][pos];
        }
        return null;
    }
    #endregion
    #region 道具和服务器交互
    /// <summary>
    /// 服务器返回
    /// 拉取，使用，出售等操作的返回
    /// </summary>
    //private void OnItemReply(Server.ItemReply reply)
    //{
    //    //        //道具返回值
    //    //enum ITEM_RESULT
    //    //    {
    //    //        ITEM_RESULT_SUCCESS = 1; //成功
    //    //        ITEM_RESULT_FAIL = 2; //失败，没有更多信息
    //    //        ITEM_RESULT_WRONG_DATA = 3; //失败，配置表信息错误
    //    //        ITEM_RESULT_CANNOT_USE = 4; //失败，无法使用
    //    //        ITEM_RESULT_CANNOT_BATCH = 5; //失败，不能批量使用
    //    //        ITEM_RESULT_NO_NUMBER = 6; //失败，使用数量为零
    //    //        ITEM_RESULT_LASS_NUMBER = 7; //失败，道具数量不足
    //    //        ITEM_RESULT_SQL_ERROR = 8; //失败，数据库操作失败
    //    //    }
    //    Debug.Log(string.Format("================|reply.result={0}|=================", reply.result));

    //    if (reply.result == Common.ITEM_RESULT.ITEM_RESULT_SUCCESS)
    //    {
    //        if (reply.itemAction == Common.ITEM_ACTION.ITEM_ACTION_LIST)
    //        {
    //            Debug.Log(string.Format("================|背包物品数量={0}|=================", reply.item.Count));
    //            ClearBag();
    //            for (int i = 0; i < reply.item.Count; i++)
    //            {
    //                if (reply.item[i].itemId > 0)
    //                {
    //                    ItemVO temp = new ItemVO(reply.item[i].itemId, reply.item[i].itemAmount);
    //                    AddItem(temp);
    //                }
    //            }
    //        }
    //        else if (reply.itemAction == Common.ITEM_ACTION.ITEM_ACTION_USE)
    //        {
    //            Slg.SoundManager.Instance.PlaySound(AK.EVENTS.MSOUND_ITEMUSE);

    //            for (int i = 0; i < reply.item.Count; i++)
    //            {
    //                ItemVO temp = new ItemVO(reply.item[i].itemId, reply.item[i].itemAmount);
    //                UpdateItem(temp);
    //                //UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", string.Format("[color=#FF8E00FF]{0}" + LocalizationManager.Instance.GetString("KEY.500.1001") + "[/color]", LocalizationManager.Instance.GetString(temp.ItemData.Name)));
    //            }
    //            //检测奖励数据中是否有书签更新信息，如果有，更新个人书签最大容量
    //            for(var rewardIndex = 0; rewardIndex < reply.reward.Count; ++rewardIndex) {
    //                if(reply.reward[rewardIndex].UId == 1012) {
    //                    MapDataManager.Instance.SetMaxBookmarkCount_Normal((int)reply.reward[rewardIndex].amount);
    //                    break;
    //                }
    //            }
    //        }
    //        else if (reply.itemAction == Common.ITEM_ACTION.ITEM_ACTION_SELL)
    //        {
    //            Slg.SoundManager.Instance.PlaySound(AK.EVENTS.MSOUND_ITEMSELL);

    //            for (int i = 0; i < reply.item.Count; i++)
    //            {
    //                ItemVO temp = new ItemVO(reply.item[i].itemId, reply.item[i].itemAmount);
    //                UpdateItem(temp);
    //                Utils.GlobalEventDispatcher.Instance.dispatchEvent(Utils.GlobalEventNameDefine.ItemSellSuccess, temp);
    //                UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", string.Format(LocalizationManager.Instance.GetString("KEY.500.1009"), temp.Name));
    //            }

    //        }
    //        //购买并使用，购买成功后，操作
    //        if (buyAddUseItemId != 0)
    //        {

    //            if (mBuySuccessCallBack == null)
    //            {
    //                if (m_IsTimeSpeedUpItem)
    //                {
    //                    UseTimeSpeedUpItem(m_TimeQueueType, m_TimeQueueUid, 0, buyAddUseItemId, buyAddUseItemNum);
    //                    m_IsTimeSpeedUpItem = false;
    //                }
    //                else
    //                {
    //                    UseItem(buyAddUseItemId, buyAddUseItemNum);
    //                }
    //            }
    //            else
    //            {
    //                mBuySuccessCallBack();
    //            }
    //            buyAddUseItemId = 0;
    //            buyAddUseItemNum = 0;
    //            mBuySuccessCallBack = null;
    //            return;
    //        }
    //        DoRefreshEvent();
    //        //下边显示，操作反馈弹窗
    //        if (ItemActionLately != Common.ITEM_ACTION.ITEM_ACTION_LIST)
    //        {
    //            if (ItemActionLately == Common.ITEM_ACTION.ITEM_ACTION_USE)
    //            {
    //                GameData.ItemData itemData = DataManager.Instance.ItemDatas.Value[itemIdLately];
    //                if (itemData != null && itemData.ItemEvent == (int)EnumItemEvent.GetVIPExp)
    //                {
    //                    UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", string.Format( LocalizationManager.Instance.GetString("KEY.500.1074"), LocalizationManager.Instance.GetString(itemData.Name)));
    //                }
    //                else
    //                {
    //                    UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", LocalizationManager.Instance.GetString("KEY.500.1001"));
    //                }
    //                itemIdLately = 0;
    //                itemActionLately = Common.ITEM_ACTION.ITEM_ACTION_LIST;

    //                //QuickUse界面调用关闭函数
    //                if (Utils.GlobalEventDispatcher.Instance.GetdispatchEvent(Utils.GlobalEventNameDefine.QuickUsePanelCloseFunc)) {
    //                    Utils.GlobalEventDispatcher.Instance.dispatchEvent(Utils.GlobalEventNameDefine.QuickUsePanelCloseFunc, null);
    //                }
    //            }
    //            else
    //            {
    //                GameData.ItemData itemdata = GameDataManager.Instance.GetItemDataById(sellIdLately);
    //                if (itemdata != null)
    //                {
    //                    ItemVO itemvo = new ItemVO(sellIdLately, 1);
    //                    UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", string.Format(LocalizationManager.Instance.GetString("KEY.500.1009"), itemvo.Name));
    //                    Utils.GlobalEventDispatcher.Instance.dispatchEvent(Utils.GlobalEventNameDefine.ItemSellSuccess, itemvo);
    //                }
    //                sellIdLately = 0;
    //                itemActionLately = Common.ITEM_ACTION.ITEM_ACTION_LIST;
    //            }

    //        }
    //    }
    //    else
    //    {
    //        DoRefreshEvent();
    //        GameDataManager.Instance.ShowServerReturnErrorPromptMessage(reply.result.ToString(), UIPopMessage.Mode.Tips);
    //    }
    //}

    /// <summary>
    /// 服务器返回
    /// 时间道具使用返回
    /// </summary>
    //private void OnTimeSpeedUpItemReply(Server.FinishOrSpeedUpReply reply)
    //{
    //    Debug.Log(string.Format("================|FinishOrSpeedUp reply.result={0}|=================", reply.result));

    //    if (reply.result == Server.SPEEDUP_RESULT.SPEEDUP_RESULT_SUCCESS)
    //    {
    //        Server.TimeProgress timeProgress = reply.timeProgress;

    //        if (timeProgress != null)
    //        {

    //        }
    //        TimeQueueManager.TimeQueue timeQueue = new TimeQueueManager.TimeQueue();
    //        timeQueue.type = (TimeQueueManager.Type)timeProgress.progressType;
    //        timeQueue.uuid = timeProgress.uid;
    //        timeQueue.TotalTime = timeProgress.period;
    //        timeQueue.time = timeProgress.endTime;
    //        timeQueue.help = timeProgress.help;
    //        TimeQueueManager.TimeQueue currentQueue = TimeQueueManager.Instance.GetTimeQueue(timeQueue.type, timeQueue.uuid);
    //        if (currentQueue != null)
    //            timeQueue.tips = currentQueue.tips;
    //        TimeQueueManager.Instance.AndOrReplaceLocalData(timeQueue.type, timeQueue.uuid, timeQueue);
    //        if (timeProgress.endTime - EngineUtil.GetServerTime() > 1.0f)   //todo 使用立即完成或免费，不提示道具使用成功提示
    //            UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", string.Format("<color=#FF8E00FF>{0}~</color>",
    //            LocalizationManager.Instance.GetString("KEY.500.1001")));
    //    }
    //    else
    //    {
    //        GameDataManager.Instance.ShowServerReturnErrorPromptMessage(reply.result.ToString(), UIPopMessage.Mode.Ok);
    //    }
    //}

    /// <summary>
    /// 请求服务器数据
    /// </summary>
    //public void RequestBagInfo()
    //{
    //    Client.ClientMessage msg = new Client.ClientMessage();
    //    msg.item = new Client.Item();
    //    msg.item.itemAction = Common.ITEM_ACTION.ITEM_ACTION_LIST;
    //    NetworkManager.Instance.SendProtobufMessage(msg);
    //    itemActionLately = Common.ITEM_ACTION.ITEM_ACTION_LIST;
    //}

    /// <summary>
    /// 使用道具,特殊道具的使用，都走这里
    /// </summary>
    /// <param name="useId"></param>
    /// <param name="useCount"></param>
    /// <param name="pIsDirectUse">是否直接使用，如果是购买并使用设false，直接使用设true</param>
    public void UseItem(int useId, int useCount = 1, bool pIsDirectUse = false)
    {
        //if (!ChekcoutIsCanUseByDefenseItem(useId))
        //    return;
        //if (useCount <= 0)
        //{
        //    UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", "<color=#FF8E00FF>" + LocalizationManager.Instance.GetString("KEY.500.1282") + "</color>");
        //}
        //ItemData data = GetItemDataById(useId);

        //#region 道具使用，需要特殊处理的
        //if (data.ItemEvent == 2) {      //体力道具使用，需要特殊处理
        //    int curBuyCount = GameDataManager.Instance.Vip.staminaTimes;
        //    int totalBuyCount = GameDataManager.Instance.GetStaminaLimit();
        //    if (totalBuyCount != 0 && curBuyCount >= totalBuyCount) {
        //        string buyHint = "0/" + totalBuyCount;
        //        UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.OkCancel, LocalizationManager.Instance.GetString("KEY.702.1014"),
        //            LocalizationManager.Instance.GetString("KEY.703.2427").Replace("{0}", buyHint), () => {
        //                UIManager.Instance.Show(UIManager.UIType.UIVIP, true, null, null);
        //            }, null, 1.5f, LocalizationManager.Instance.GetString("KEY.700.1091"));
        //        return;
        //    }
        //} else if(data.ItemEvent == 3) {            //Vip等级存在上限，当Vip经验满时，使用会弹出特殊处理飘字
        //    if(Vip.vipLevel >= MaxVIPLevel) {
        //        UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", LocalizationManager.Instance.GetString("KEY.500.1124"));
        //        return;
        //    }
        //} else if (data.ItemEvent == 1000 && pIsDirectUse) {        //Buff道具，如果可替换，弹出提示框 在商店背包中使用才检测
        //    BuffData previousBuffData = null;
        //    if(ChekcoutIsExistOverrideBuff(data.Number, ref previousBuffData)) {
        //        var curBuffData = DataManager.Instance.BuffDatas.Value[data.Number];
        //        string contentHint = LocalizationManager.Instance.GetString("KEY.703.1995").
        //            Replace("{0}", LocalizationManager.Instance.GetString(curBuffData.Name)).
        //            Replace("{1}", LocalizationManager.Instance.GetString(previousBuffData.M_BuffData.Name));

        //        if(previousBuffData.LeftTime > 0) {
        //            UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.OkCancel, LocalizationManager.Instance.GetString("KEY.702.1059"), contentHint,
        //            () => {
        //                useItem(data, useId, useCount);
        //            });
        //        }
                
        //        return;
        //    }
        //} else if (data.ItemEvent == 1012) {        //个人书签容量存在上限，当个人书签容量达到上限时，使用会弹出特殊处理飘字
        //    int maxBookMarkCount = Const.Instance.GetIntConfigConst("MaxBookmarks");
        //    if(MapDataManager.Instance.MaxBookmarkCount_Normal >= maxBookMarkCount) {
        //        UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", LocalizationManager.Instance.GetString("KEY.500.1156"));
        //        return;
        //    }
        //} else if (data.ItemEvent == 1015) {        //装备扩充格存在上限，当达到上限时，使用会弹出特殊处理飘字
        //    int maxCellCount = Const.Instance.GetIntConfigConst("MaximumQuantity");
        //    if (this.WorkShopReply.workShop.cellCount >= maxCellCount) {
        //        UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", LocalizationManager.Instance.GetString("KEY.500.1293"));
        //    } else {
        //        Client.WorkShop shop = new Client.WorkShop();
        //        shop.action = Common.WORKSHOP_ACTION.WORKSHOP_ACTION_UNLOCK_EXPANDER;
        //        GameDataManager.Instance.RequestEquipOperation(shop);
        //    }
        //    return;
        //}
        //#endregion

        //useItem(data, useId, useCount);
    }
    //真实使用道具
    private void useItem(ItemData data, int useId, int useCount = 1) {
        ////打开某个ui的道具
        //if (data.OpenUI > 0) {
        //    UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", "<color=#FF8E00FF>"+ LocalizationManager.Instance.GetString("KEY.500.1283") +"</color>");
        //    //UIManager.Instance.ShowOnly( (UIManager.UIType)data.OpenUI);
        //    return;
        //}
        ////发送使用协议
        //RequestToOperateItem(useId, useCount, Common.ITEM_ACTION.ITEM_ACTION_USE);
    }

    /// <summary>
    /// 出售道具
    /// </summary>
    /// <param name="sellId"></param>
    /// <param name="sellCount"></param>
    public void SellItem(int sellId, int sellCount = 1)
    {
        //if (sellCount <= 0)
        //{
        //    UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", "<color=#FF8E00FF>"+ LocalizationManager.Instance.GetString("KEY.500.1281") +"</color>");
        //    return;
        //}
        //RequestToOperateItem(sellId, sellCount, Common.ITEM_ACTION.ITEM_ACTION_SELL);
        //sellIdLately = sellId;
    }
    //private void RequestToOperateItem(int id, int count, Common.ITEM_ACTION itemAction)
    //{
        //请求数据
        //Client.ClientMessage msg = new Client.ClientMessage();
        //msg.item = new Client.Item();
        //msg.item.itemAction = itemAction;
        //msg.item.item = new Common.Item();
        //msg.item.item.itemId = id;
        //msg.item.item.itemAmount = count;
        //NetworkManager.Instance.SendProtobufMessage(msg);
        //itemIdLately = id;
        //itemActionLately = itemAction;
    //}
    /// <summary>
    /// 购买道具,统一处理货币不够情况
    /// </summary>
    /// <param name="shopType">商店类型</param>
    /// <param name="shopTab">商店页签</param>
    /// <param name="shopId">商品Id</param>
    /// <param name="count">数量</param>
    /// <param name="buyType">购买类型</param>
    /// <param name="buySuccessCallBack">购买类型为BuyAddUse时，购买成功后的回调，为null时，默认使用（一般道具和时间道具）</param>
    //public void BuyItem(int shopType, int shopTab, int shopId, int count = 1, EBuyType buyType = EBuyType.Buy, System.Action buySuccessCallBack = null)
    //{
    //    if (count <= 0)
    //    {
    //        UIManager.Instance.ShowPopMessage(UIPopMessage.Mode.Tips, "", "<color=#FF8E00FF>"+LocalizationManager.Instance.GetString("KEY.500.1280") +"</color>");
    //    }
    //    GameData.ShopData shopData = DataManager.Instance.ShopDatas.Value[shopId];

    //    long ownMoney = GameDataManager.Instance.GetCoinCount((GameDataManager.CoinType)shopData.CurrencyId);
    //    //if (ownMoney >= shopData.CurrencyNumber)
    //    //{
    //    Client.ClientMessage msg = new Client.ClientMessage();
    //    msg.shop = new Client.Shop();
    //    msg.shop.action = Common.SHOP_ACTION.SHOP_BUY;
    //    msg.shop.shopType = shopType;
    //    msg.shop.shopTab = shopTab;
    //    msg.shop.shopId = shopId;
    //    msg.shop.num = count;
    //    NetworkManager.Instance.SendProtobufMessage(msg);
    //    if (buyType == EBuyType.BugAddUse)
    //    {
    //        buyAddUseItemId = shopData.ItemId;
    //        buyAddUseItemNum = shopData.ItemNumber;
    //        mBuySuccessCallBack = buySuccessCallBack;
    //    }
    //    //}
    //    //else
    //    //{
    //    //    UIManager.Instance.ShowPopMessage( UIPopMessage.Mode.Ok, LocalizationManager.Instance.GetString("KEY.702.1014"), LocalizationManager.Instance.GetString("KEY.703.2070"),
    //    //        null, null, 2, LocalizationManager.Instance.GetString("KEY.700.1091"));
    //    //}
    //}
    #endregion



    public static string GetItemColorByQuality(int quality)
    {
        //1：白色
        //2：绿色
        //3：蓝色
        //4：紫色
        //5：橙色
        //6：红色
        if (quality == 1)
            return "<color=#ffffffff>{0}</color>";
        else if (quality == 2)
            return "<color=#008000ff>{0}</color>";
        else if (quality == 3)
            return "<color=#0000ffff>{0}</color>";
        else if (quality == 4)
            return "<color=#800080ff>{0}</color>";
        else if (quality == 5)
            return "<color=#ffa500ff>{0}</color>";
        else if (quality == 6)
            return "<color=#ff0000ff>{0}</color>";
        else
        {
            Debug.LogError("Error : quality = " + quality.ToString() + " is not case in GetItemColorByQuality()!");
            return "<color=#ffffffff>{0}</color>";
        }
    }

    #endregion

    /// <summary>
    /// 获取道具品质框图片
    /// </summary>
    /// <param name="pQuality"></param>
    /// <returns></returns>
    //public FairyGUI.NTexture GetItemQualityIcon(int pQuality)
    //{
        //string iconPath = "border_prop1";
        //switch (pQuality) {
        //    case 0:
        //    case 1:
        //        iconPath = "border_prop1";
        //        break;
        //    case 2:
        //        iconPath = "border_prop2";
        //        break;
        //    case 3:
        //        iconPath = "border_prop3";
        //        break;
        //    case 4:
        //        iconPath = "border_prop4";
        //        break;
        //    case 5:
        //        iconPath = "border_prop5";
        //        break;
        //    case 6:
        //        iconPath = "border_prop6";
        //        break;
        //    default:
        //        Debug.LogError("not this Item Quality icon, quality ==" + pQuality);
        //        break;
        //}
        //return ResourcesLoaderEx.GetNTextureItem(Const.ItemTypeIconPrefix + iconPath);
    //}

}
