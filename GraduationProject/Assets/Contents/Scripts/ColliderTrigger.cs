using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public OnEnterDelegate OnEnter;
    public OnExitDelegate OnExit;

    public void OnTriggerEnter(Collider collider)
    {
      
        if (this.OnEnter != null)
        {
            this.OnEnter(collider);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (this.OnExit != null)
        {
            this.OnExit(collider);
        }
    }

    public delegate void OnEnterDelegate(Collider collider);

    public delegate void OnExitDelegate(Collider collider);
}

