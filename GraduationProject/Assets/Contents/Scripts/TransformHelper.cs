using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelper : MonoBehaviour {


    public Vector3 position;

	
    public void setPosition(Vector3 pos)
    {
        transform.position = pos;
    }

}
