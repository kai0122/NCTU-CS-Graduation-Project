using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class ServerCanvas : NetworkBehaviour {

    [SyncVar]
    public string Score;
    [SyncVar]
    public string GameOver;
    [SyncVar]
    public string Time;
    [SyncVar]
    public float X;
    [SyncVar]
    public float Z;
    [SyncVar]
    public Vector3 playerPos;

    // Use this for initialization
    void Start () {
        
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        Score = GameObject.Find("score").GetComponent<Text>().text;
        GameOver = GameObject.Find("GameOver").GetComponent<Text>().text;
        Time = GameObject.Find("Time").GetComponent<Text>().text;

        //GameObject.Find("U_CharacterFront(Clone)2").transform.position = position;
        if (GameObject.Find("U_CharacterFront(Clone)2") != null)
            GameObject.Find("GameOver").GetComponent<Text>().text = (int)X + "," + Z;
    }
}
