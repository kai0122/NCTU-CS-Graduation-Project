using System;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{


    private TrackObject trackObject;

    private void Awake()
    {

        trackObject = this.GetComponent<TrackObject>();

    }

    public void pickUp()
    {

        this.transform.parent = GameGlobals.Instance.trackGenerator.trackObjectsRoot.transform;
        this.transform.position = GameGlobals.Instance.trackGenerator.rootPos;
        this.trackObject.positioned = false;
        this.gameObject.SetActive(false);

        GameGlobals.Instance.audioController.playSound("PowerupPoof", false);
        GameGlobals.Instance.controller.doAnEffect(Controller.EffetcType.PowerupPickUp);
      
        switch (trackObject.objectType)
        {
            case TrackObject.ObjectType.PickableLetter:
                GameGlobals.Instance.powerupController.doLetterCollected();
                break;
            case TrackObject.ObjectType.PickableFruit:
                GameGlobals.Instance.powerupController.doFruitCollected();
                break;
            case TrackObject.ObjectType.PowerupVacuum:
                GameGlobals.Instance.powerupController.doVacuumCollected();
                break;
        }

    }

}
