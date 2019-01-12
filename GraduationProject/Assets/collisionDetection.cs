using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class collisionDetection : MonoBehaviour {
    public bool realGameOver;
    public GameObject player1; 
    public AvatarController m_avatarController;
    public GameObject player2;
    public AvatarController m_avatarController2;
    public GameObject Player1_lefthand;
    public GameObject Player1_righthand;
    public GameObject Player2_lefthand;
    public GameObject Player2_righthand;
    private bool endGame;
    public GameObject UIParent;
    //public Text gameOverView;
    public GameObject CameraRig;
    public MoveCameraRig m_MoveCameraRig;
    public GameObject Environment;
    public newTrack m_newTrack;
    // Use this for initialization
    void Start () {
        player1 = GameObject.Find("U_CharacterFront(Clone)1");
        UIParent = GameObject.Find("Canvas");
        UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
        m_avatarController = player1.GetComponent<AvatarController>();
        endGame = false;
        Time.timeScale = 1;
        CameraRig = GameObject.Find("[CameraRig]");
        m_MoveCameraRig = CameraRig.GetComponent<MoveCameraRig>();
        Environment = GameObject.Find("Environment");
        m_newTrack = Environment.GetComponent<newTrack>();
        Player1_lefthand = null;
        Player1_righthand = null;
        Player2_lefthand = null;
        Player2_righthand = null;
        realGameOver = false;
    }
	
	// Update is called once per frame
	void Update () {
        player1 = GameObject.Find("U_CharacterFront(Clone)1");
        if(player1!=null) m_avatarController = player1.GetComponent<AvatarController>();
        player2 = GameObject.Find("U_CharacterFront(Clone)2");
        if (player2 != null) m_avatarController2 = player2.GetComponent<AvatarController>();

        if (GameObject.Find("U_CharacterFront(Clone)1") != null)
        {
            //it exists
            Player1_lefthand = GameObject.Find("Player1_lefthand");
            Player1_righthand = GameObject.Find("Player1_righthand");
            //Debug.Log(m_avatarcontroller.bones.Length);
            //block.transform.position = Player1_lefthand.transform.position;
        }
        if (GameObject.Find("U_CharacterFront(Clone)2") != null)
        {
            //it exists
            Player2_lefthand = GameObject.Find("Player2_lefthand");
            Player2_righthand = GameObject.Find("Player2_righthand");
        }

        if (player2 != null)
        {
            if (realGameOver && (Input.GetKeyDown("r") || (m_avatarController.ifRightHandGrip && m_avatarController2.ifRightHandGrip)))
            {
                endGame = false;
                realGameOver = false;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, CameraRig.transform.position.y, CameraRig.transform.position.z - 10);
                //scoreView.text = "Score: " + 0;

                m_MoveCameraRig.StartScore = m_MoveCameraRig.UpdateScore;
                m_MoveCameraRig.realScore = 0;
                m_MoveCameraRig.currentTime = 60f;
                m_MoveCameraRig.startTime = Time.time;

                UIParent = GameObject.Find("Canvas");
                UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + "0";
                UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
                //gameOverView.GetComponent<Text>().enabled = false;
                Time.timeScale = 1;
            }
            if (endGame && (Input.GetKeyDown("r") || (player1.transform.position.y > 0.1 && player2.transform.position.y > 0.1)))
            {

                endGame = false;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, CameraRig.transform.position.y, CameraRig.transform.position.z + 5);
                //scoreView.text = "Score: " + 0;

                if(m_MoveCameraRig.realScore >= 50)
                {
                    m_MoveCameraRig.realScore = m_MoveCameraRig.realScore - 50;
                }
                else
                {
                    m_MoveCameraRig.realScore = 0;
                }
                
                UIParent = GameObject.Find("Canvas");
                UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + (m_MoveCameraRig.realScore - 30);
                UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
                //gameOverView.GetComponent<Text>().enabled = false;
                //Time.timeScale = 1;
                m_MoveCameraRig.StopMoving = false;
            }
        }
        else
        {
            if (realGameOver && (Input.GetKeyDown("r") || (m_avatarController.ifRightHandGrip)))
            {
                endGame = false;
                realGameOver = false;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, CameraRig.transform.position.y, CameraRig.transform.position.z - 10);
                //scoreView.text = "Score: " + 0;

                m_MoveCameraRig.StartScore = m_MoveCameraRig.UpdateScore;
                m_MoveCameraRig.realScore = 0;
                m_MoveCameraRig.currentTime = 60f;
                m_MoveCameraRig.startTime = Time.time;
                UIParent = GameObject.Find("Canvas");
                UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + "0";
                UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
                //gameOverView.GetComponent<Text>().enabled = false;
                Time.timeScale = 1;
            }
            if (endGame && (Input.GetKeyDown("r") || player1.transform.position.y > 0.1))
            {

                endGame = false;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, CameraRig.transform.position.y, CameraRig.transform.position.z + 5);
                //scoreView.text = "Score: " + 0;

                if (m_MoveCameraRig.realScore >= 50)
                {
                    m_MoveCameraRig.realScore = m_MoveCameraRig.realScore - 50;
                }
                else
                {
                    m_MoveCameraRig.realScore = 0;
                }
                UIParent = GameObject.Find("Canvas");
                UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + (m_MoveCameraRig.realScore - 30);
                UIParent.transform.Find("GameOver").GetComponent<Text>().text = "";
                //gameOverView.GetComponent<Text>().enabled = false;
                //Time.timeScale = 1;
                m_MoveCameraRig.StopMoving = false;
            }
        }
        

        if(m_MoveCameraRig.StopMoving == true)
        {
            if (m_MoveCameraRig.WALL == 1)
            {
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7 || Player2_lefthand.transform.position.y > 7 || Player2_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }

                    }
                }
            }
            if (m_MoveCameraRig.WALL == 2)
            {
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }
                    }
                }
            }
            if (m_MoveCameraRig.WALL == 3)
            {
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }

                    }
                }
            }
            if (m_MoveCameraRig.WALL == 4)
            {
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y < 9 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }

                    }
                }
            }
            if (m_MoveCameraRig.WALL == 5)
            {
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y > 7 || Player1_righthand.transform.position.y < 9 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y > 7 || Player1_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                        }
                        else
                        {
                            m_MoveCameraRig.WALL = -1;
                        }

                    }
                }
            }
        }

	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        //Debug.Log(other.tag);
        if (other.tag == "obstacle")
        {
            if (other.name == "barrier_updown")
            {
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (player1.transform.position.y < 0.1 || player2.transform.position.y < 0.1)
                        {
                            Debug.Log("End Game");
                            m_MoveCameraRig.StopMoving = true;
                            //Time.timeScale = 0;
                            endGame = true;
                            UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Please Jump together!!!";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }
                    }
                    else
                    {
                        if (player1.transform.position.y < 0.1)
                        {
                            Debug.Log("End Game");
                            m_MoveCameraRig.StopMoving = true;
                            //Time.timeScale = 0;
                            endGame = true;
                            UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Please Jump together!!!";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }

                    }
                }
            }
            else if (other.name == "HandsUp")
            {
                m_MoveCameraRig.StopMoving = true;
                m_MoveCameraRig.WALL = 1;
                if (player1 != null)
                {
                    if (player2 != null)
                    {

                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7 || Player2_lefthand.transform.position.y > 7 || Player2_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;

                        }

                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }

                    }
                }
            }
            else if (other.name == "HandsUp2")
            {
                m_MoveCameraRig.StopMoving = true;
                m_MoveCameraRig.WALL = 2;
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }

                    }
                }
            }
            else if (other.name == "HandsUp3")
            {
                m_MoveCameraRig.StopMoving = true;
                m_MoveCameraRig.WALL = 3;
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }

                    }
                }
            }
            else if (other.name == "HandsUp4")
            {
                m_MoveCameraRig.StopMoving = true;
                m_MoveCameraRig.WALL = 4;
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y < 9 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }
                    }
                    else
                    {
                        //Debug.Log(Player1_lefthand.transform.position.y);
                        //Debug.Log(Player1_righthand.transform.position.y);
                        if (Player1_lefthand.transform.position.y < 9 || Player1_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }

                    }
                }
            }
            else if (other.name == "HandsUp5")
            {
                m_MoveCameraRig.StopMoving = true;
                m_MoveCameraRig.WALL = 5;
                if (player1 != null)
                {
                    if (player2 != null)
                    {
                        if (Player1_lefthand.transform.position.y > 7 || Player1_righthand.transform.position.y < 9 || Player2_lefthand.transform.position.y < 9 || Player2_righthand.transform.position.y > 7)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }
                    }
                    else
                    {
                        if (Player1_lefthand.transform.position.y > 7 || Player1_righthand.transform.position.y < 9)
                        {
                            Debug.Log("End Game");
                            //Time.timeScale = 0;
                            //endGame = true;
                            //UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over";
                            //gameOverView.GetComponent<Text>().enabled = true;
                        }

                    }
                }
            }
            else
            {
                Debug.Log("End Game");
                Time.timeScale = 0;
                endGame = true;
                realGameOver = true;
                UIParent.transform.Find("GameOver").GetComponent<Text>().text = "Game Over!!! Please grip right hand to restart!!!";
                UIParent.transform.Find("score").GetComponent<Text>().text = "Score: " + m_MoveCameraRig.realScore;

                //gameOverView.GetComponent<Text>().enabled = true;
            }




        }

    }
}
