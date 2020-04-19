using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DataManager : Singleton<DataManager> {


    public Dictionary<string, ItemData> itemDataDic = new Dictionary<string, ItemData>();
    public Dictionary<string, EquipmentData> equipmentDataDic = new Dictionary<string, EquipmentData>();
    public Dictionary<string, SkillData> skillDataDic = new Dictionary<string, SkillData>();
    public DataManager(){
        Load();
        LoadEquipment();
        LoadSkill();
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
    void LoadEquipment()
    {
        string filepath = Application.dataPath + "/Resources/JsonConfig/" + "Equipment.json";
        if (!File.Exists(filepath))
        {
            return;
        }
        StreamReader sr = new StreamReader(filepath, System.Text.Encoding.UTF8);
        string strLine = sr.ReadToEnd();
        equipmentDataDic = JsonMapper.ToObject<Dictionary<string, EquipmentData>>(strLine);
        sr.Dispose();
        foreach (var item in equipmentDataDic)
        {
            Debug.Log(item.Value.Id);
        }
    }
    void LoadSkill()
    {
        string filepath = Application.dataPath + "/Resources/JsonConfig/" + "Skill.json";
        if (!File.Exists(filepath))
        {
            return;
        }
        StreamReader sr = new StreamReader(filepath, System.Text.Encoding.UTF8);
        string strLine = sr.ReadToEnd();
        skillDataDic = JsonMapper.ToObject<Dictionary<string, SkillData>>(strLine);
        sr.Dispose();
        foreach (var item in skillDataDic)
        {
            Debug.Log(item.Value.Id);
        }
    }
    public ItemData GetItem(int id)
    {
        if (itemDataDic.ContainsKey(id.ToString()))
        {
            return itemDataDic[id.ToString()];
        }
        return null;
    }
    public EquipmentData GetEquipment(int id)
    {
        if (equipmentDataDic.ContainsKey(id.ToString()))
        {
            return equipmentDataDic[id.ToString()];
        }
        return null;
    }
    public SkillData GetSkill(int id)
    {
        if (skillDataDic.ContainsKey(id.ToString()))
        {
            return skillDataDic[id.ToString()];
        }
        return null;
    }
}
public class ItemData
{
    public int Id;
    public string Name;
}

