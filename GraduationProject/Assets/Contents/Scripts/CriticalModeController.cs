using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CriticalModeController : MonoBehaviour {


    // Singleton
    private static CriticalModeController instance = null;
    public static CriticalModeController Instance
    {
        get
        {
            return instance;
        }
    }

    public enum CriticalModeState
    {
        Idle = 0,
        onEnter = 1,
        onExit = 2
    }


   
    public CriticalModeState currentState;

    public float criticalModeDuration;

    public UnityEvent onEnterState;
    public UnityEvent onExitState;
    public UnityEvent onReset;

    private void Awake()
    {

        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }


    public void doOnEnter()
    {
        if (onEnterState == null) return;
        if (currentState != CriticalModeState.Idle) return;

        currentState = CriticalModeState.onEnter;
        onEnterState.Invoke();
    }

    public void doOnExit()
    {
        if (onExitState == null) return;

        currentState = CriticalModeState.onExit;
        timeEplased = 0;
        onExitState.Invoke();
        
    }

    private float timeEplased;

    void FixedUpdate()
    {


        if (GameGlobals.Instance.isInGamePlay() == true)
        {
            if (currentState == CriticalModeState.onEnter)
            {
                timeEplased += Time.deltaTime;

                if (timeEplased >= criticalModeDuration)
                {
                    doOnExit();
                }
            }
        }

    }

    public void HideVillains()
    {
        if (currentState == CriticalModeState.onExit)
        {
            currentState = CriticalModeState.Idle;
            if (onReset != null)
            {
                onReset.Invoke();
            }
        }   
    }

    public void Reset()
    {

       timeEplased = 0;
       currentState = CriticalModeState.Idle;
  
    }




}
