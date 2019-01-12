using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MovingObstacle : MonoBehaviour
{

    public enum movingObstacleTypes
    {
        Trail = 0,
        Single = 1

    }


    private TrackObject trackObject;
    public float speed = 1f;
    public bool lockSpeed;
    public movingObstacleTypes type;
    private Transform train;
    public float trainCount = 3f;
    private static CharacterController playerController;
    private static List<MovingObstacle> activeMovingObstacles;
    private bool resumeMotion;
    public static float resumeMotionDistance;
    private AudioClip trianPassClip;
    private AudioSource trainPassSource;
    private bool isInitialized;

    private bool isPaused;

    private Animator obstacleAnimator;

    private AudioSource audioSrc;

    void Awake()
    {


    
        trackObject = transform.GetComponent<TrackObject>();

        if (trackObject != null)
        {

            switch (trackObject.objectGroup)
            {
                case TrackObject.ObjectGroup.MovingObstaclesSingle:

                    break;
                case TrackObject.ObjectGroup.MovingObstaclesTrail3:

                    break;
            }

        }

        MovingObstacle.activeMovingObstacles = new List<MovingObstacle>();
        MovingObstacle.resumeMotionDistance = 190;

        MovingObstacle.playerController = GameObject.Find("U_CharacterFront(Clone)1").GetComponent<CharacterController>();

        Transform movingMesh = transform.Find("movingMesh");

        if (movingMesh != null)
        {
            audioSrc = movingMesh.GetComponent<AudioSource>();

            train = movingMesh.transform;
            train.localPosition = -Vector3.up * 200f;

            this.GetComponent<MovingObstacle>().enabled = false;

            Transform obstacle = transform.Find("movingMesh/obstacle");
            if (obstacle != null)
            {
                obstacleAnimator = obstacle.GetComponent<Animator>();
                if (obstacleAnimator != null)
                {
                    obstacleAnimator.enabled = false;
                }
            }

        }



    }

    public void handlePause(bool isPaused)
    {

        switch (isPaused)
        {
            case false:

                playAnimation("Run");

                if (PlayerPrefs.GetInt("audio", 1) == 1)
                {
                    audioSrc.Play();
                }
                else if (PlayerPrefs.GetInt("audio", 1) == 0)
                {
                    audioSrc.Stop();
                }

                break;
            case true:
                playAnimation("Idle_Look");
                audioSrc.Stop();
                break;
        }
    }


    public void doActive()
    {

        //Debug.Log("Trail Activated !!! Trail:" + this.GetComponent<MovingObstacle>().trackObject.objectType.ToString() + " At:" + transform.parent.transform.parent.name);

        if (lockSpeed == false)
        {
            this.speed = UnityEngine.Random.Range(1.0f, 2.0f);
        }

        MovingObstacle.activeMovingObstacles.Add(this);
        this.GetComponent<MovingObstacle>().enabled = true;
        resumeMotion = false;

        float currentZ = base.transform.position.z;
        Vector3 playerPos = MovingObstacle.playerController.transform.position;
        this.train.localPosition = new Vector3(0f, 0f, (currentZ - playerPos.z) * this.speed);


        if (obstacleAnimator != null)
        {
            obstacleAnimator.enabled = true;
            playAnimation("Run");
        }


        if (audioSrc != null)
        {

            if (PlayerPrefs.GetInt("audio", 1) == 1)
            {
                audioSrc.volume = 0;
                audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
                audioSrc.timeSamples = UnityEngine.Random.Range(0, audioSrc.timeSamples);
                audioSrc.Play();
            }

        }

    }

    public void doDeactive()
    {

        if (obstacleAnimator != null)
        {
            obstacleAnimator.enabled = false;
        }

        MovingObstacle.activeMovingObstacles.Remove(this);
        this.GetComponent<MovingObstacle>().enabled = false;

    }



    public void Update()
    {

        if (resumeMotion == false)
        {
            float currentZ = base.transform.position.z;
            Vector3 playerPos = MovingObstacle.playerController.transform.position;


            float calcZ = (currentZ - playerPos.z) * this.speed;
            if (audioSrc != null)
            {
                float volume = 1.0f - Mathf.Abs(calcZ / 100.0f);
                audioSrc.volume = volume;
            }


            Vector3 currentPos = new Vector3(0f, 0f, calcZ);
            this.train.position = base.transform.TransformPoint(currentPos);

        }
        else if (resumeMotion == true)
        {

            this.train.position = this.train.position - (((Vector3.forward * Time.deltaTime) * GameGlobals.Instance.controller.currentLevelSpeed) * this.speed);

            if (audioSrc != null)
            {
                if (audioSrc.volume > 0)
                {
                    audioSrc.volume = audioSrc.volume - 0.003f;
                }
            }


        }

    }

    private void playAnimation(string anim)
    {
        switch (type)
        {
            case movingObstacleTypes.Trail:


                break;
            case movingObstacleTypes.Single:

                Transform obstacle = transform.Find("movingMesh/obstacle");
                if (obstacle != null)
                {
                    obstacle.GetComponent<Animator>().Play(anim);
                }

                break;

        }


    }

    // STATIC ALL

    public static void ActivateResumeMotion()
    {
        foreach (MovingObstacle activeTrain in MovingObstacle.activeMovingObstacles)
        {
            if (activeTrain.transform.Find("movingMesh").GetComponent<Collider>().bounds.min.z - MovingObstacle.playerController.transform.position.z >= MovingObstacle.resumeMotionDistance)
            {
                continue;
            }
            activeTrain.resumeMotion = true;
        }
    }

    public static void handlePauseAll(bool isPaused)
    {

        foreach (MovingObstacle activeTrain in MovingObstacle.activeMovingObstacles)
        {
            activeTrain.handlePause(isPaused);
        }

    }

}

