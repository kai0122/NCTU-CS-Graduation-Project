using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMeshCollider : MonoBehaviour {
	public Mesh Mesh;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var collider = gameObject.GetComponent<MeshCollider>();
 		//collider.sharedMesh =  Mesh.GetNewMesh();
	}
}
