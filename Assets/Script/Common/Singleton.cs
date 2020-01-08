using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : new() {

    private static T instance;
    public static T Instance {
        get {
            if(instance == null) {
                instance = new T();
            }
            return instance;
        }
    }

    public static void ReleaseInstance() {
        instance = default(T);
    }
}
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;
    public static T Instance {
        get {
            if(instance == null) {
                instance = GameObject.FindObjectOfType<T>();
                if(instance == null) {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

}