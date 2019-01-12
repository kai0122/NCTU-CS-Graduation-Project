using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinLine : MonoBehaviour
{
    public float length = 100f;
    public float coinSpacing = 15f;
    private int coinCount;

    public enum CoinLineRefraction
    { 
        None = 0,
        LeftStraight = 1,
        RightStraight = 2,
        LeftCurved = 3,
        RightCurved = 4
    }

    public CoinLineRefraction coinLineRefraction;

    private List<Transform> coins = new List<Transform>();
    private Vector3[] arc = null;

    private int lastAddedIndex;


    private void Awake()
    {
        lastAddedIndex = 0;
    }


    public void doActive()
    {

        if (GameGlobals.Instance.trackGenerator == null) return;
        if (arc == null) arc = new Vector3[4];

        switch (coinLineRefraction)
        {
            case CoinLineRefraction.None:

                arc[0] = new Vector3(0, 0, 0);
                arc[1] = new Vector3(0, 0, (length / 3.0f) * 1.0f);
                arc[2] = new Vector3(0, 0, (length / 3.0f) * 2.0f);
                arc[3] = new Vector3(0, 0, (length / 3.0f) * 3.0f);

                break;
            case CoinLineRefraction.LeftStraight:

                arc[0] = new Vector3(0, 0, 0);
                arc[1] = new Vector3(-0.6f, 0, (length / 3.0f) * 1.0f);
                arc[2] = new Vector3(-6.0f, 0, (length / 3.0f) * 2.0f);
                arc[3] = new Vector3(-6.6f, 0, (length / 3.0f) * 3.0f);

                break;
            case CoinLineRefraction.RightStraight:

                arc[0] = new Vector3(0, 0, 0);
                arc[1] = new Vector3(0.6f, 0, (length / 3.0f) * 1.0f);
                arc[2] = new Vector3(6.0f, 0, (length / 3.0f) * 2.0f);
                arc[3] = new Vector3(6.6f, 0, (length / 3.0f) * 3.0f);

                break;
            case CoinLineRefraction.LeftCurved:

                arc[0] = new Vector3(0, 0, 0);
                arc[1] = new Vector3(-6.0f, 0, (length / 3.0f) * 1.0f);
                arc[2] = new Vector3(-6.0f, 0, (length / 3.0f) * 2.0f);
                arc[3] = new Vector3(0, 0, (length / 3.0f) * 3.0f);

                break;

            case CoinLineRefraction.RightCurved:

                arc[0] = new Vector3(0, 0, 0);
                arc[1] = new Vector3(6.0f, 0, (length / 3.0f) * 1.0f);
                arc[2] = new Vector3(6.0f, 0, (length / 3.0f) * 2.0f);
                arc[3] = new Vector3(0, 0, (length / 3.0f) * 3.0f);

                break;

        }

        // Reset
        lastAddedIndex = 0;
        coinCount = 0;

        coinCount = (int)Mathf.Round(length / coinSpacing);
        float perIncrement = 1.0f / coinCount;
        float currentPercent = perIncrement;

        // First Coint
        insetCoin(new Vector3(0, 0, 0));
        
        // Path Coins
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

            if (coinLineRefraction == CoinLineRefraction.None)
            {
                coin.GetComponent<Coin>().audioPitch = 0;
            }
            else
            {
                float pitch = 1.0f + (0.4f / coinCount * lastAddedIndex);
                coin.GetComponent<Coin>().audioPitch = pitch;
            }
           

            coin.gameObject.SetActive(true);
            coins.Insert(lastAddedIndex, coin.transform);

            lastAddedIndex++;
        }


    }

    public void doDeactive()
    {

        arc = null;
        lastAddedIndex = 0;
        coinCount = 0;

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

    public void OnDrawGizmos()
    {

       Vector3[] arcGizmo = new Vector3[4];

       float objectX = transform.position.x;
       float objectY = transform.position.y;
       float objectZ = transform.position.z;
   

       switch (coinLineRefraction)
       {
           case CoinLineRefraction.None:

               arcGizmo[0] = new Vector3(objectX, objectY, objectZ);
               arcGizmo[1] = new Vector3(objectX, objectY, objectZ + (length / 3.0f) * 1.0f);
               arcGizmo[2] = new Vector3(objectX, objectY, objectZ + (length / 3.0f) * 2.0f);
               arcGizmo[3] = new Vector3(objectX, objectY, objectZ + (length / 3.0f) * 3.0f);

               break;
           case CoinLineRefraction.LeftStraight:

               arcGizmo[0] = new Vector3(objectX, objectY, objectZ);
               arcGizmo[1] = new Vector3(objectX  + -0.6f, objectY, objectZ + (length / 3.0f) * 1.0f);
               arcGizmo[2] = new Vector3(objectX  + -6.0f, objectY, objectZ + (length / 3.0f) * 2.0f);
               arcGizmo[3] = new Vector3(objectX  + -6.06f, objectY, objectZ + (length / 3.0f) * 3.0f);

               break;
           case CoinLineRefraction.RightStraight:

               arcGizmo[0] = new Vector3(objectX, objectY, objectZ);
               arcGizmo[1] = new Vector3(objectX + 0.6f, objectY, objectZ + (length / 3.0f) * 1.0f);
               arcGizmo[2] = new Vector3(objectX + 6.0f, objectY, objectZ + (length / 3.0f) * 2.0f);
               arcGizmo[3] = new Vector3(objectX + 6.06f, objectY, objectZ + (length / 3.0f) * 3.0f);

               break;
           case CoinLineRefraction.LeftCurved:

               arcGizmo[0] = new Vector3(objectX, objectY, objectZ);
               arcGizmo[1] = new Vector3(objectX + -6.0f, objectY, objectZ + (length / 3.0f) * 1.0f);
               arcGizmo[2] = new Vector3(objectX + -6.0f, objectY, objectZ + (length / 3.0f) * 2.0f);
               arcGizmo[3] = new Vector3(objectX, objectY, objectZ + (length / 3.0f) * 3.0f);

               break;

           case CoinLineRefraction.RightCurved:

               arcGizmo[0] = new Vector3(objectX, objectY, objectZ);
               arcGizmo[1] = new Vector3(objectX + 6.0f, objectY, objectZ + (length / 3.0f) * 1.0f);
               arcGizmo[2] = new Vector3(objectX + 6.0f, objectY, objectZ + (length / 3.0f) * 2.0f);
               arcGizmo[3] = new Vector3(objectX, objectY, objectZ + (length / 3.0f) * 3.0f);

               break;

       }


        ThunderTween.DrawPath(arcGizmo,Color.black);
        Gizmos.color = Color.yellow;

        int coinCount = (int)Mathf.Round(length / coinSpacing);
        float perIncrement = 1.0f / coinCount;
        float currentPercent = perIncrement;

        // First Coint
        Gizmos.DrawSphere(ThunderTween.PointOnPath(arcGizmo, 0), 1f);

        // Path Coins
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 coinPos = ThunderTween.PointOnPath(arcGizmo, currentPercent);
            currentPercent += perIncrement;
            Gizmos.DrawSphere(coinPos, 1f);
        }

      
    }
}