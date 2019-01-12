using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGlobals : MonoBehaviour {


    // Singleton
    private static GameGlobals instance = null;
    public static GameGlobals Instance
    {
        get
        {
            return instance;
        }
    }


    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject playerController;
    [HideInInspector]
    public Controller controller;
    [HideInInspector]
    public CameraController cameraController;
    [HideInInspector]
    public Camera mainCamera;
    [HideInInspector]
    public TrackGenerator trackGenerator;
    [HideInInspector]
    public Achievements achievements;
    [HideInInspector]
    public PowerupController powerupController;
    [HideInInspector]
    public AudioController audioController;
    [HideInInspector]
    public TouchController touchController;

    // Updated by GameState Component
    [HideInInspector]
    public string currentGameState;

    public float bendingX;
    public float bendingY;



   
    private TweenFOV cameraFovEffetc;
    public GameObject highScorePanel;
    public GameObject messageBoxPanel;
    public GameObject tutorialPanel;

    [HideInInspector]
    public bool gameAutoStart;
    public GameState openingSceneState;
    public GameState escapeSceneState;
    public GameState pauseGameState;
    public GameState resumeGameState;
    public GameState villainCheerState;
    public GameState highScoreState;

    // Auto starts game when player hits replay button at the score scene
    public void checkAutoStart()
    {
        if (gameAutoStart == false)
        {
            if (openingSceneState != null)
            {
                openingSceneState.ExecuteAll();
            }   
        }
        else
        {
            if (escapeSceneState != null)
            {
                escapeSceneState.ExecuteAll();
            }
            gameAutoStart = false;
        }
    }

    public void setGameAutoStart()
    {
        gameAutoStart = true;
    }

    private void Awake()
    {


        // Singleton
        instance = this;


        // Begin
        Application.targetFrameRate = 60;

        highScorePanel.SetActive(false);

        // First Start Defaulu Settings
        if (PlayerPrefs.GetInt("firstStart", 0) == 0)
        {
            PlayerPrefs.SetInt("firstStart", 1);
            PlayerPrefs.SetInt("rate_complete", 0);
            PlayerPrefs.SetInt("rate_peak", 16);

            PlayerPrefs.SetInt("audio", 1);
            PlayerPrefs.SetInt("highscore", 0);
            PlayerPrefs.SetInt("ads_unitypeak", 5);
            PlayerPrefs.SetInt("ads_chartboostpeak", 0);
        }


        // Assing Bending Values
        Shader.SetGlobalFloat("_V_CW_X_Bend_Size", bendingX);
        Shader.SetGlobalFloat("_V_CW_Y_Bend_Size", bendingY);
        Shader.SetGlobalFloat("_V_CW_Z_Bend_Size", 0);
        Shader.SetGlobalFloat("_V_CW_Z_Bend_Bias", 0);
        Shader.SetGlobalFloat("_V_CW_Camera_Bend_Offset", 0);



        player = GameObject.Find("Player");
        playerController = GameObject.Find("playerController");

        controller = GetComponent<Controller>();
        audioController = GetComponent<AudioController>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraFovEffetc = GameObject.Find("Main Camera").GetComponent<TweenFOV>();

        cameraController = GetComponent<CameraController>();

        trackGenerator = GetComponent<TrackGenerator>();
        achievements = GetComponent<Achievements>();
        powerupController = GetComponent<PowerupController>();
        touchController = GetComponent<TouchController>();


    


    }



    private void FixedUpdate()
    {
        //handleBendDirection();
    }

    private int backButtonDelay;

    private void Update()
    {

        // Back button
        // ANDROID BACK BUTTON EXIT MENU

        if (Input.GetKey(KeyCode.Escape) && backButtonDelay <= 0)
        {

        
            backButtonDelay = 50;

            if (currentGameState == "OnGameRunning")
            {
                pauseGameState.ExecuteAll();
                return;
            }

            if (currentGameState == "onPauseGame")
            {
                resumeGameState.ExecuteAll();
                playPlayerAnimaiton("run");
                return;
            }

            if (currentGameState == "OnOpeningScene")
            {
                showQuitDialog();
                return;
            }
        

        }

        if (backButtonDelay > 0)
        {
            backButtonDelay--;
        }


    }


  
    // Quit Dailog
    private void showQuitDialog()
    {
        DialogWindow.Instance.showDialog(DialogWindow.DialogType.YesNo, "Are you sure you want to quit the ?", onQuitDialogResult);

    }


    private void onQuitDialogResult(DialogWindow.DialogResult result)
    {
   
        if (result == DialogWindow.DialogResult.Yes)
        {
            Application.Quit();
        }

        backButtonDelay = 0;
        /*
        DialogWindow.closeDialog();
        DialogWindow.onResultMethod = null;
        */

    }


    private void OnApplicationQuit()
    {

        // Reset bending for design
        Shader.SetGlobalFloat("_V_CW_X_Bend_Size", 0);
        Shader.SetGlobalFloat("_V_CW_Y_Bend_Size", 0);
        Shader.SetGlobalFloat("_V_CW_Z_Bend_Size", 0);
        Shader.SetGlobalFloat("_V_CW_Z_Bend_Bias", 0);
        Shader.SetGlobalFloat("_V_CW_Camera_Bend_Offset", 0);

    }

    private void OnApplicationPause(bool pauseStatus)
    {
      
    }


    public void onGameStart()
    {

    }

    public void doCameraFovEffect()
    {

        if (cameraFovEffetc != null)
        {
            cameraFovEffetc.ResetToBeginning();
            cameraFovEffetc.PlayForward();
        }
    }

    public void ResetGame()
    {

        isHighScoreRecorded = false;
        controller.Reset();
        trackGenerator.Reset();
        //cameraController.Reset();
        achievements.Reset();
        powerupController.Reset();
        CharacterObstacle.Reset();
        CriticalModeController.Instance.Reset();

        Sign.ShuffleSings();

        

    }

  

    public void handlePause(bool isPaused)
    {

        MovingObstacle.handlePauseAll(isPaused);
        CharacterObstacle.hanlePause(isPaused);

    }

    public void handleDeath()
    {
        CharacterObstacle.handleDeath();
    }


    public bool isInGamePlay()
    {
        if (GameGlobals.Instance.currentGameState == null) return false;

        if (GameGlobals.Instance.currentGameState.Equals("OnGameRunning"))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool isInDeathScene()
    {
        /*
        if (gameFSM.ActiveStateName == "PreDeath" || gameFSM.ActiveStateName == "Death Scene")
        {
            return true;
        }
        else
        {
            return false;
        }
        */

        return false;
    }

    public bool isInCriticalMode()
    {
        //Debug.Log(villainsFSM.ActiveStateName);

        /*
        if (villainsFSM.ActiveStateName == "Listening")
        {
            return false;
        }
        else
        {
            return true;
        }
        */
        return false;
    }

    // Global Player Handlers ------------------------------------------------------------------------------------------------------------------------------------------------------

    public void playPlayerAnimaiton(string animName)
    {

        if (playerController != null)
        {
            Animator playerAnimator = playerController.GetComponent<Animator>();
            playerAnimator.Play(animName, 0, 0);
        }

    }

    public void movePlayerToDefaultArea()
    {
        if (playerController != null)
        {
            //ShopWindow.SetLayerOnAllRecursive(playerController.gameObject, "Default");
            playerController.transform.parent = player.transform;
            playerController.transform.localPosition = new Vector3(0, 0, 0);
            playerController.transform.localRotation = new Quaternion(0, 0, 0, 0);
            playerController.transform.localScale = new Vector3(1, 1, 1);

        }
    }

    public void movePlayerToHighScoreArea()
    {
        Transform highScoreArea = GameObject.Find("HighScrore").transform.Find("playerHighScoreHolder");

        if (playerController != null)
        {
            //ShopWindow.SetLayerOnAllRecursive(playerController.gameObject, "UI");

            playerController.transform.parent = highScoreArea;
            playerController.transform.localPosition = new Vector3(0, 80.0f, -75.0f);
            playerController.transform.localRotation = new Quaternion(0, 180.0f, 0, 0);
            playerController.transform.localScale = new Vector3(50, 50, 50);

            Animator playerAnimator = playerController.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.Play("hiphop", 0, 0);
            }

        }
    }



    public void disableMovingObstacles()
    {
        MovingObstacle.ActivateResumeMotion();
        MovingCoin.ActivateResumeMotion();
    }

    public void setGroundedPlayer()
    {
        StartCoroutine(movePlayerToLastPosition());
    }


    private IEnumerator movePlayerToLastPosition()
    {

        int posTimer = 0;
        float duration = 60;

        GameObject villains = GameObject.Find("villians");
        CharacterController characterController = player.GetComponent<CharacterController>();

        float destPos = 0;
        switch (controller.trackIndex)
        {
            case -1:
                destPos = -6.0f;
                break;
            case 0:
                destPos = 0;
                break;
            case 1:
                destPos = 6.0f;
                break;
        }


        while (true)
        {

            if (posTimer >= duration)
            {
                break;
            }

            controller.verticalSpeed -= controller.gravity * Time.deltaTime;
            Vector3 yPos = (Vector3)((controller.verticalSpeed * Time.deltaTime) * Vector3.up);


            float delta = 1.0f / duration * posTimer;


            Vector3 currentPos = player.transform.position;
            Vector3 nextPos = Vector3.right * Mathf.Lerp(player.transform.position.x, destPos, delta);
            Vector3 xpos = new Vector3((nextPos - currentPos).x, 0, 0);

            characterController.Move(yPos);
            characterController.Move(xpos);

            villains.transform.position = player.transform.position;

            posTimer++;

            yield return new WaitForSeconds(Time.fixedDeltaTime);

        }


    
    }




    // -------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void goToMainMenu()
    {
    }

    [HideInInspector]
    public bool isHighScoreRecorded;
    public void checkHighScoreRecorded()
    {

        if (isHighScoreRecorded == false)
        {
           villainCheerState.ExecuteAll();
        }
        else
        {
            highScorePanel.SetActive(true);
            highScoreState.ExecuteAll();
        }
    }


    public void testDialog()
    {
        DialogWindow.Instance.showDialog(DialogWindow.DialogType.YesNo, "NABER LA ?", testResult);
    }

    public void testResult(DialogWindow.DialogResult result)
    {
        Debug.Log("DIALOG GAPANDI " + result);
    }



    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

}
