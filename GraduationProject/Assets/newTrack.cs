using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newTrack : MonoBehaviour {

	public GameObject Player1;
	private int TrackPositionZ;
	public GameObject[] Tracklist;
	public GameObject firstTrack;
	public GameObject secondTrack;
	public GameObject thirdTrack;
    public int randNum;

	// Use this for initialization
	void Start () {
		TrackPositionZ = 500;	//	current last position z
        randNum = 3;

    }

	// Update is called once per frame
	void Update () {
        //Player1 = GameObject.Find("U_CharacterFront(Clone)1");
        if (Player1 == null) return;
        if (Player1.transform.position.z > TrackPositionZ - 200 && Player1.transform.position.z > TrackPositionZ - 500){
			Destroy(firstTrack);
			firstTrack = secondTrack;
			secondTrack = thirdTrack;

            if (randNum >= Tracklist.Length) randNum = 0;
            
			thirdTrack = Instantiate(Tracklist[randNum]);
			thirdTrack.transform.position = new Vector3(0, 0, TrackPositionZ + 250);
			TrackPositionZ = TrackPositionZ + 250;
            randNum = randNum + 1;
        }
	}
}
