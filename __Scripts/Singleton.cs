using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance) return _instance;
            return _instance = FindObjectOfType<T>();
        }
    }
}
