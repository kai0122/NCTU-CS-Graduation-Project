using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TrackObject: MonoBehaviour
{

    public enum ObjectGroup
    { 
        None = 0,
        CharacterObstacles = 1,
        Bales1 = 2,
        Bales3 = 3,
        Bales5 = 4,
        Barriers = 5,  
        Tunnels = 6,
		MovingObstaclesSingle = 7,
		MovingObstaclesTrail3 = 8,
		MovingObstaclesTrail5 = 9,
        PowerUps = 10,
        Pickables = 11
    }

    public enum ObjectType
    {
		None = 0,
		PointsSingle = 1,
		PointsLine = 2,
		PointsCurve = 3,
		PointsMovingLine = 4,
        PowerupVacuum = 5,
		PowerupBomb = 6,
		PickableLetter = 7,
		PickableFruit = 8,
		BarrierUp = 9,
		BarrierDown = 10,
		BarrierUpDown = 11,
		StaticCroco = 12,
		StaticMonke = 13,
		StaticParwet = 14,
		TunnelCenter = 15,
		TunnelRight = 16,
		TunnelLeft = 17,
		Tunnel2Path = 18,
		Ramp = 19,
		BaleYellow1 = 20,
		BaleYellow3 = 21,
		BaleYellow5 = 22,
		BaleGreen1 = 23,
		BaleGreen3 = 24,
		BaleGreen5 = 25,
		BaleGray1 = 26,
		BaleGray3 = 27,
		BaleGray5 = 28,
		MovingElephant = 29,
		MovingGazelle = 30,
		MovingGireffe = 31,
		MovingHippo = 32,
		MovingLion = 33,
		MovingRhino = 34,
		MovingTiger = 35,
		MovingZebra = 36,
		TrailYellow3 = 37,
		TrailGreen3 = 38,
		TrailGray3 = 39,
		TrailYellow5 = 40,
		TrailGreen5 = 41,
		TrailGray5 = 42,
        StaticGruba = 43,
        StaticWaba = 44
    }


	public string ID;

    [HideInInspector]
    public bool positioned;

    public ObjectGroup objectGroup;
    public ObjectType objectType;

    public bool placeHolder;
    public bool disableShuffle;

}
