using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdvertController : MonoBehaviour {


    public bool disableAdverts;

    public bool showAdsOnMainMenu;
    public int  adsOnMainMenuInterval;
    private int adsOnMainMenuIntervalCounter;

    public bool showAdsOnGameOver;
    public int adsOnGameOverInterval;
    private int adsOnGameOverIntervalCounter;

    public GameObject advertGuiContainer;
    public bool useAdvertGuiContainer;

    public UnityEvent onAdvertRequested;
    public UnityEvent onAdvertShowed;

    // Use this for initialization
    void Awake ()
    {
        if (advertGuiContainer != null)
        {
            advertGuiContainer.SetActive(false);
        }

        adsOnMainMenuIntervalCounter = 1;
        adsOnGameOverIntervalCounter = 1;
    }



    public void requestAdvert()
    {

        if (disableAdverts == true) return;

        if (onAdvertRequested != null)
        {
            onAdvertRequested.Invoke();
        }
        //////////////////////////////////////////
        // CALL YOUR ADVERT REQUEST METHOD HERE
        // .......
        //////////////////////////////////////////

    }


    public void showAdvertOnMainMenu()
    {
        if (disableAdverts == true) return;

        if (showAdsOnMainMenu == true && adsOnMainMenuIntervalCounter == adsOnMainMenuInterval)
        {
            adsOnMainMenuIntervalCounter = 0;
            Debug.Log("ADVERT SHOWING ON MAIN MENU");

            if (useAdvertGuiContainer == true)
            {
                advertGuiContainer.SetActive(true);
            }

            if (onAdvertShowed != null)
            {
                onAdvertShowed.Invoke();
            }
            //////////////////////////////////////////
            // CALL YOUR ADVERT SHOW METHOD HERE
            // .......
            //////////////////////////////////////////
        }

        adsOnMainMenuIntervalCounter++;


    }

    public void showAdvertOnGameOver()
    {

        if (disableAdverts == true) return;

        if (showAdsOnGameOver == true && adsOnGameOverIntervalCounter == adsOnGameOverInterval)
        {

            adsOnGameOverIntervalCounter = 0;
            Debug.Log("ADVERT SHOWING ON GAME OVER");

            if (useAdvertGuiContainer == true)
            {
                advertGuiContainer.SetActive(true);
            }

            if (onAdvertShowed != null)
            {
                onAdvertShowed.Invoke();
            }

            //////////////////////////////////////////
            // CALL YOUR ADVERT SHOW METHOD HERE
            // .......
            //////////////////////////////////////////
        }

        adsOnGameOverIntervalCounter++;


    }

}
