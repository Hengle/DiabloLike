using System;
using System.Collections.Generic;
using UnityEngine;
using ItemDrop;

public class ItemDropsManager : MonoSingleton<ItemDropsManager>
{
    private GameObject dropPrefab;
    public GameObject DropPrefab
    {
        get
        {
            if(dropPrefab == null)
            {
                dropPrefab = Resources.Load("Prefab/ItemDrops/ItemDrops") as GameObject;
            }
            return dropPrefab;
        }
    }
    public List<DropItem> ItemList = new List<DropItem>();
    UIItemDrop uiItemDrop = null;
    private int uidMaker = 0;

    GameObject m;
    aRPG_Master ms;
    private void Start()
    {
        m = GameObject.Find("SCRIPTS");
        ms = m.GetComponent<aRPG_Master>();
        
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            DoItemDrops(ms.player.transform.position, 0);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            uiItemDrop.Hide();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            UIManager.Instance.ShowWind(EUIType.UIBag);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            uiItemDrop.Show();
        }
        //名字显示到屏幕
        if (ItemList.Count > 0)
        {
            uiItemDrop.Update();
        }
    }
    /// <summary>
    /// 传入掉落点，和掉落id
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dropId"></param>
    public void DoItemDrops(Vector3 pos, int dropId) {
        //检查是否掉落，掉落什么
        //掉落物品
        
        if(DropPrefab != null)
        {
            DropItem drop = new DropItem();
            drop.itemvo = CreateDropItem();
   


            GameObject obj = GameObject.Instantiate(DropPrefab);
            obj.transform.position = new Vector3(UnityEngine.Random.value + pos.x, pos.y, UnityEngine.Random.value + pos.z);
            Debug.Log("掉落道具");
            drop.OutLookTrans = obj.transform;


            if (uiItemDrop == null)
            {
                uiItemDrop = UIManager.Instance.CreateWindow( EUIType.UIItemDrop) as UIItemDrop;
                uiItemDrop.Show();
                uiItemDrop.AfterOnShown();
            }
            else
            {
                uiItemDrop.Show();
                uiItemDrop.AfterOnShown();
            }
            drop.UIName = uiItemDrop.CreateItemName(drop);
            ItemList.Add(drop);
        } 
    }
    public void PickUpItem(DropItem item)
    {
        if(item != null)
        {
            //判断包有没有满
            Debug.Log("拾取道具，加入背包");
           
            ItemDataManager.Instance.AddItem(item.itemvo);

            item.OutLookTrans.gameObject.SetActive(false);
            GameObject.Destroy(item.OutLookTrans.gameObject);
            item.UIName.Dispose();
            ItemList.Remove(item);
        }  
    }
    public ItemVO CreateDropItem()
    {
        ++uidMaker;
        int id = UnityEngine.Random.Range(100001, 100015);
        //如果是装备
        if (id > 100000)
        {
            return new ItemVO(uidMaker, id, 1, new EquipmentVO(uidMaker,id));
        }
        return new ItemVO(uidMaker, id);
    }

}
/// <summary>
/// 掉落物品
/// </summary>
public class DropItem
{
    public Transform OutLookTrans;
    public ItemVO itemvo;

    public UI_ItemName UIName;
}

