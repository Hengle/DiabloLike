using System;
using System.Collections.Generic;
using UnityEngine;

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
            GameObject obj = GameObject.Instantiate(DropPrefab);
            obj.transform.position = new Vector3(UnityEngine.Random.value + pos.x, pos.y, UnityEngine.Random.value + pos.z);
            Debug.Log("掉落道具");
        } 
    }
    public void PickUpItem(Transform clickTrans)
    {
        if(clickTrans != null)
        {
            ItemDrops drop = clickTrans.GetComponent<ItemDrops>();
            if(drop != null)
            {
                //判断包有没有满
                Debug.Log("拾取道具，加入背包");
                drop.gameObject.SetActive(false);
                GameObject.Destroy(drop.gameObject);
            }
        }
    }
}

