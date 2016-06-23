using UnityEngine;
using System;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour
    where T : MonoBehaviour, new()
{
    protected static T classInstance;

    public static T Instance {
        get {
            if (classInstance == null) {
                classInstance = FindObjectOfType(typeof(T)) as T;
                if (classInstance == null) {
                    classInstance = new T();
                    Debug.Log(classInstance);
                }
            }
            return classInstance;
        }
    }

    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (this == Instance) {
            return true;
        }
        DestroyImmediate(this);
        return false;
    }

    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}
