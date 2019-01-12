using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{

    [HideInInspector]
    public int currentCoins;
    [HideInInspector]
    public int currentScore;

    [HideInInspector]
    public int totalCoins;
    [HideInInspector]
    public int totalScore;

    [HideInInspector]
    public int highScore;

    public int minimumHighScore;


    public Text lblMainMenuTotalCoins, lblMainMenuTotalScore,lblMainMenuHighScore;
    public Text lblHudCoins, lblHudScore;
    public Text lblScorePanelCollectedCoins, lblScorePanelCollectedScore;
    public Text lblScorePanelTotalCoins, lblScorePanelTotalScore;

    public Text lblHudHighScoreCountert;
    public Text lblHighScorePanelScore;

    [HideInInspector]
    public bool scoresLock;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        currentCoins = 0;
        currentScore = 0;

        totalCoins = PlayerPrefs.GetInt("totalCoins", 0);
        totalScore = PlayerPrefs.GetInt("totalScore", 0);
        highScore = PlayerPrefs.GetInt("highscore", 0);

        if (highScore <= 0)
        {
            PlayerPrefs.SetInt("highscore", minimumHighScore);
        }
     
        lblMainMenuTotalCoins.text = totalCoins.ToString();
        lblMainMenuTotalScore.text = totalScore.ToString();
        lblMainMenuHighScore.text = highScore.ToString();


        lblScorePanelTotalCoins.text = totalCoins.ToString();
        lblScorePanelTotalScore.text = totalScore.ToString();

        highScoreOpenPanelTrig = false;
        highScoreRecordedPanelTrig = false;

    }


    public void resetScoresAndAchievements()
    {
        PlayerPrefs.SetInt("totalCoins", 0);
        PlayerPrefs.SetInt("totalScore", 0);
    }


    public void increasePoints(int point)
    {
     
        if (scoresLock == true) return;
        currentCoins += point;
     
    }

    public void increaseScore(int score)
    {
        if (scoresLock == true) return;
        currentScore += score;
    }


    // SCORE --------------------------------------------------------------------------------------------------

    private int bonusCoins = 2;
    private int pointsCounterDuration = 60;
    private int scoreCounterDuration = 80;
    private int pointsCounter, scoresCounter,scoreWaitCounter;

    private int  storedScore,storedtotalCoins,storedTotalScore;

    public void saveScores()
    {
        // Saving Scores
        PlayerPrefs.SetInt("totalCoins", totalCoins + currentCoins);
        PlayerPrefs.SetInt("totalScore", (totalScore + currentScore) + (currentCoins * bonusCoins));

        if (highScoreRecordedPanelTrig == true)
        {
            PlayerPrefs.SetInt("highscore", currentScore + (currentCoins * bonusCoins));

        }
    }

    public void coundDownScore()
    {

        Invoke("startCountDownScore", 3);
       
    }

    private void startCountDownScore()
    {
        storedScore = currentScore;
        storedtotalCoins = totalCoins;
        storedTotalScore = totalScore;

        pointsCounter = 0;
        scoresCounter = 0;
        scoreWaitCounter = 0;

        scoring = StartCoroutine(countScore());
    }

    private Coroutine scoring;
    public void cancelCountScore()
    {
        if (scoring != null)
        {
            StopCoroutine(scoring);
        }
    }

    private IEnumerator countScore()
    {

        int pitchTrigger = 0;
        AudioController.AudioItem endingAnimMusic = GameGlobals.Instance.audioController.getAudoClip("MusicScore");

        // Coins Counting
        while (true)
        {

         
            if (pointsCounter <= pointsCounterDuration)
            {
                float countedPoints = ((float)currentCoins / (float)pointsCounterDuration) * (float)pointsCounter;

                currentScore = storedScore + (int)countedPoints * bonusCoins;

                lblScorePanelCollectedCoins.text = (currentCoins - (int)countedPoints).ToString();
               
                totalCoins = storedtotalCoins + (int)countedPoints;
                lblScorePanelTotalCoins.text = totalCoins.ToString();


                // Audio
                float pitch = 1.0f + 0.3f / pointsCounterDuration * pointsCounter;
                if (pitchTrigger == 3)
                {
                    GameGlobals.Instance.audioController.playSoundPitched("PowerupDefault", pitch);
                    pitchTrigger = 0;
                }
                else
                {
                    pitchTrigger++;
                }
               

                pointsCounter++;
                yield return new WaitForSeconds(Time.fixedDeltaTime);

            }
            else
            {
                break;
            }

        }

        // Wait a while
        while (true)
        {

            if (scoreWaitCounter <= 100)
            {
                scoreWaitCounter++;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            else
            {
                break;
            }

        }

        // Scores Counting
        while (true)
        {

            if (scoresCounter <= scoreCounterDuration)
            {

                float countedScore = ((float)currentScore / (float)scoreCounterDuration) * (float)scoresCounter;

                lblScorePanelCollectedScore.text = (currentScore - (int)countedScore).ToString();
                totalScore = storedTotalScore + (int)countedScore;

                lblScorePanelTotalScore.text = totalScore.ToString();

                // Audio
                float pitch = 1.0f + 0.3f / scoreCounterDuration * scoresCounter;
                if (pitchTrigger == 3)
                {
                    GameGlobals.Instance.audioController.playSoundPitched("PowerupDefault", pitch);
                    pitchTrigger = 0;
                }
                else
                {
                    pitchTrigger++;
                }

                scoresCounter++;

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            else
            {
                break;
            }
     

        }

        // Waiting For Auto Start
        scoreWaitCounter = 0;
        if (PlayerPrefs.GetInt("audio", 1) == 0)
        {
            while (true)
            {

                if (scoreWaitCounter < 500)
                {
                    scoreWaitCounter++;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
                else
                {

                    GameGlobals.Instance.goToMainMenu();
                    break;
                }

            }
        }
        else if (PlayerPrefs.GetInt("audio", 1) == 1)
        {
            if (endingAnimMusic != null)
            {
                while (true)
                {

                    if (endingAnimMusic.audioSource.isPlaying == true)
                    {
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }
                    else
                    {
                        GameGlobals.Instance.goToMainMenu();
                        break;
                    }

                }
            }
            else
            {
                GameGlobals.Instance.goToMainMenu();
            }
        }


     
        
 


    }


    public bool isHighScoreRecorded()
    {
        if (PlayerPrefs.GetInt("highscore", 0) <= 0)
        {
            if (currentScore > minimumHighScore)
            {
                return true;
            }
        }
        else
        {
            if (currentScore > PlayerPrefs.GetInt("highscore", 0))
            {
                return true;
            }
        }
      
        return false;
  
    }

    public GameObject highScoreHudPanel;
    public Text highScoreHudText;
    public TweenPosition highScoreHudTween;
    public void setHighScoreLabel()
    {

       
        if (highScoreHudText != null)
        {
            highScoreHudText.text = currentScore.ToString();
        }
        
    }
    //-----------------------------------------------------------------------------------------------------


    private void FixedUpdate()
    {

        // UpdatingHUD
        if (GameGlobals.Instance.isInGamePlay() == true)
        {
            updateHud();
        }
       
    }

    // HUD -----------------------------------------------------------------------------------------------

    
    private float hudTimeEplased;
    private float hudScoreUpdateDelay = 0.05f;
    private bool highScoreOpenPanelTrig;
    private bool highScoreRecordedPanelTrig;

    private void updateHud()
    {

        //if (globals.isInGamePlay() == false) return;

        if (GameGlobals.Instance.controller.playerIsRunning == true && hudTimeEplased > hudScoreUpdateDelay)
        {
            hudTimeEplased = 0;
            increaseScore(1);
        }
        else
        {
            hudTimeEplased += Time.deltaTime;
        }

        // High Score ----
        if (scoresLock == false)
        {
            if (currentScore > (highScore - 450))
            {

                if (highScoreOpenPanelTrig == false)
                {
                    highScoreOpenPanelTrig = true;
                    if (highScoreHudPanel != null) highScoreHudPanel.SetActive(true);
                    if (highScoreHudTween != null) highScoreHudTween.PlayForward();
                }

                if (currentScore < highScore)
                {
                    lblHudHighScoreCountert.text = (highScore - currentScore).ToString();
                }


            }

            if (highScoreRecordedPanelTrig == false && currentScore > highScore)
            {
                highScoreRecordedPanelTrig = true;
                GameGlobals.Instance.audioController.playSound("PowerupLetterComplete", false);
                GameGlobals.Instance.isHighScoreRecorded = true;
               
                lblHudHighScoreCountert.text = "0";
                if (highScoreHudTween != null) highScoreHudTween.PlayReverse();
            }
        }

        //----------------

        lblHudCoins.text = currentCoins.ToString();
        lblHudScore.text = currentScore.ToString();

        lblScorePanelCollectedCoins.text = currentCoins.ToString();
        lblScorePanelCollectedScore.text = currentScore.ToString();

    }


    // ---------------------------------------------------------------------------------------------------

    public void updateHighScorePanelScore()
    {

        if (lblHighScorePanelScore != null)
        {

            lblHighScorePanelScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        }
     
    }

}

