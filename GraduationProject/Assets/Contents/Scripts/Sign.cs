using System;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{

    private static List<Sign> activeSignBoards = new List<Sign>();

    [HideInInspector]
    public List<GameObject> signs = new List<GameObject>();

    private void Awake()
    {

        foreach (Transform sign in this.transform)
        {
            SpriteRenderer signSiprite = sign.GetComponent<SpriteRenderer>();
            if (signSiprite != null)
            {
                signs.Add(sign.gameObject);
            }
        }

        activeSignBoards.Add(this);
        //ShuffleSings();
    
    }


    public static void ShuffleSings()
    {

        foreach (Sign signBoard in activeSignBoards)
        {
            foreach (GameObject sign in signBoard.signs)
            {
                //sign.gameObject.SetActive(false);
            }

            signBoard.signs[UnityEngine.Random.Range(0, signBoard.signs.Count)].gameObject.SetActive(true);

        }


    }

}

