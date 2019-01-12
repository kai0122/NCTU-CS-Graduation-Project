using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public OnEnterDelegate OnEnter;
    public OnExitDelegate OnExit;

    public void OnTriggerEnter(Collider collider)
    {

        if (this.OnEnter != null)
        {
            this.OnEnter(this.gameObject,collider);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (this.OnExit != null)
        {
            this.OnExit(this.gameObject,collider);
        }
    }

    public delegate void OnEnterDelegate(GameObject self, Collider collider);

    public delegate void OnExitDelegate(GameObject self, Collider collider);
}

