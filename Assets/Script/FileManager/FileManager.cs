using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

class RoleInfo {
    public readonly List<string> commonTalk = new List<string>();
   
    public readonly Dictionary<string, bool> channelVoiceSetting = new Dictionary<string, bool>();
}
class CacheUserInfo {
    public string UserName { get; set; }
    public bool AudioActive { get; set; }
    public bool MusicActive { get; set; }

    public readonly Dictionary<string, RoleInfo> roleInfoDict = new Dictionary<string, RoleInfo>();
    public CacheUserInfo() {
        AudioActive = true;
        MusicActive = true;
        //isShowFirstBattleCG = false;
    }
    public string ToJson() {
        string data = JsonMapper.ToJson(this);
        return data;
    }
    public static CacheUserInfo ToObject(string data) {
        CacheUserInfo obj = new CacheUserInfo();
        obj = JsonMapper.ToObject<CacheUserInfo>(data);
        return obj;
    }
}


class FileManager {
    private string m_fileName;
    private FileInfo m_fileInfo;
    public FileManager(string name) {
        //正常持久化的数据
        //m_fileName = Application.persistentDataPath + '/' + name;
        //编辑状态下assets目录下数据
        m_fileName = Application.dataPath + "/CubeMap/" + name;
        m_fileInfo = new FileInfo(m_fileName);
    }

    public string Read() {
        StreamReader streRead = null;
        if (m_fileInfo.Exists) {
            try {
                streRead = File.OpenText(m_fileName);
            }
            catch {
                return "";
            }
            string txt = "";
            txt = streRead.ReadToEnd();
            streRead.Close();
            streRead.Dispose();
            return txt;
        }
        return "";
    }

    public void Write(string txt) {
        StreamWriter streWrite;
        if (m_fileInfo.Exists) {
            streWrite = m_fileInfo.AppendText();
        }
        else {
            streWrite = m_fileInfo.CreateText();
        }
        streWrite.WriteLine(txt);
        streWrite.Close();
        streWrite.Dispose();
    }

    public void Delete() {
        if (m_fileInfo.Exists) {
            File.Delete(m_fileName);
        }
    }
}

class CacheManager:Singleton<CacheManager>{

    private CacheUserInfo m_userInfo;
   
    public const string CACHE_USER_INFO_FILE = "cacheInfo.txt";



    private void Init() {
        FileManager file = new FileManager(CACHE_USER_INFO_FILE);
        string userJson = file.Read();
        if (string.IsNullOrEmpty(userJson)) {
            m_userInfo = new CacheUserInfo();
        }
        else {
            m_userInfo = CacheUserInfo.ToObject(userJson);
        }
       
    }

    public void SetUserLoginInfo(CacheUserInfo info) {
        m_userInfo = info;
    }

    public void SaveCache() {
        FileManager file = new FileManager(CACHE_USER_INFO_FILE);
        file.Delete();
        file.Write(m_userInfo.ToJson());
    }

    public CacheUserInfo GetCacheInfo() {
        return m_userInfo;
    }


}


