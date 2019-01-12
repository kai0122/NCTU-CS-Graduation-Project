using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class TrackGenerator : MonoBehaviour
{

    public List<GameObject> startTracks;
    public List<GameObject> tracks;
    public List<GameObject> trackObjects;
    public List<GameObject> trackInjections;

    public GameObject tutorialTrack;
    private GameObject tutorialTrackInstance;

    [HideInInspector]
    public List<GameObject> startTrackPool = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> trackPool = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> injectedTrackPool = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> trackObjectPool = new List<GameObject>();

    //private List<GameObject> currentSelectedTracks = new List<GameObject>();

    private bool startTrackPlaced;
    private bool tutorialTrackPlaced;
    private float trackCurrentZ;
    private float trackRenderDistance = 300;

    public GameObject fixTrack;
    public GameObject testTrack;
    private GameObject player;

    [HideInInspector]
    public GameObject tracksRoot, trackObjectsRoot;
    [HideInInspector]
    public Vector3 rootPos = new Vector3(0, 0, -5000);

    
    public bool testMode;


    private void Awake()
    {

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("tutorial", 1);

        player = GameObject.Find("Player");
        tracksRoot = GameObject.Find("TracksRoot");
        trackObjectsRoot = GameObject.Find("TrackObjectsRoot");

  
   
        // Clonning Tutorial Track
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            tutorialTrackInstance = (GameObject)GameObject.Instantiate(tutorialTrack, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            tutorialTrackInstance.transform.parent = tracksRoot.transform;
            tutorialTrackInstance.transform.position = rootPos;
            tutorialTrackInstance.gameObject.SetActive(false);

            disposeEditorTrackItems(tutorialTrackInstance);

        }

        // Colling Start Tracks
        foreach (GameObject track in startTracks)
        {
            GameObject cloneTrack = (GameObject)GameObject.Instantiate(track, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            cloneTrack.transform.parent = tracksRoot.transform;
            cloneTrack.transform.position = rootPos;
            cloneTrack.gameObject.SetActive(false);

            disposeEditorTrackItems(cloneTrack);

            startTrackPool.Add(cloneTrack);
        }

        // Clonnig Regular Tracks
        foreach (GameObject track in tracks)
        {

            if (track != null)
            {
                GameObject cloneTrack = (GameObject)GameObject.Instantiate(track, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                cloneTrack.transform.parent = tracksRoot.transform;
                cloneTrack.transform.position = rootPos;
                cloneTrack.gameObject.SetActive(false);

                disposeEditorTrackItems(cloneTrack);

                trackPool.Add(cloneTrack);
            }
           
        }

        // Clonig Track Objects
        foreach (GameObject trackObject in trackObjects)
        {

            for (int i = 0; i < getMaxCountPerTrackObject(trackObject.GetComponent<TrackObject>()); i++)
            {
                GameObject cloneTrackObject = (GameObject)GameObject.Instantiate(trackObject, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                cloneTrackObject.transform.parent = trackObjectsRoot.transform;
                cloneTrackObject.transform.position = rootPos;
                cloneTrackObject.GetComponent<TrackObject>().placeHolder = false;
                cloneTrackObject.gameObject.SetActive(false);
                trackObjectPool.Add(cloneTrackObject);
            }

        }

        Reset();

    }


    public void Reset()
    {

        startTrackPlaced = false;

        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            tutorialTrackPlaced = false;
        }
        else
        {
            tutorialTrackPlaced = true;
        }

        trackCurrentZ = 0;
        disposeAllTracks();

    }

    private void disposeEditorTrackItems(GameObject track)
    {
        Transform currentTrackObjectsRoot = track.transform.Find("trackObjects");
        if (currentTrackObjectsRoot != null)
        {
            foreach (Transform trackItem in currentTrackObjectsRoot.transform)
            {
                foreach (Transform child in trackItem) Destroy(child.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        StartCoroutine(GenerateTracks());
    }



    public IEnumerator GenerateTracks()
    {

        float forwardTargetDistance = player.transform.position.z + trackRenderDistance;


        if (trackCurrentZ < forwardTargetDistance)
        {
            StartCoroutine(clearTracksAndObjectsBehind());
        }

        while (trackCurrentZ < forwardTargetDistance)
        {

            // Getting an empty track
            GameObject currentTrack = null;
            if (testMode == false)
            {
                currentTrack = getRandomTrack();
            }
            else if (testMode == true)
            {
                currentTrack = getTestTrack();
            }


            // Populating selected Track
            StartCoroutine(populateTrack(currentTrack));

            currentTrack.transform.position = (Vector3)(Vector3.forward * trackCurrentZ);
            trackCurrentZ += currentTrack.GetComponent<Track>().zSize;

        }

        yield break;

    }



    private GameObject getRandomTrack()
    {

        int regularTrackCount = 0;
        float playerZ = player.transform.position.z;

        if (playerZ < 0) playerZ = 0;

        List<GameObject> possibleTracks = new List<GameObject>();

        if (tutorialTrackPlaced == false && tutorialTrackInstance != null)
        {
            tutorialTrackPlaced = true;
            Track item = tutorialTrackInstance.GetComponent<Track>();
            possibleTracks.Add(item.gameObject);

        }
        else if (tutorialTrackPlaced == true)
        {

            switch (startTrackPlaced)
            {
                case false:

                    startTrackPlaced = true;

                    for (int i = 0; i < startTrackPool.Count; i++)
                    {
                        Track item = startTrackPool[i].GetComponent<Track>();
                        possibleTracks.Add(item.gameObject);
                    }

                    break;

                case true:

                    for (int i = 0; i < trackPool.Count; i++)
                    {
                        Track item = trackPool[i].GetComponent<Track>();

                        // Adding Regular Tracks
                        if (item.positioned == false && item.CanSpawnAtEveryWhere == false && item.CanSpawnAtMaximum == false)
                        {
                            if ((item.spawnStartPos <= playerZ) && (playerZ < item.spawnEndPos))
                            {
                                possibleTracks.Add(item.gameObject);
                                regularTrackCount++;
                            }
                        }

                        // Adding Anywhere Tracks
                        if (item.positioned == false && item.CanSpawnAtEveryWhere == true)
                        {
                            possibleTracks.Add(item.gameObject);
                        }

                        // Adding Maximum Tracks
                        if (item.positioned == false && regularTrackCount <= 0 && item.CanSpawnAtMaximum == true)
                        {
                            possibleTracks.Add(item.gameObject);
                        }

                    }

                    break;

            }

        
        }
       

        if (possibleTracks.Count > 0)
        {

            GameObject selectedTrack = possibleTracks[Random.Range(0, possibleTracks.Count)];
            selectedTrack.GetComponent<Track>().positioned = true;
            selectedTrack.gameObject.SetActive(true);

            return selectedTrack;
        }
        else
        {
            GameObject createdFixTrack = (GameObject)GameObject.Instantiate(fixTrack, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            createdFixTrack.transform.parent = tracksRoot.transform;

            return createdFixTrack;
        }

    }

    bool testSwitch = false;
    private GameObject getTestTrack()
    {

        if (testSwitch == true)
        {

            GameObject testTracks = (GameObject)GameObject.Instantiate(testTrack, new Vector3(0, 0, -5000), new Quaternion(0, 0, 0, 0));
            
            testTracks.transform.parent = tracksRoot.transform;
            testTracks.transform.position = rootPos;

            testTracks.GetComponent<Track>().positioned = true;
            testTracks.gameObject.SetActive(true);

            disposeEditorTrackItems(testTracks);

            trackPool.Add(testTracks);

            testSwitch = false;

            return testTracks;
        }
        else
        {
            GameObject createdFixTrack = (GameObject)GameObject.Instantiate(fixTrack, new Vector3(0, 0, -5000), new Quaternion(0, 0, 0, 0));
            createdFixTrack.transform.parent = tracksRoot.transform;
            createdFixTrack.transform.position = rootPos;
            createdFixTrack.GetComponent<Track>().positioned = true;
            createdFixTrack.gameObject.SetActive(true);

            trackPool.Add(createdFixTrack);

            disposeEditorTrackItems(createdFixTrack);

            testSwitch = true;

            return createdFixTrack;
        }


    }

    private bool isInCurrentSelectedTracks(GameObject track)
    {
        //for (int i = 0; i < currentSelectedTracks.Count; i++)
        //{
        //    if (currentSelectedTracks[i].gameObject.name.Equals(track.gameObject.name))
        //    {
        //        return true;
        //    }
        //}

        return false;
    }

    IEnumerator populateTrack(GameObject track)
    {

        Transform trackObjectsRoot = track.transform.Find("trackObjects");

        // No Track Objects
        if (trackObjectsRoot == null) yield break;

        // Activating Decor Animators
        DecorAnimation.activeTrackDecors(track.GetComponent<Track>().ID, true);

        populateTrackObjects(trackObjectsRoot);

        yield break;
    }


    public IEnumerator injectTrack(string ID, Vector3 spawnPosition)
    {
        GameObject from = null;

        if (trackInjections == null) yield break;

        foreach (GameObject injection in trackInjections)
        {
            Track curTrack = injection.GetComponent<Track>();
            if (curTrack != null)
            {
                if (curTrack.ID.Equals(ID))
                {
                    from = (GameObject)Instantiate(injection, new Vector3(0, 0, -5000), Quaternion.identity);
                }
            }
        }

        if (from == null) yield break;

        Track fromTrack = from.GetComponent<Track>();

        if (fromTrack == null) yield break;

        from.transform.parent = tracksRoot.transform;
        from.transform.localPosition = spawnPosition;

        Transform trackObjectsRoot = from.transform.Find("trackObjects");

        // No Track Objects
        if (trackObjectsRoot == null) yield break;

        populateTrackObjects(trackObjectsRoot);

        fromTrack.positioned = true;
        injectedTrackPool.Add(from);


        yield return null;
    }



    public void populateTrackObjects(Transform trackObjectsRoot)
    {

        if (trackObjectsRoot == null)
        {
            Debug.Log("trackObjectsRoot yok");
        }

        foreach (Transform trackItem in trackObjectsRoot.transform)
        {

            TrackObject currentTrackObject = trackItem.GetComponent<TrackObject>();
            if (currentTrackObject != null)
            {

                if (currentTrackObject.placeHolder == true)
                {

                    if (currentTrackObject.objectGroup == TrackObject.ObjectGroup.None)
                    {

                        GameObject selectedObject = getRandomTrackObjectByType(currentTrackObject.objectType);

                        if (selectedObject != null)
                        {

                            // Positioning New Object
                            selectedObject.transform.parent = trackItem.transform.parent;
                            selectedObject.transform.position = trackItem.transform.position;


                            // Handle Selected
                            switch (currentTrackObject.objectType)
                            {
                                case TrackObject.ObjectType.PointsLine:

                                    CoinLine CurrencyLine_placeHolder = currentTrackObject.GetComponent<CoinLine>();
                                    CoinLine CurrencyLine_selected = selectedObject.GetComponent<CoinLine>();

                                    if (CurrencyLine_placeHolder != null && CurrencyLine_selected != null)
                                    {
                                        CurrencyLine_selected.coinSpacing = CurrencyLine_placeHolder.coinSpacing;
                                        CurrencyLine_selected.length = CurrencyLine_placeHolder.length;
                                        CurrencyLine_selected.coinLineRefraction = CurrencyLine_placeHolder.coinLineRefraction;

                                        CurrencyLine_selected.doActive();
                                    }

                                    break;
                                case TrackObject.ObjectType.PointsCurve:
                                    CoinCurve CurrencyCurve_placeHolder = currentTrackObject.GetComponent<CoinCurve>();
                                    CoinCurve CurrencyCurve_selected = selectedObject.GetComponent<CoinCurve>();

                                    if (CurrencyCurve_placeHolder != null && CurrencyCurve_selected != null)
                                    {
                                        CurrencyCurve_selected.offsetObject = null;
                                        CurrencyCurve_selected.offsetObject = CurrencyCurve_placeHolder.offsetObject;

                                        CurrencyCurve_selected.doActive();
                                    }


                                    break;
                                case TrackObject.ObjectType.PointsMovingLine:

                                    MovingCoin MovingCoin_placeHolder = currentTrackObject.GetComponent<MovingCoin>();
                                    MovingCoin MovingCoin_selected = selectedObject.GetComponent<MovingCoin>();

                                    if (MovingCoin_placeHolder != null && MovingCoin_selected != null)
                                    {
                                        MovingCoin_selected.speed = MovingCoin_placeHolder.speed;
                                    }

                                    CoinLine SubCoinLine_placeHolder = currentTrackObject.GetComponent<CoinLine>();
                                    CoinLine SubCoinLine_selected = selectedObject.transform.Find("movingMesh").gameObject.AddComponent<CoinLine>();

                                    if (SubCoinLine_placeHolder != null && SubCoinLine_selected != null)
                                    {
                                        SubCoinLine_selected.coinSpacing = SubCoinLine_placeHolder.coinSpacing;
                                        SubCoinLine_selected.length = SubCoinLine_placeHolder.length;
                                        SubCoinLine_selected.coinLineRefraction = SubCoinLine_placeHolder.coinLineRefraction;

                                        SubCoinLine_selected.doActive();
                                    }

                                    // Moving Coin
                                    if (MovingCoin_selected != null)
                                    {
                                        MovingCoin_selected.doActive();
                                    }

                                    break;

                            }



                        }
                        else
                        {
                            Debug.Log("Regular Item not found" + currentTrackObject.name);
                        }

                    }
                    else
                    {

                        GameObject selectedObject = null;

                        if (currentTrackObject.disableShuffle == false)
                        {
                            selectedObject = getRandomTrackObjectByGroup(currentTrackObject.objectGroup);
                        }
                        else
                        {
                            selectedObject = getRandomTrackObjectByType(currentTrackObject.objectType);
                        }

                        // Power-up handle codes places here
                        if (currentTrackObject.objectGroup == TrackObject.ObjectGroup.PowerUps)
                        {
                            if (checkPowerUpSpawning(selectedObject) == false) continue;
                        }

                        // Pickables handle codes places here
                        if (currentTrackObject.objectGroup == TrackObject.ObjectGroup.Pickables)
                        {
                            if (checkPowerUpSpawning(selectedObject) == false) continue;
                        }


                        if (selectedObject != null)
                        {

                            // Handle Selected
                            switch (currentTrackObject.objectGroup)
                            {
                                case TrackObject.ObjectGroup.None:
                                    break;
                                case TrackObject.ObjectGroup.CharacterObstacles:

                                    // Character Obstacles
                                    CharacterObstacle CharacterObstacle_placeHolder = currentTrackObject.GetComponent<CharacterObstacle>();
                                    CharacterObstacle CharacterObstacle_selected = selectedObject.GetComponent<CharacterObstacle>();

                                    if (CharacterObstacle_placeHolder != null && CharacterObstacle_selected != null)
                                    {
                                        CharacterObstacle_selected.lastWarrior = CharacterObstacle_placeHolder.lastWarrior;
                                        CharacterObstacle_selected.singleWarrior = CharacterObstacle_placeHolder.singleWarrior;
                                        CharacterObstacle_selected.doActive();
                                    }

                                    break;
                                case TrackObject.ObjectGroup.Bales1:
                                    break;
                                case TrackObject.ObjectGroup.Bales3:
                                    break;
                                case TrackObject.ObjectGroup.Bales5:
                                    break;
                                case TrackObject.ObjectGroup.Barriers:
                                    break;
                                case TrackObject.ObjectGroup.MovingObstaclesSingle:
                                   
                                    // Moving Obstacles Single
                                    MovingObstacle MovingObstaclesSingle_placeHolder = currentTrackObject.GetComponent<MovingObstacle>();
                                    MovingObstacle MovingObstaclesSingle_selected = selectedObject.GetComponent<MovingObstacle>();

                                    if (MovingObstaclesSingle_placeHolder != null && MovingObstaclesSingle_selected != null)
                                    {
                                        MovingObstaclesSingle_selected.trainCount = MovingObstaclesSingle_placeHolder.trainCount;
                                        MovingObstaclesSingle_selected.speed = MovingObstaclesSingle_placeHolder.speed;
                                        MovingObstaclesSingle_selected.doActive();
                                    }


                                    break;
                                case TrackObject.ObjectGroup.MovingObstaclesTrail3:

                                    // Moving Obstacles Trail
                                    MovingObstacle MovingObstaclesTrail_placeHolder3 = currentTrackObject.GetComponent<MovingObstacle>();
                                    MovingObstacle MovingObstaclesTrail_selected3 = selectedObject.GetComponent<MovingObstacle>();

                                    if (MovingObstaclesTrail_placeHolder3 != null && MovingObstaclesTrail_selected3 != null)
                                    {
                                        MovingObstaclesTrail_selected3.trainCount = MovingObstaclesTrail_placeHolder3.trainCount;
                                        MovingObstaclesTrail_selected3.speed = MovingObstaclesTrail_placeHolder3.speed;
                                        MovingObstaclesTrail_selected3.doActive();
                                    }

                                    break;
                                case TrackObject.ObjectGroup.MovingObstaclesTrail5:

                                    // Moving Obstacles Trail
                                    MovingObstacle MovingObstaclesTrail_placeHolder5 = currentTrackObject.GetComponent<MovingObstacle>();
                                    MovingObstacle MovingObstaclesTrail_selected5 = selectedObject.GetComponent<MovingObstacle>();

                                    if (MovingObstaclesTrail_placeHolder5 != null && MovingObstaclesTrail_selected5 != null)
                                    {
                                        MovingObstaclesTrail_selected5.trainCount = MovingObstaclesTrail_placeHolder5.trainCount;
                                        MovingObstaclesTrail_selected5.speed = MovingObstaclesTrail_placeHolder5.speed;
                                        MovingObstaclesTrail_selected5.doActive();
                                    }

                                    break;
                            }

                            // Positioning New Object
                            selectedObject.transform.parent = trackItem.transform.parent;
                            selectedObject.transform.position = trackItem.transform.position;

                        }
                        else
                        {
                            Debug.Log("Shuffled Item not found" + currentTrackObject.name);
                        }

                    }

                }

            }

        }

    }


    private GameObject getRandomTrackObjectByGroup(TrackObject.ObjectGroup objectGroup)
    {

        List<GameObject> randomPool = new List<GameObject>();

        foreach (GameObject currentObject in trackObjectPool)
        {
            TrackObject currentTrackObject = currentObject.GetComponent<TrackObject>();
            if (currentTrackObject != null)
            {

                if (currentTrackObject.positioned == false && currentTrackObject.objectGroup == objectGroup)
                {
                    randomPool.Add(currentObject);
                }

            }
        }

        if (randomPool.Count > 0)
        {

            GameObject selectedTrackObject = randomPool[Random.Range(0, randomPool.Count)];
            selectedTrackObject.GetComponent<TrackObject>().positioned = true;
            selectedTrackObject.gameObject.SetActive(true);

            return selectedTrackObject;
        }

        return null;
    }

    public GameObject getRandomTrackObjectByType(TrackObject.ObjectType objectType)
    {

        List<GameObject> randomPool = new List<GameObject>();

        foreach (GameObject currentObject in trackObjectPool)
        {
            TrackObject currentTrackObject = currentObject.GetComponent<TrackObject>();
            if (currentTrackObject != null)
            {

                if (currentTrackObject.positioned == false && currentTrackObject.objectType == objectType)
                {
                    randomPool.Add(currentObject);
                }

            }
        }

        if (randomPool.Count > 0)
        {
            GameObject selectedTrackObject = randomPool[Random.Range(0, randomPool.Count)];
            selectedTrackObject.GetComponent<TrackObject>().positioned = true;
            selectedTrackObject.gameObject.SetActive(true);
            return selectedTrackObject;
        }

        return null;
    }

    private bool checkPowerUpSpawning(GameObject powerUp)
    {

        TrackObject selectedPowerUp = powerUp.GetComponent<TrackObject>();
        if (selectedPowerUp != null)
        {

            switch (selectedPowerUp.objectType)
            {
                case TrackObject.ObjectType.PickableLetter:

                  GameGlobals.Instance.powerupController.peakPowerUp(PowerupController.powerUpTypes.Letter);

                    if (GameGlobals.Instance.powerupController.isPowerUpCanSpawn(PowerupController.powerUpTypes.Letter) == true)
                    {
                        return true;
                    }


                    break;
                case TrackObject.ObjectType.PickableFruit:

                    GameGlobals.Instance.powerupController.peakPowerUp(PowerupController.powerUpTypes.Fruit);

                    if (GameGlobals.Instance.powerupController.isPowerUpCanSpawn(PowerupController.powerUpTypes.Fruit) == true)
                    {
                        return true;
                    }


                    break;

                case TrackObject.ObjectType.PowerupVacuum:


                    GameGlobals.Instance.powerupController.peakPowerUp(PowerupController.powerUpTypes.Vacuum);

                    if (GameGlobals.Instance.powerupController.isPowerUpCanSpawn(PowerupController.powerUpTypes.Vacuum) == true)
                    {
                        return true;
                    }

                    break;


                case TrackObject.ObjectType.PowerupBomb:

                    GameGlobals.Instance.powerupController.peakPowerUp(PowerupController.powerUpTypes.Bomb);

                    if (GameGlobals.Instance.powerupController.isPowerUpCanSpawn(PowerupController.powerUpTypes.Bomb) == true)
                    {
                        return true;
                    }

                    break;


            }

        }

        selectedPowerUp.positioned = false;
        selectedPowerUp.transform.parent = trackObjectsRoot.transform;
        selectedPowerUp.transform.position = rootPos;
        selectedPowerUp.gameObject.SetActive(false);

        return false;
    }

    IEnumerator clearTracksAndObjectsBehind()
    {

        // Tutorial Track
        if (tutorialTrackInstance != null)
        {
            Track tutorialTrack = tutorialTrackInstance.GetComponent<Track>();

            if (tutorialTrack != null)
            {
                if (player.transform.position.z > tutorialTrackInstance.gameObject.transform.position.z + tutorialTrack.zSize + 50.0f)
                {
                    disposeTrackAndTrackObjects(tutorialTrack, true);
                }
            }
           
          
        }

        // Track Pools
        disposeTrackPoolBehind(startTrackPool);
        disposeTrackPoolBehind(trackPool);
        disposeTrackPoolBehind(injectedTrackPool);

        yield return null;
    }

    private void disposeTrackPoolBehind(List<GameObject> tPool)
    {

        List<int> injectedTracksForDeleting = new List<int>();

        for (int i = 0; i < tPool.Count; i++)
        {
            Track item = tPool[i].GetComponent<Track>();

            if (item != null)
            {
                if (item.positioned == true)
                {
                    if (player.transform.position.z > item.gameObject.transform.position.z + item.zSize + 50.0f)
                    {
                        // Disposing Tracks
                        disposeTrackAndTrackObjects(item, true);

                        if (item.injectedTrack == true)
                        {
                            injectedTracksForDeleting.Add(i);
                        }
                    }



                }
            }

        }

        // Destroying Injected Tracks
        for (int i = 0; i < injectedTracksForDeleting.Count; i++)
        {
            Destroy(injectedTrackPool[injectedTracksForDeleting[i]]);
            injectedTrackPool.RemoveAt(injectedTracksForDeleting[i]);
        }

    }

    private void disposeAllTracks()
    {

        // Tutorial Track
        if (tutorialTrackInstance != null)
        {
            disposeTrackAndTrackObjects(tutorialTrackInstance.GetComponent<Track>(), true);
        }


        // Disposing All Start Tracks
        for (int i = 0; i < startTrackPool.Count; i++)
        {
            Track item = startTrackPool[i].GetComponent<Track>();
            if (item != null)
            {
                disposeTrackAndTrackObjects(item, true);
            }
        }


        // Disposing All Tracks
        for (int i = 0; i < trackPool.Count; i++)
        {
            Track item = trackPool[i].GetComponent<Track>();
            if (item != null)
            {
                disposeTrackAndTrackObjects(item, true);
            }
        }

    }



    public void disposeTrackAndTrackObjects(Track track, bool disposeTrackGeometry)
    {

        // Disabling Decor Animators
        DecorAnimation.activeTrackDecors(track.ID, false);

        if (disposeTrackGeometry == true)
        {

            track.positioned = false;
            track.gameObject.transform.position = rootPos;
            track.gameObject.SetActive(false);
        }


        List<Transform> objectsToClear = new List<Transform>();

        Transform currentTrackObjectsRoot = track.gameObject.transform.Find("trackObjects");
        if (currentTrackObjectsRoot != null)
        {
            foreach (Transform trackItem in currentTrackObjectsRoot.transform)
            {
                TrackObject curTrackObject = trackItem.GetComponent<TrackObject>();

                if (curTrackObject.placeHolder == false)
                {
                    objectsToClear.Add(trackItem);
                }

            }
        }

        if (objectsToClear.Count > 0)
        {
            foreach (Transform clearItem in objectsToClear)
            {

                TrackObject clearedTrackObject = clearItem.GetComponent<TrackObject>();
                if (clearedTrackObject != null)
                {
                    clearedTrackObject.positioned = false;

                    // Handle Track Object
                    switch (clearedTrackObject.objectType)
                    {
                        case TrackObject.ObjectType.PointsLine:
                            if (clearedTrackObject.gameObject.GetComponent<CoinLine>() != null) clearedTrackObject.gameObject.GetComponent<CoinLine>().doDeactive();
                            break;
                        case TrackObject.ObjectType.PointsCurve:
                            if (clearedTrackObject.gameObject.GetComponent<CoinCurve>() != null) clearedTrackObject.gameObject.GetComponent<CoinCurve>().doDeactive();
                            break;
                        case TrackObject.ObjectType.PointsMovingLine:
                            if (clearedTrackObject.gameObject.transform.Find("movingMesh").GetComponent<CoinLine>() != null) clearedTrackObject.gameObject.transform.Find("movingMesh").GetComponent<CoinLine>().doDeactive();
                            if (clearedTrackObject.gameObject.transform.Find("movingMesh").GetComponent<CoinLine>() != null) Destroy(clearedTrackObject.gameObject.transform.Find("movingMesh").GetComponent<CoinLine>());
                            if (clearedTrackObject.gameObject.GetComponent<MovingCoin>() != null) clearedTrackObject.gameObject.GetComponent<MovingCoin>().doDeactive();
                            break;
                    }

                    switch (clearedTrackObject.objectGroup)
                    {
                        case TrackObject.ObjectGroup.CharacterObstacles:
                            if (clearedTrackObject.gameObject.GetComponent<CharacterObstacle>() != null) clearedTrackObject.gameObject.GetComponent<CharacterObstacle>().doDeactive();
                            break;
                        case TrackObject.ObjectGroup.MovingObstaclesTrail3:
                            if (clearedTrackObject.gameObject.GetComponent<MovingObstacle>() != null) clearedTrackObject.gameObject.GetComponent<MovingObstacle>().doDeactive();
                            break;
                        case TrackObject.ObjectGroup.MovingObstaclesTrail5:
                            if (clearedTrackObject.gameObject.GetComponent<MovingObstacle>() != null) clearedTrackObject.gameObject.GetComponent<MovingObstacle>().doDeactive();
                            break;
                    }

                }

                clearItem.transform.parent = trackObjectsRoot.transform;
                clearItem.position = rootPos;
                clearItem.gameObject.SetActive(false);

            }

        }




    }

    public List<GameObject> getActiveTracks()
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Transform track in tracksRoot.transform)
        {
            if (track.gameObject.activeInHierarchy == true)
            {
                result.Add(track.gameObject);
            }
        }

        return result;
    }



    private int getMaxCountPerTrackObject(TrackObject trackObject)
    {

        switch (trackObject.objectType)
        {
            case TrackObject.ObjectType.None:
                break;
            case TrackObject.ObjectType.PointsSingle:
                return 300;
            case TrackObject.ObjectType.PointsLine:
                return 50;
            case TrackObject.ObjectType.PointsCurve:
                return 50;
            case TrackObject.ObjectType.PointsMovingLine:
                return 50;
            case TrackObject.ObjectType.PickableLetter:
                return 10;
            case TrackObject.ObjectType.PickableFruit:
                return 10;
            case TrackObject.ObjectType.PowerupVacuum:
                return 10;
            case TrackObject.ObjectType.PowerupBomb:
                return 10;
            case TrackObject.ObjectType.Ramp:
                return 20;
            case TrackObject.ObjectType.BaleYellow1:
                return 20;
            case TrackObject.ObjectType.BaleYellow3:
                return 20;
            case TrackObject.ObjectType.BaleYellow5:
                return 20;
            case TrackObject.ObjectType.BaleGreen1:
                return 20;
            case TrackObject.ObjectType.BaleGreen3:
                return 20;
            case TrackObject.ObjectType.BaleGreen5:
                return 20;
            case TrackObject.ObjectType.BaleGray1:
                return 20;
            case TrackObject.ObjectType.BaleGray3:
                return 20;
            case TrackObject.ObjectType.BaleGray5:
                return 20;
            case TrackObject.ObjectType.BarrierUp:
                return 20;
            case TrackObject.ObjectType.BarrierDown:
                return 20;
            case TrackObject.ObjectType.BarrierUpDown:
                return 20;
            case TrackObject.ObjectType.MovingElephant:
                return 10;
            case TrackObject.ObjectType.MovingGazelle:
                return 10;
            case TrackObject.ObjectType.MovingGireffe:
                return 10;
            case TrackObject.ObjectType.MovingHippo:
                return 10;
            case TrackObject.ObjectType.MovingLion:
                return 10;
            case TrackObject.ObjectType.MovingRhino:
                return 10;
            case TrackObject.ObjectType.MovingTiger:
                return 10;
            case TrackObject.ObjectType.MovingZebra:
                return 10;
            case TrackObject.ObjectType.StaticCroco:
                return 40;
            case TrackObject.ObjectType.StaticMonke:
                return 40;
            case TrackObject.ObjectType.TunnelCenter:
                return 10;
            case TrackObject.ObjectType.TunnelLeft:
                return 10;
            case TrackObject.ObjectType.TunnelRight:
                return 10;
            case TrackObject.ObjectType.Tunnel2Path:
                return 10;
            case TrackObject.ObjectType.TrailYellow3:
                return 10;
            case TrackObject.ObjectType.TrailGreen3:
                return 10;
            case TrackObject.ObjectType.TrailGray3:
                return 10;
            case TrackObject.ObjectType.TrailYellow5:
                return 10;
            case TrackObject.ObjectType.TrailGreen5:
                return 10;
            case TrackObject.ObjectType.TrailGray5:
                return 10;
            case TrackObject.ObjectType.StaticGruba:
                return 40;
            case TrackObject.ObjectType.StaticWaba:
                return 40;

        }

        return 0;
    }

}
