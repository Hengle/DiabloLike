using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具值对象
/// </summary>
public class ItemVO
{
    private long uid = 0;
    private int id = 0;
    private int count = 0;

    private ItemData itemData;
    private Sprite itemIcon;
    public EquipmentVO Equipment;

    public ItemData ItemData
    {
        get
        {
            if (itemData == null)
            {
                itemData = DataManager.Instance.GetItem(id);
            }
            return itemData;
        }
    }
    public int Count
    {
        get
        {
            return count; 
        }
    }
    public int Id
    {
        get
        {
            return id;
        }
    }
    public long UId
    {
        get
        {
            return uid;
        }
    }

    /// <summary>
    /// Item构造基础信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public ItemVO(long uid, int id, int count = 1, EquipmentVO equip = null)
    {
        this.uid = uid;
        this.id = id;
        this.count = count;
        this.itemData = null;
        this.itemIcon = null;
        Equipment = equip;
    }
    
    /// <summary>
    /// 带品质颜色的名字
    /// </summary>
    //public string ColorName
    //{
    //    get {
    //        return string.Format(GameDataManager.GetItemColorByQuality(this.ItemData.Quality), this.Name );
    //    }
    //}
    /// <summary>
    /// 多语言转换后的Name
    /// </summary>
    public string Name
    {
        get
        {
            return this.ItemData.Name;
        }
    }
    /// <summary>
    /// 多语言转换后的Desc
    /// </summary>
    //public string Desc
    //{
    //    get
    //    {
    //        return this.ItemData.Desc;
    //    }
    //}
    //public bool Useable
    //{
    //    get
    //    {
    //        return this.ItemData.Type == 7 || this.ItemData.Type == 9;//分类详见item.xlsx
    //    }
    //}

    //public bool IsSpeedUpItem
    //{
    //    get
    //    {
    //        if (1001 <= this.ItemData.ItemEvent && this.ItemData.ItemEvent <= 1008)
    //            return true;//分类详见item.xlsx
    //        return false;
    //    }
    //}
    //是否可以批量用
    //public bool BatchUseable
    //{
    //    get
    //    {
    //        return this.ItemData.BatchUsage == 1 && Count > 1;//分类详见item.xlsx
    //    }
    //}
    /// <summary>
    /// 是否能查看，针对宝箱
    /// </summary>
    //public bool CheckAble
    //{
    //    get
    //    {
    //        //box 1可以 2不可以
    //        return this.ItemData.Box == 1;
    //    }
    //}
    /// <summary>
    /// Ugui获得道具图片，已经废弃，请使用NTextureIcon
    /// </summary>
    //public Sprite ItemIcon
    //{
    //    get
    //    {
    //        if (itemIcon == null)
    //        {
    //            if (ItemData != null)
    //            {
    //                Sprite sprite = ResourcesLoaderEx.GetSpriteItem(Const.ItemIconPrefix + this.ItemData.ItemIcon);
    //                if (sprite == null)
    //                {
    //                    return ResourcesLoaderEx.GetSpriteItem(Const.ItemIconPrefix + "Equip1001");
    //                }
    //                return sprite;
    //            }
    //            else
    //            {
    //                Debug.LogErrorFormat("ItemData is null,id==={0}", id);
    //            }
    //        }
    //        return itemIcon;
    //    }
    //}
    //public FairyGUI.NTexture NTextureIcon
    //{
    //    get
    //    {
    //        return ResourcesLoaderEx.GetNTextureItem(ItemData.ItemIcon);
    //    }
    //}


    //public static FairyGUI.NTexture ItemNullQualityIcon
    //{
    //    get
    //    {
    //        return GameDataManager.Instance.GetItemQualityIcon(0);
    //    }
    //}
    //public FairyGUI.NTexture QualityIcon
    //{
    //    get
    //    {
    //        if(ItemData != null) {
    //            return GameDataManager.Instance.GetItemQualityIcon(ItemData.Quality);
    //        }

    //        string iconPath = "border_prop1";
    //        //if (itemData != null) {
    //        //    switch (itemData.Quality) {
    //        //        case 0:
    //        //        case 1:
    //        //            iconPath = "border_hero_pinzhi_bai";
    //        //            break;
    //        //        case 2:
    //        //            iconPath = "border_hero_pinzhi_lv";
    //        //            break;
    //        //        case 3:
    //        //            iconPath = "border_hero_pinzhi_lan";
    //        //            break;
    //        //        case 4:
    //        //            iconPath = "border_hero_pinzhi_zi";
    //        //            break;
    //        //        case 5:
    //        //            iconPath = "border_hero_pinzhi_huang";
    //        //            break;
    //        //        default:
    //        //            Debug.LogError("not this Quality icon, quality ==" + itemData.Quality);
    //        //            iconPath = "border_hero_pinzhi_bai";
    //        //            break;
    //        //    }
    //        //}
    //        return ResourcesLoaderEx.GetNTextureItem(Const.ItemTypeIconPrefix + iconPath); ;
    //    }
    //}
    //public bool CanSell
    //{
    //    get
    //    {
    //        return ItemData.SellCurrency != "0";
    //    }
    //}
    //public int SellMoneyType
    //{
    //    get
    //    {
    //        if (CanSell)
    //        {
    //            string[] arr = ItemData.SellCurrency.Split('|');
    //            if (arr.Length > 0)
    //            {
    //                return int.Parse(arr[0]);
    //            }
    //        }
    //        return 0;
    //    }

    //}
    //public int SellMoneyNumber
    //{
    //    get
    //    {
    //        if (CanSell)
    //        {
    //            string[] arr = ItemData.SellCurrency.Split('|');
    //            if (arr.Length > 1)
    //            {
    //                return int.Parse(arr[1]);
    //            }
    //        }
    //        return 0;
    //    }
    //}
}

/// <summary>
/// 按照ID对ItemVO进行递增排序
/// </summary>
public class ItemVOSortComparerByIDIncrement : IComparer<ItemVO>
{
    public int Compare(ItemVO pItem1, ItemVO pItem2) {
        if (pItem1 != null && pItem2 != null) {
            if (pItem1.Id < pItem2.Id) {
                return -1;
            } else if (pItem1.Id > pItem2.Id) {
                return 1;
            }
        }

        return 0;
    }
}
