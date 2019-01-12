using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

public class GameTools
{
	// Add a new menu item under an existing menu

    [MenuItem("Game Tools/Reset Game Preferences")]
    private static void resetPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("tutorial", 1);
 

    }


	private static List<Component> FindAllComponents(System.Type typ, Transform trans)
	{
		List<Component> comps = new List<Component>();
		comps.AddRange((Component[])trans.GetComponents(typ));
		foreach (Transform child in trans)
		{
			comps.AddRange(FindAllComponents(typ, child));
		}
		return comps;
	}



	[MenuItem("Game Tools/Sync Selected Track Obkjects")]
	private static void synceTrackObjects()
	{

		if (Selection.activeGameObject == null)
		{
			Debug.Log("Please select a track");
			return;
		}

		if (Selection.activeGameObject.transform.Find("trackObjects") == null)
		{
			Debug.Log("Track object does not contains trackObjects root");
			return;
		}

		TrackGenerator	trackGen = GameObject.Find("PlayMaker").GetComponent<TrackGenerator>();

		if (trackGen == null)
		{
			Debug.Log("Track generator gameobject not found");
			return;
		}


		foreach (GameObject currentObject in trackGen.trackObjects) 
		{

			TrackObject tObject = currentObject.GetComponent<TrackObject>();
			if (tObject != null)
			{

				switch (tObject.ID) 
				{
					case "gray1":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales1;
						tObject.objectType = TrackObject.ObjectType.BaleGray1;
						break;
					case "gray3":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales3;
						tObject.objectType = TrackObject.ObjectType.BaleGray3;
						break;
					case "gray5":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales5;
						tObject.objectType = TrackObject.ObjectType.BaleGray5;
						break;
					case "green1":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales1;
						tObject.objectType = TrackObject.ObjectType.BaleGreen1;
						break;
					case "green3":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales3;
						tObject.objectType = TrackObject.ObjectType.BaleGreen3;
						break;
					case "green5":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales5;
						tObject.objectType = TrackObject.ObjectType.BaleGreen5;
						break;
					case "yellow1":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales1;
						tObject.objectType = TrackObject.ObjectType.BaleYellow1;
						break;
					case "yellow3":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales3;
						tObject.objectType = TrackObject.ObjectType.BaleYellow3;
						break;
					case "yellow5":
						tObject.objectGroup = TrackObject.ObjectGroup.Bales5;
						tObject.objectType = TrackObject.ObjectType.BaleYellow5;
						break;
					case "ramp":
						tObject.objectGroup = TrackObject.ObjectGroup.None;
						tObject.objectType = TrackObject.ObjectType.Ramp;
						break;
					case "barrierdown":
						tObject.objectGroup = TrackObject.ObjectGroup.Barriers;
						tObject.objectType = TrackObject.ObjectType.BarrierDown;
						break;
					case "barrierdownup":
						tObject.objectGroup = TrackObject.ObjectGroup.Barriers;
						tObject.objectType = TrackObject.ObjectType.BarrierUpDown;
						break;
					case "barrierup":
						tObject.objectGroup = TrackObject.ObjectGroup.Barriers;
						tObject.objectType = TrackObject.ObjectType.BarrierUp;
						break;
					case "currency":
						tObject.objectGroup = TrackObject.ObjectGroup.None;
						tObject.objectType = TrackObject.ObjectType.PointsSingle;
						break;
					case "pointscurve":
						tObject.objectGroup = TrackObject.ObjectGroup.None;
						tObject.objectType = TrackObject.ObjectType.PointsCurve;
						break;
					case "pointsline":
						tObject.objectGroup = TrackObject.ObjectGroup.None;
						tObject.objectType = TrackObject.ObjectType.PointsLine;
						break;
					case "pointsmovingline":
						tObject.objectGroup = TrackObject.ObjectGroup.None;
						tObject.objectType = TrackObject.ObjectType.PointsMovingLine;
						break;
					case "trailgray3":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesTrail3;
						tObject.objectType = TrackObject.ObjectType.TrailGray3;
						break;
					case "trailgray5":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesTrail5;
						tObject.objectType = TrackObject.ObjectType.TrailGray5;
						break;
					case "trailgreen3":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesTrail3;
						tObject.objectType = TrackObject.ObjectType.TrailGreen3;
						break;
					case "trailgreen5":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesTrail5;
						tObject.objectType = TrackObject.ObjectType.TrailGreen5;
						break;
					case "trailyellow3":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesTrail3;
						tObject.objectType = TrackObject.ObjectType.TrailYellow3;
						break;
					case "trailyellow5":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesTrail5;
						tObject.objectType = TrackObject.ObjectType.TrailYellow5;
						break;
					case "croco":
						tObject.objectGroup = TrackObject.ObjectGroup.CharacterObstacles;
						tObject.objectType = TrackObject.ObjectType.StaticCroco;
						break;
					case "monke":
						tObject.objectGroup = TrackObject.ObjectGroup.CharacterObstacles;
						tObject.objectType = TrackObject.ObjectType.StaticMonke;
						break;
					case "tunnel2path":
						tObject.objectGroup = TrackObject.ObjectGroup.Tunnels;
						tObject.objectType = TrackObject.ObjectType.Tunnel2Path;
						break;
					case "tunnelcenter":
						tObject.objectGroup = TrackObject.ObjectGroup.Tunnels;
						tObject.objectType = TrackObject.ObjectType.TunnelCenter;
						break;
					case "tunnelleft":
						tObject.objectGroup = TrackObject.ObjectGroup.Tunnels;
						tObject.objectType = TrackObject.ObjectType.TunnelLeft;
						break;
					case "tunnelright":
						tObject.objectGroup = TrackObject.ObjectGroup.Tunnels;
						tObject.objectType = TrackObject.ObjectType.TunnelRight;
						break;
					case "elephant":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingElephant;
						break;
					case "gazelle":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingGazelle;
						break;
					case "gireffe":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingGireffe;
						break;
					case "hippo":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingHippo;
						break;
					case "lion":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingLion;
						break;
					case "rhino":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingRhino;
						break;
					case "tiger":
						tObject.objectGroup = TrackObject.ObjectGroup.MovingObstaclesSingle;
						tObject.objectType = TrackObject.ObjectType.MovingTiger;
						break;

				}

				// Syncing Track Object
				//changeElementTypeInScene(trackGen,tObject.ID,tObject.objectGroup,tObject.objectType);

				Transform trackObjectsRoot = Selection.activeGameObject.transform.Find("trackObjects");
				
				if (trackObjectsRoot != null)
				{
					
					List<GameObject> trackObjectsClone = new List<GameObject>();
					
					foreach (Transform item in trackObjectsRoot.transform) 
					{
						trackObjectsClone.Add(item.gameObject);
					}
					
					
					foreach (GameObject existTrackObject in trackObjectsClone) 
					{
						TrackObject tObj = existTrackObject.GetComponent<TrackObject>();
						if (tObj != null)
						{
							if (tObj.ID.Equals(tObject.ID))
							{
								// Assign Object Type
								tObj.objectGroup = tObject.objectGroup;
								tObj.objectType = tObject.objectType;
								
								GameObject newPrefab = getGameObjectPrefab(tObj.ID);
								if (newPrefab != null)
								{
									
									GameObject newTrackObject = (GameObject)Editor.Instantiate(newPrefab,Vector3.zero,Quaternion.identity);
									newTrackObject.transform.parent = trackObjectsRoot.transform;
									
									newTrackObject.GetComponent<TrackObject>().placeHolder = tObj.placeHolder;
									newTrackObject.GetComponent<TrackObject>().disableShuffle = tObj.disableShuffle;
									
									newTrackObject.transform.position = existTrackObject.transform.position;
									newTrackObject.transform.localPosition = existTrackObject.transform.localPosition;
									
									
									// Speacial Components
									
									if (existTrackObject.GetComponent<MovingObstacle>() != null) copyComponentValues(existTrackObject.GetComponent<MovingObstacle>(),newTrackObject.GetComponent<MovingObstacle>());
									if (existTrackObject.GetComponent<CoinLine>() != null) copyComponentValues(existTrackObject.GetComponent<CoinLine>(),newTrackObject.GetComponent<CoinLine>());
									if (existTrackObject.GetComponent<CoinCurve>() != null) copyComponentValues(existTrackObject.GetComponent<CoinCurve>(),newTrackObject.GetComponent<CoinCurve>());
									if (existTrackObject.GetComponent<MovingCoin>() != null) copyComponentValues(existTrackObject.GetComponent<MovingCoin>(),newTrackObject.GetComponent<MovingCoin>());
									
									Editor.DestroyImmediate(existTrackObject);
								}

							}
						}
					}
					
				}

			}
		
		}

	}


	static GameObject getGameObjectPrefab(string ID)
	{

		TrackGenerator trackGen =  GameObject.Find("PlayMaker").GetComponent<TrackGenerator>();
		
		foreach (GameObject item in trackGen.trackObjects) 
		{
			if (item.GetComponent<TrackObject>().ID.Equals(ID))
			{
				return item;
			}
		}

		return null;
	}

	static void copyComponentValues(Component from,Component to)
	{

		System.Reflection.FieldInfo[] fields = from.GetType().GetFields(); 
		
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(to, field.GetValue(from));
		}
		
	}

	
}