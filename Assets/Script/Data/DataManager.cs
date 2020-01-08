using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DataManager : Singleton<DataManager> {


    public Dictionary<string, ItemData> itemDataDic = new Dictionary<string, ItemData>();
    public DataManager(){
        Load();
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
}
public class ItemData
{
    public int Id;
    public string Name;
}
