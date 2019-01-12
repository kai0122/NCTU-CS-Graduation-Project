using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHumanPose : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    private AvatarController m_avatarcontroller;
    private AvatarController m_avatarcontroller2;
    public GameObject Player1_lefthand;
    public Vector3 Player1_lefthand_now;
    public Vector3 Player1_lefthand_last;
    public GameObject Player1_righthand;
    public Vector3 Player1_righthand_now;
    public Vector3 Player1_righthand_last;
    public GameObject Player2_lefthand;
    public Vector3 Player2_lefthand_now;
    public Vector3 Player2_lefthand_last;
    public GameObject Player2_righthand;
    public Vector3 Player2_righthand_now;
    public Vector3 Player2_righthand_last;
    public float Player1_handVelocity;
    public float Player2_handVelocity;
    public bool handTogether;
    public GameObject block;

    // Use this for initialization
    void Start () {
        block = GameObject.Find("Cube (1)");
        Player1_lefthand = null;
        Player1_righthand = null;
        Player2_lefthand = null;
        Player2_righthand = null;
        Player1_lefthand_last = new Vector3(0, 0, 0);
        Player2_lefthand_last = new Vector3(0, 0, 0);

        handTogether = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Player1_lefthand == null)
        {
            if (GameObject.Find("U_CharacterFront(Clone)1") != null)
            {
                player1 = GameObject.Find("U_CharacterFront(Clone)1");
                //it exists
                m_avatarcontroller = GameObject.Find("U_CharacterFront(Clone)1").GetComponent<AvatarController>();
                Player1_lefthand = GameObject.Find("Player1_lefthand");
                Player1_righthand = GameObject.Find("Player1_righthand");
                Player1_lefthand_last = Player1_lefthand.transform.position - player1.transform.position;
                Player1_righthand_last = Player1_righthand.transform.position - player1.transform.position;
                //Debug.Log(Player1_lefthand.transform.position);
            }
        }
        else
        {
            Player1_lefthand_now = Player1_lefthand.transform.position - player1.transform.position;
            Player1_righthand_now = Player1_righthand.transform.position - player1.transform.position;

            Player1_handVelocity = ((Player1_lefthand_now - Player1_lefthand_last)).x + ((Player1_lefthand_now - Player1_lefthand_last)).y + ((Player1_lefthand_now - Player1_lefthand_last)).z + ((Player1_righthand_now - Player1_righthand_last)).x + ((Player1_righthand_now - Player1_righthand_last)).y + ((Player1_righthand_now - Player1_righthand_last)).z;
            //Debug.Log(Player1_handVelocity);
            //Debug.Log("--------------------------");
            
            //Debug.Log(Player1_lefthand_now);
            //Debug.Log(Player1_lefthand_last - Player1_lefthand_now);

            Player1_lefthand_last = Player1_lefthand.transform.position - player1.transform.position;
            Player1_righthand_last = Player1_righthand.transform.position - player1.transform.position;
        }

        if (Player2_lefthand == null)
        {
            if (GameObject.Find("U_CharacterFront(Clone)2") != null)
            {
                //it exists
                m_avatarcontroller2 = GameObject.Find("U_CharacterFront(Clone)2").GetComponent<AvatarController>();
                Player2_lefthand = GameObject.Find("Player2_lefthand");
                Player2_righthand = GameObject.Find("Player2_righthand");
                Player2_lefthand_last = Player2_lefthand.transform.position;
                Player2_righthand_last = Player2_righthand.transform.position;
                //Debug.Log(Player2_lefthand.transform.position);
            }
        }
        else
        {
            Player2_lefthand_now = Player2_lefthand.transform.position;
            Player2_righthand_now = Player2_righthand.transform.position;

            Player2_handVelocity = ((Player2_lefthand_now - Player2_lefthand_last)).x + ((Player2_lefthand_now - Player2_lefthand_last)).y + ((Player2_lefthand_now - Player2_lefthand_last)).z + ((Player2_righthand_now - Player2_righthand_last)).x + ((Player2_righthand_now - Player2_righthand_last)).y + ((Player2_righthand_now - Player2_righthand_last)).z;
            //Debug.Log(Player2_handVelocity);
            //Debug.Log("--------------------------");

            //Debug.Log(Player1_lefthand_now);
            //Debug.Log(Player1_lefthand_last - Player1_lefthand_now);

            Player2_lefthand_last = Player2_lefthand.transform.position;
            Player2_righthand_last = Player2_righthand.transform.position;
        }
        /*
        if(GameObject.Find("U_CharacterFront(Clone)1") != null && GameObject.Find("U_CharacterFront(Clone)2") != null)
        {
            // ***********************
            //      Hand detection
            // ***********************
            if (Player1_lefthand != null && Player2_righthand != null)
            {
                if (Vector3.Distance(Player1_lefthand.transform.position, Player2_righthand.transform.position) < 2)
                {
                    handTogether = true;
                    Debug.Log("握手!!!");
                }
                else
                {
                    handTogether = false;
                }
            }
            if (Player1_lefthand != null && Player2_lefthand != null)
            {
                if (Vector3.Distance(Player1_lefthand.transform.position, Player2_lefthand.transform.position) < 2)
                {
                    handTogether = true;
                    Debug.Log("握手!!!");
                }
                else
                {
                    handTogether = false;
                }
            }
            if (Player1_righthand != null && Player2_lefthand != null)
            {
                if (Vector3.Distance(Player1_righthand.transform.position, Player2_righthand.transform.position) < 2)
                {
                    handTogether = true;
                    Debug.Log("握手!!!");
                }
                else
                {
                    handTogether = false;
                }
            }
            if (Player1_righthand != null && Player2_lefthand != null)
            {
                if (Vector3.Distance(Player1_righthand.transform.position, Player2_lefthand.transform.position) < 2)
                {
                    handTogether = true;
                    Debug.Log("握手!!!");
                }
                else
                {
                    handTogether = false;
                }
            }
        }*/



    }
}
