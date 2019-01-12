using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//using UnityEngine.XR;


public class MoveCameraRig : NetworkBehaviour {
    public float startTime;
    public float currentTime;
    private float timeRound = 1f;
    public int oil;
    public bool StopMoving;
    public int WALL;
    public int speed;
    public Transform cameraRigTransform;
    public Transform headTransform; // The camera rig's head
    public Vector3 teleportReticleOffset; // Offset from the floor for the reticle to avoid z-fighting
    public LayerMask teleportMask; // Mask to filter out areas where teleports are allowed

    private SteamVR_TrackedObject trackedObj;

    public GameObject laserPrefab; // The laser prefab
    private GameObject laser; // A reference to the spawned laser
    private Transform laserTransform; // The transform component of the laser for ease of use

    private Vector3 hitPoint; // Point where the raycast hits

    public AvatarController m_avatarController;
    public AvatarController m_avatarController2;
    public GameObject Player1;
    public GameObject Player2;
    public DetectHumanPose m_detecthumanpose;
    public int StartScore;
    public int UpdateScore;
    public int realScore;
    public GameObject UIParent;
    public Text scoreView;
    public GameObject cube;
    public int stage = 1;
    public bool startGame;
    // Use this for initialization
    void Start () {
        startGame = false;
        oil = 0;
        WALL = 0;
        StopMoving = false;
        speed = 30;
        //InputTracking.disablePositionalTracking = true;
        StartScore = 0;
        //cube = GameObject.Find("Cube");
        startTime = (int)Time.time;
        currentTime = 60f;
    }
	
	// Update is called once per frame
	void Update () {
        Player2 = GameObject.Find("U_CharacterFront(Clone)2");
        if (Player2 == null) return;

        if(startGame == false)
        {
            if (Input.GetKey("s"))
            {
                startGame = true;
            }
            else
            {
                return;
            }
        }
        

        // update time show
        if ((int)Time.time != startTime && currentTime > 0)
        {
            startTime = (int)Time.time;
            currentTime = currentTime - 1;
            
            UIParent = GameObject.Find("Canvas");
            UIParent.transform.Find("Time").GetComponent<Text>().text = "Time: " + (int)currentTime;
        }

        if (currentTime == -1)
        {
            // Already print game over
            // wait for restart
            Player1 = GameObject.Find("U_CharacterFront(Clone)1");
            Player2 = GameObject.Find("U_CharacterFront(Clone)2");

            if (Player1 != null)
            {
                m_avatarController = Player1.GetComponent<AvatarController>();
                if (Player2 != null)
                {
                    m_avatarController2 = Player2.GetComponent<AvatarController>();
                    if(Input.GetKey("r") || (m_avatarController.ifRightHandGrip && m_avatarController2.ifRightHandGrip))
                    {
                        currentTime = 60f;
                        startTime = (int)Time.time;
                        UIParent = GameObject.Find("Canvas");
                        UIParent.transform.Find("Time").GetComponent<Text>().text = "Time: " + (int)currentTime;

                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                        StartScore = UpdateScore;
                        realScore = 0;
                        UIParent = GameObject.Find("Canvas");
                        UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + "0";
                        UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
                        Time.timeScale = 1;
                    }
                }
                else
                {
                    if (Input.GetKey("r") || (m_avatarController.ifRightHandGrip))
                    {
                        currentTime = 60f;
                        startTime = (int)Time.time;
                        UIParent = GameObject.Find("Canvas");
                        UIParent.transform.Find("Time").GetComponent<Text>().text = "Time: " + (int)currentTime;

                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                        StartScore = UpdateScore;
                        realScore = 0;
                        UIParent = GameObject.Find("Canvas");
                        UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + "0";
                        UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
                        Time.timeScale = 1;
                    }
                }
            }
        }
        /*
        if (m_detecthumanpose.handTogether)
        {
            UIParent = GameObject.Find("Canvas");
            UIParent.transform.Find("Pose").GetComponent<Text>().text = "Shake hands!!!";
        }
        else
        {
            UIParent = GameObject.Find("Canvas");
            UIParent.transform.Find("Pose").GetComponent<Text>().text = "";
        }*/


        Player1 = GameObject.Find("U_CharacterFront(Clone)1");
        if (Player1 != null)
        {
            m_avatarController = Player1.GetComponent<AvatarController>();
            if (Mathf.Abs(m_detecthumanpose.Player1_handVelocity) > 0.3)
            {
                oil = oil + 10;
            }
            else if (oil > 0)
            {
                oil = oil - 1;
            }
            if (currentTime > 0)
            {
                if (!StopMoving && (Input.GetKey("w") || oil > 3))
                {
                    //Debug.Log(m_detecthumanpose.Player1_handVelocity);
                    Teleport();
                }
            }
            else if(currentTime == 0)
            {
                currentTime = -1;
                // End Game: Show Final Score
                UIParent = GameObject.Find("Canvas");
                UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + realScore;
                Time.timeScale = 0;
            }

        }

        UpdateScore = (int)gameObject.transform.position.z;
        if ((UpdateScore - StartScore) % 50 == 0 && UpdateScore != StartScore)
        {
            StartScore = UpdateScore;
            realScore = realScore + 50;
            UIParent = GameObject.Find("Canvas");
            UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + realScore;

        }

        if (gameObject.transform.position.y < 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 2, gameObject.transform.position.z);
        }

        // ****************************
        //      Find HandsUp
        // ****************************
        if (WALL == -1)
        {
            StopMoving = false;
            WALL = 0;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 15);
        }

    }
            
    

    private void Teleport()
    {
        Vector3 difference = cameraRigTransform.position - headTransform.position; // Calculate the difference between the center of the virtual room & the player's head
        difference.y = 0; // Don't change the final position's y position, it should always be equal to that of the hit point

        
        cameraRigTransform.position += new Vector3(0.0f, 0.0f, speed * Time.deltaTime); // Change the camera rig position to where the the teleport reticle was. Also add the difference so the new virtual room position is relative to the player position, allowing the player's new position to be exactly where they pointed. (see illustration)
        
    }

    float maxJumpHeight = 15.0f;
    float groundHeight;
    Vector3 groundPos;
    float jumpSpeed = 25.0f;
    float fallSpeed = 15.0f;
    public bool inputJump = false;
    public bool grounded = true;
    bool ifJumping = false;


    IEnumerator Jump()
    {
        ifJumping = true;
        while (true)
        {
            if (transform.position.y >= maxJumpHeight)
                inputJump = false;
            if (inputJump)
                transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
            else if (!inputJump)
            {
                
                transform.Translate(Vector3.down * fallSpeed * Time.smoothDeltaTime);
                if (transform.position.y < cube.transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, (int)cube.transform.position.y, transform.position.z);
                    StopAllCoroutines();
                    ifJumping = false;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

}
