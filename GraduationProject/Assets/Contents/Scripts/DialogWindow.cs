using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour
{


    // Singleton
    private static DialogWindow instance = null;
    public static DialogWindow Instance
    {
        get
        {
            return instance;
        }
    }

    public delegate void DialogCallback(DialogResult result);

    public enum DialogType
    {
        OkOnly = 0,
        YesNo = 1
    }

    public enum DialogResult
    {
        Yes = 0,
        No = 1,
        Ok = 2
    }


    public GameObject dialogPanel;
    public GameObject okButton, yesButton, noButton;
    public Text messageText;
    public TweenScale scaleTween;
    private DialogCallback callback;

    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;


        dialogPanel.SetActive(false);

    }

    private void Start()
    {



    }

  
 
    public void showDialog(DialogType dialogType, string message, DialogCallback callback)
    {

        GameGlobals.Instance.audioController.playSound("UIQuestion", false);

        dialogPanel.SetActive(true);
        this.callback = callback;

        if (dialogType == DialogType.YesNo)
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
            okButton.SetActive(false);
        }
        else
        {
            yesButton.SetActive(false);
            noButton.SetActive(false);
            okButton.SetActive(true);
        }

        messageText.text = message;
        //scaleTween.ResetToBeginning();
        scaleTween.PlayForward();



    }

    public void closeDialog(string buttonID)
    {
        GameGlobals.Instance.audioController.playSound("UITap", false);

        scaleTween.PlayReverse();
        Invoke("hideDialogPanel", 0.3f);

        if (buttonID == null) return;

        if (buttonID == "yes")
        {
            if (callback != null)
            {
                callback(DialogResult.Yes);
            }
          
        }
        else if (buttonID == "no")
        {
            if (callback != null)
            {
                callback(DialogResult.No);
            }
              
        }
        else if (buttonID == "ok")
        {
            if (callback != null)
            {
                callback(DialogResult.Ok);
            }
        }

    }

    private void hideDialogPanel()
    {
        dialogPanel.SetActive(false);
    }


    

}
