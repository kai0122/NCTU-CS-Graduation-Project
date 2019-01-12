using UnityEngine;
using System;
using System.Collections;

public class StaticCoroutine : MonoBehaviour
{

    private static StaticCoroutine mInstance = null;

    private static StaticCoroutine instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(StaticCoroutine)) as StaticCoroutine;

                if (mInstance == null)
                {
                    mInstance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
                }
            }
            return mInstance;
        }
    }

    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as StaticCoroutine;
        }
    }

    IEnumerator Perform(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
        Die();
    }


    public static Coroutine DoCoroutine(IEnumerator coroutine)
    {
        return instance.StartCoroutine(instance.Perform(coroutine)); 
    }

    public static void EndCoroutine(Coroutine coroutine)
    {
        try
        {
            instance.StopCoroutine(coroutine);
        }
        catch (Exception)
        {

        }
       
    }

    void Die()
    {
        mInstance = null;
        Destroy(gameObject);
    }

    void OnApplicationQuit()
    {
        mInstance = null;
    }
}