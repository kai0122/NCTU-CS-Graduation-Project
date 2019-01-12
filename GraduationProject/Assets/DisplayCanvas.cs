using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayCanvas : MonoBehaviour {
    private GameObject tempObject, CameraEyeObject;
    private Canvas EscCan;
    private Camera CameraEye;
    // Use this for initialization
	void Start () {
        GameObject tempObject = GameObject.Find("Canvas");
        if (tempObject != null)
        {
            //If we found the object , get the Canvas component from it.
            EscCan = tempObject.GetComponent<Canvas>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        CameraEyeObject = GameObject.Find("Camera (eye)");
        if (CameraEyeObject)
        {
            CameraEye = CameraEyeObject.GetComponent<Camera>();
        }
        EscCan.worldCamera = CameraEye;

    }
}
