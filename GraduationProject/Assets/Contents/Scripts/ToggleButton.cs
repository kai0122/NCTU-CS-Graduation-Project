using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour {

    public TweenPosition toggleTween;

    public string togglePref;
    public bool toggleState;

    public UnityEvent onToggleOn;
    public UnityEvent onToggleOff;


    void Start()
    {
        if (togglePref != "")
        {
            if (PlayerPrefs.GetInt(togglePref, 0) == 1)
            {
                doToggleOn();
            }
        }

    }

    public void toggleSwitch()
    {
        if (toggleState == false)
        {
            doToggleOn();
        }
        else
        {
            doToggleOff();
        }
    }

    public void doToggleOn()
    {
        toggleState = true;
        toggleTween.PlayForward();

        if (togglePref != "")
        {
            PlayerPrefs.SetInt(togglePref, 1);
        }

        if (onToggleOn != null)
        {
            onToggleOn.Invoke();
        }

    }

    public void doToggleOff()
    {
        toggleState = false;
        toggleTween.PlayReverse();

        if (togglePref != "")
        {
            PlayerPrefs.SetInt(togglePref, 0);
        }
        if (onToggleOff != null)
        {
            onToggleOff.Invoke();
        }
    }

}
