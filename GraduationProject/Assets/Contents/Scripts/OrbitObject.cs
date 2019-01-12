using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class OrbitObject : MonoBehaviour {

   

    public GameObject[] orbitedObjects;
    private float[] pathPercents;

    //private float pathPercent;

    List<Vector3> circlePath = new List<Vector3>();

	void Start () {

        Transform[] wayPoints = new Transform[this.transform.childCount];
        pathPercents = new float[orbitedObjects.Length];

        // Assihn Random Path Percents
        float curPathPercent = 0;
        for (int i = 0; i < orbitedObjects.Length; i++)
        {
            pathPercents[i] = curPathPercent;
            curPathPercent += (1.0f / (float)orbitedObjects.Length);
        }


        for (int i = 0; i < this.transform.childCount; i++)
        {
            wayPoints[i] = this.transform.GetChild(i);
        }

        Transform[] sortedWayPoints = wayPoints.OrderBy(go => go.name).ToArray();

        for (int i = 0; i < sortedWayPoints.Length; i++)
        {
            circlePath.Add(new Vector3(sortedWayPoints[i].position.x, sortedWayPoints[i].position.y, sortedWayPoints[i].transform.position.z)); 
        }
   

        
	}


    void FixedUpdate()
    {

        if (orbitedObjects != null)
        {
            for (int i = 0; i < orbitedObjects.Length; i++)
            {
                if (orbitedObjects[i] != null)
                {
                    if (pathPercents[i] >= 1.0f) pathPercents[i] = 0;
                    ThunderTween.PutOnPath(orbitedObjects[i].transform, circlePath.ToArray(), pathPercents[i]);
                    orbitedObjects[i].transform.transform.LookAt(ThunderTween.PointOnPath(circlePath.ToArray(), pathPercents[i] + 0.05f));
                    pathPercents[i] += 0.0005f;
                }
           
            }
        }

      
    }
}
