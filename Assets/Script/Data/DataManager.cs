using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DataManager : Singleton<DataManager> {


    public Dictionary<string, ItemData> itemDataDic = new Dictionary<string, ItemData>();
    public Dictionary<string, zhiye> zhiyeDataDic = new Dictionary<string, zhiye>();
    public DataManager(){
        Load();
        LoadZhiye();
    }
    public void Init()
    {

    }
    void Load()
    {
        string filepath = Application.dataPath + "/Resources/JsonConfig/" + "Item.json";
        if (!File.Exists(filepath))
        {
            return;
        }
        StreamReader sr = new StreamReader(filepath, System.Text.Encoding.UTF8);
        string strLine = sr.ReadToEnd();
        itemDataDic = JsonMapper.ToObject<Dictionary<string, ItemData>>(strLine);
        sr.Dispose();
        foreach (var item in itemDataDic)
        {
            Debug.Log(item.Value.Name);
        }
    }
    void LoadZhiye()
    {
        string filepath = Application.dataPath + "/Resources/JsonConfig/" + "职业1.json";
        if (!File.Exists(filepath))
        {
            return;
        }
        StreamReader sr = new StreamReader(filepath, System.Text.Encoding.UTF8);
        string strLine = sr.ReadToEnd();
        zhiyeDataDic = JsonMapper.ToObject<Dictionary<string, zhiye>>(strLine);
        sr.Dispose();
        foreach (var item in zhiyeDataDic)
        {
            Debug.Log(item.Value.hp);
        }
    }
}
public class ItemData
{
    public int Id;
    public string Name;
}
public class zhiye
{
    public int id;
    public int lv;
    public double hp;
    //public List<string> atk;
    public List<int> fa;
    public bool jue;
    public string pu;
    public List<string> p2u;
}
