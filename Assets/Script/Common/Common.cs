using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace ClashGame
{
    public class Common
    {
        private static uint m_siInstanceID = 0;
        public static uint GetInstanceID()
        {
            return ++m_siInstanceID;
        }
        public static Vector3 SetV3_x(Vector3 v3,float x)
        {
            return new Vector3(x,v3.y,v3.z);
        }

        public static Vector3 SetV3_y(Vector3 v3, float y)
        {
            return new Vector3(v3.x, y, v3.z);
        }

        public static Vector3 SetV3_z(Vector3 v3, float z)
        {
            return new Vector3(v3.x, v3.y, z);
        }

        public static Vector3 AddV3_x(Vector3 v3,float x)
        {
            return SetV3_x(v3, v3.x + x);
        }

        public static Vector3 AddV3_y(Vector3 v3, float y)
        {
            return SetV3_y(v3, v3.y + y);
        }
 
        public static string GetAssetBundlePath(string resname)
        {
            string path = resname;
            if (resname.Contains("/ep-")) //场景物件
            {
                char[] separator = new char[] { '/' };
                string[] paths = path.Split(separator);
                if (paths.Length < 3)
                {
                    Debug.LogError("path Error!!! path=" + path);
                    return "";
                }
                int strlength = resname.Length - paths[paths.Length - 1].Length - paths[paths.Length - 2].Length - 2;
                path = resname.Substring(0, strlength);
            }
            else if (resname.Contains("/mp-"))//怪物
            {
                char[] separator = new char[] { '/' };
                string[] paths = path.Split(separator);
                if (paths.Length < 2)
                {
                    Debug.LogError("path Error!!! path=" + path);
                    return "";
                }
                int strlength = resname.Length - paths[paths.Length - 1].Length - 1;
                path = resname.Substring(0, strlength);
            }
            else if (resname.Contains("/sp-"))//ui
            {
                char[] separator = new char[] { '/' };
                string[] paths = path.Split(separator);
                if (paths.Length < 2)
                {
                    Debug.LogError("path Error!!! path=" + path);
                    return "";
                }
                int strlength = resname.Length - paths[paths.Length - 1].Length - 1;
                path = resname.Substring(0, strlength);
            }
            
            return path;
        }

        public static string md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        public static string HashFile(string file)
        {
            try
            {
                string[] strfile = File.ReadAllLines(file + ".manifest"); //取manifest中的hash值。sprite packer mode=always enabled 每次重生成后。MD5码会变
                string hash = strfile[5];
                hash = hash.Substring(10);
                Debug.Log(hash);
                return hash;
                  //byte[] stream = null;
                  //AssetBundle bundle = null;
                  //stream = File.ReadAllBytes(file);
                  //Debug.Log("1");
                  //if (stream != null)
                  //{
                  //    Debug.Log("2");
                  //    bundle = AssetBundle.CreateFromMemoryImmediate(stream); //关联数据的素材绑定
                  //    if (bundle != null)
                  //    {
                  //        Debug.Log("3");
                  //        AssetBundleManifest mainfest = (AssetBundleManifest)bundle.LoadAsset("AssetBundleManifest");
                  //        Debug.Log("4");

                  //        string[] ss = bundle.GetAllAssetNames();
                  //        for (int i = 0; i < ss.Length; i++)
                  //        {
                  //            Debug.Log(ss[i]);
                  //        }

                  //        string[] str = mainfest.GetAllAssetBundles();
                  //        Debug.Log(str[0]);
                  //        return mainfest.GetAssetBundleHash(str[0]).ToString();
                  //    }
                  //}
            }
            catch (Exception ex)
            {
                throw new Exception("HashFile() fail, error:" + ex.Message);
            }
            Debug.LogError("HashFile() fail, error:");
            return "error";
        }

        public static string strKey = "menghuan";
        public static string strIV = "amiaoyou";
        // 文件解密加载
        public static void XmlLoadDecrypt(XmlDocument xmlDoc, string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            byte[] bsXml = new byte[fileStream.Length];
            fileStream.Read(bsXml, 0, bsXml.Length);
            fileStream.Close();

            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateDecryptor(Encoding.UTF8.GetBytes(strKey), Encoding.UTF8.GetBytes(strIV)), CryptoStreamMode.Write);
            encStream.Write(bsXml, 0, bsXml.Length);
            encStream.FlushFinalBlock();

            xmlDoc.Load(new MemoryStream(ms.ToArray()));
        }

        // 文件加密存储
        public static void XmlSaveEncrypt(XmlDocument xmlDoc, string fileName)
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            FileStream fileStream = new FileStream(fileName, FileMode.Truncate);
            MemoryStream msXml = new MemoryStream();
            xmlDoc.Save(msXml);

            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream cs = new CryptoStream(fileStream, tdes.CreateEncryptor(Encoding.UTF8.GetBytes(strKey), Encoding.UTF8.GetBytes(strIV)), CryptoStreamMode.Write);
            cs.Write(msXml.ToArray(), 0, msXml.ToArray().Length);
            cs.FlushFinalBlock();

            msXml.Close();
            fileStream.Close();
        }

        public static string strKey2 = "amiaoyou";
        public static string strIV2 = "menghuan";
        // 字符串加密
        public static string Encrypt(string _strQ)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(_strQ);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(Encoding.UTF8.GetBytes(strKey2), Encoding.UTF8.GetBytes(strIV2)), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray()).Replace("+", "%");
        }

        // 字符串解密
        public static string Decrypt(string _strQ)
        {
            _strQ = _strQ.Replace("%", "+");
            byte[] buffer = Convert.FromBase64String(_strQ);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateDecryptor(Encoding.UTF8.GetBytes(strKey2), Encoding.UTF8.GetBytes(strIV2)), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
        {
            // Project A and B onto the plane orthogonal target axis
            dirA = dirA - Vector3.Project(dirA, axis);
            dirB = dirB - Vector3.Project(dirB, axis);

            // Find (positive) angle between A and B
            float angle = Vector3.Angle(dirA, dirB);

            // Return angle multiplied with 1 or -1
            return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
        }

        public static bool BeArrived(Vector3 curpos,Vector3 startpos,Vector3 endpos)
        {
            float dis = Vector3.Distance(curpos, endpos);
            if (dis<0.1f)
            {
                return true;
            }
            Vector3 v1=startpos-endpos;
            Vector3 v2=curpos-endpos;
            float dot = Vector3.Dot(v1, v2);
            if (dot<0)
            {
                return true;
            }
            return false;
        }

    }

}
