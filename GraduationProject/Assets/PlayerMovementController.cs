using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementController : NetworkBehaviour {

    [SyncVar]
    public Vector3 playerPos;

    private Rigidbody rb;
	public float speed;
    public Camera camera_rig;
    public GameObject CameraRig;
    public GameObject UIParent;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        if (GameObject.Find("U_CharacterFront(Clone)") != null)
        {
            //it exists
            if (GameObject.Find("U_CharacterFront(Clone)1") != null)
            {
                //it exists
                gameObject.name = "U_CharacterFront(Clone)2";
                GameObject Player2_lefthand;
                Player2_lefthand = GameObject.Find("Player_lefthand");
                Player2_lefthand.name = "Player2_lefthand";
                GameObject Player2_righthand;
                Player2_righthand = GameObject.Find("Player_righthand");
                Player2_righthand.name = "Player2_righthand";
            }
            else
            {
                gameObject.name = "U_CharacterFront(Clone)1";
                GameObject Player1_lefthand;
                Player1_lefthand = GameObject.Find("Player_lefthand");
                Player1_lefthand.name = "Player1_lefthand";
                GameObject Player1_righthand;
                Player1_righthand = GameObject.Find("Player_righthand");
                Player1_righthand.name = "Player1_righthand";
            }
                
        }

        CameraRig = GameObject.Find("[CameraRig]"); 
        camera_rig = CameraRig.transform.Find("Camera (eye)").GetComponent<Camera>();


        rb = gameObject.GetComponent<Rigidbody>();
        offset = new Vector3(0, 0, 0f);
        //offset = camera_rig.transform.position - gameObject.transform.position;
        offset.y = 0;
        gameObject.transform.parent = CameraRig.transform;
    }
	
	// Update is called once per frame
	void Update () {
        //gameObject.transform.parent = CameraRig.transform;
        //UIParent = GameObject.Find("Canvas");
        //UIParent.transform.Find("GameOver").GetComponent<Text>().text = ": " +  Player2X + ", " + Player2Y + ", " + Player2Z;
        if (GameObject.Find("U_CharacterFront(Clone)2") != null && gameObject.name == "U_CharacterFront(Clone)2")
        {
            //it exists
            GameObject Player2_lefthand;
            Player2_lefthand = GameObject.Find("Player_lefthand");
            if (Player2_lefthand != null)
                Player2_lefthand.name = "Player2_lefthand";
            GameObject Player2_righthand;
            Player2_righthand = GameObject.Find("Player_righthand");
            if (Player2_righthand != null)
                Player2_righthand.name = "Player2_righthand";
        }
        else if(GameObject.Find("U_CharacterFront(Clone)1") != null)
        {
            GameObject Player1_lefthand;
            Player1_lefthand = GameObject.Find("Player_lefthand");
            if(Player1_lefthand != null)
                Player1_lefthand.name = "Player1_lefthand";
            GameObject Player1_righthand;
            Player1_righthand = GameObject.Find("Player_righthand");
            if (Player1_righthand != null)
                Player1_righthand.name = "Player1_righthand";
        }

        //rb.MovePosition(gameObject.transform.position + gameObject.transform.forward * Time.deltaTime * speed);
        //var collider = Player1.transform.Find("CowBoy").gameObject.GetComponent<MeshCollider>();
        //collider.sharedMesh =  ProceduralGen.GetNewMesh();
        if (gameObject.name == "U_CharacterFront(Clone)1")
            gameObject.transform.position = new Vector3(camera_rig.transform.position.x, gameObject.transform.position.y, camera_rig.transform.position.z) + offset;
    }

	
}
