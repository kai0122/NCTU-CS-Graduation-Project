using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinCurve : MonoBehaviour
{
    public float coinSpacing = 7;
    public GameObject offsetObject;
    private int coinCount;

    private List<Transform> coins = new List<Transform>();
    
    private Vector3[] arc = null;




    private int lastAddedIndex;
    private bool isActive;

   
    public void Awake()
    {
        lastAddedIndex = 0;
        isActive = false;
    }


    public void doActive()
    {
        createArc();
        isActive = true;
    }

    private void FixedUpdate()
    {
        if (isActive == true)
        {
            updateArc();
        }
    }

    public void doDeactive()
    {
 
        isActive = false;
        arc = null;
        lastAddedIndex = 0;

        foreach (Transform coin in coins)
        {
            coin.GetComponent<TrackObject>().positioned = false;
            coin.transform.parent = GameGlobals.Instance.trackGenerator.trackObjectsRoot.transform;
            coin.transform.position = GameGlobals.Instance.trackGenerator.rootPos;
            coin.transform.Find("trigger").GetComponent<Collider>().enabled = true;

            GameObject coinMesh = coin.transform.Find("coinMesh").gameObject;
            if (coinMesh != null)
            {
                Rotator rotator = coinMesh.GetComponent<Rotator>();
                if (rotator != null)
                {
                    rotator.enabled = false;
                }
            }


            coin.gameObject.SetActive(false);
        }

        coins.Clear();
    
    }


    private void createArc()
    {
       
        if (arc == null) arc = new Vector3[3];

        if (GameGlobals.Instance.controller.currentLevelSpeed <= 0) GameGlobals.Instance.controller.currentLevelSpeed = GameGlobals.Instance.controller.Accelerate(Time.time - Time.time);

        lastAddedIndex = 0;
        coinCount = 0;

        float jumpHeight = GameGlobals.Instance.controller.jumpHeight;
        float jumpLenght = GameGlobals.Instance.controller.JumpLength();

        Vector3 endPos = transform.forward * jumpLenght;

        float endZ = endPos.z + 2.0f;

        float startZ = 0;

        if (offsetObject != null)
        {
            startZ = offsetObject.transform.localPosition.z - (endZ / 2.0f);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, startZ);
        }

        

        arc[0] = new Vector3(0, 0, 0);
        arc[1] = new Vector3(0, jumpHeight,  endZ / 2.0f);
        arc[2] = new Vector3(0, 0, endZ);


        coinCount = (int)Mathf.Round(Mathf.Abs(arc[2].z) / coinSpacing);
        float perIncrement = 1.0f / coinCount;

        float currentPercent = perIncrement;

        // Start Coin
        insetCoin(new Vector3(0, 0, 0));
        
 
        // Curved Coins
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 coinPos = ThunderTween.PointOnPath(arc, currentPercent);
            insetCoin(coinPos);
            currentPercent += perIncrement;
        }

    }


    private void insetCoin(Vector3 coinPos)
    {

        GameObject coin = GameGlobals.Instance.trackGenerator.getRandomTrackObjectByType(TrackObject.ObjectType.PointsSingle);
        GameObject coinMesh = coin.transform.Find("coinMesh").gameObject;

        if (coin != null)
        {

            coin.GetComponent<TrackObject>().positioned = true;
            coin.transform.parent = transform;
            coin.transform.localPosition = coinPos;


            if (coinMesh != null)
            {
                coinMesh.transform.rotation = new Quaternion(0, 0, 0, 0);
                coinMesh.transform.Rotate(0, coins.Count * 35f, 0);

                Rotator rotator = coinMesh.GetComponent<Rotator>();
                if (rotator != null)
                {
                    rotator.enabled = true;
                }

            }

            float pitch = 1.0f + (0.4f / coinCount * lastAddedIndex);
            coin.GetComponent<Coin>().audioPitch = pitch;

            coin.gameObject.SetActive(true);
            coins.Insert(lastAddedIndex, coin.transform);

            lastAddedIndex++;
        }


    }

    private int refreshTime;
    private void updateArc()
    {

        if (coins == null) return;
        if (coins.Count <= 0) return;

        if (refreshTime >= 2000)
        {
           
            float jumpHeight = GameGlobals.Instance.controller.jumpHeight;
            float jumpLenght = GameGlobals.Instance.controller.JumpLength();

            Vector3 endPos = transform.forward * jumpLenght;
            float endZ = endPos.z + 2.0f;

            float startZ = 0;

            if (offsetObject != null)
            {
                startZ = offsetObject.transform.localPosition.z - (endZ / 2.0f);
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, startZ);
            }


            arc[0] = new Vector3(0, 0, 0);
            arc[1] = new Vector3(0, jumpHeight, endZ / 2.0f);
            arc[2] = new Vector3(0, 0, endZ);

            int coinCount = (int)Mathf.Round(Mathf.Abs(arc[2].z) / coinSpacing);
            float perIncrement = 1.0f / coinCount;
            float currentPercent = perIncrement;

            for (int i = 1; i < coins.Count; i++)
            {
                coins[i].transform.localPosition = ThunderTween.PointOnPath(arc, currentPercent);
                currentPercent += perIncrement;
            }

            refreshTime = 0;

        }
        else
        {
            refreshTime++;
        }
 

        

    }

 



}