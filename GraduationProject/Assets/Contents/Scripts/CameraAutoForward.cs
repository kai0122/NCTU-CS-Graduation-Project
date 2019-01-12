using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoForward : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	


	void FixedUpdate () {

        Vector3 nextPos = new Vector3(0, 0, 0.03f);
        transform.position += nextPos;

    }

}
