using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovingCoin : MonoBehaviour
{


    public float speed = 1f;
    private Transform coin;
    private static CharacterController playerController;
    private static List<MovingCoin> activeCoins;

    private bool resumeMotion;
    public static float resumeMotionDistance;


    void Awake()
    {

        MovingCoin.activeCoins = new List<MovingCoin>();
        MovingCoin.resumeMotionDistance = 190f;

        MovingCoin.playerController = GameObject.Find("U_CharacterFront(Clone)1").GetComponent<CharacterController>();
        this.GetComponent<MovingCoin>().enabled = false;


    }

    public void doActive()
    {
        //Debug.Log("Coins Activated !!!");

        MovingCoin.activeCoins.Add(this);
        this.GetComponent<MovingCoin>().enabled = true;
        resumeMotion = false;

        Transform coinMesh = this.transform.Find("movingMesh");

        if (coinMesh != null)
        {
            coin = this.transform.Find("movingMesh").transform;
            float currentZ = base.transform.position.z;
            Vector3 playerPos = MovingCoin.playerController.transform.position;
            coin.localPosition = new Vector3(0f, 0f, (currentZ - playerPos.z) * this.speed);
        }


    }

    public void doDeactive()
    {
        MovingCoin.activeCoins.Remove(this);
        this.GetComponent<MovingCoin>().enabled = false;
    }

    public static void ActivateResumeMotion()
    {
        foreach (MovingCoin activeCoin in MovingCoin.activeCoins)
        {
            GameObject movingMesh = activeCoin.transform.Find("movingMesh").gameObject;
            if (movingMesh != null)
            {
                if (movingMesh.GetComponent<Collider>().bounds.min.z - MovingCoin.playerController.transform.position.z >= MovingCoin.resumeMotionDistance)
                {
                    continue;
                }
            }

            activeCoin.resumeMotion = true;
        }
    }

    public void Update()
    {

        if (resumeMotion == false)
        {
            float currentZ = base.transform.position.z;
            Vector3 playerPos = MovingCoin.playerController.transform.position;

            float calcZ = (currentZ - playerPos.z) * this.speed;

            Vector3 currentPos = new Vector3(0f, 0f, calcZ);
            this.coin.position = base.transform.TransformPoint(currentPos);

        }
        else if (resumeMotion == true)
        {

            this.coin.position = this.coin.position - (((Vector3.forward * Time.deltaTime) * GameGlobals.Instance.controller.currentLevelSpeed) * this.speed);
        }



    }



}

