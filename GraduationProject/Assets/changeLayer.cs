using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeLayer : MonoBehaviour {
    public GameObject Player1;
    public GameObject Player2;

    // Use this for initialization
    void Start () {
        Player1 = GameObject.Find("U_CharacterFront(Clone)1");
        Player2 = GameObject.Find("U_CharacterFront(Clone)2");

        if (Player1 != null)
        {
            if (Player2 != null)
            {
                gameObject.transform.parent = Player2.transform;
            }
            else
            {
                gameObject.transform.parent = Player1.transform;
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
        
        
    }
}
