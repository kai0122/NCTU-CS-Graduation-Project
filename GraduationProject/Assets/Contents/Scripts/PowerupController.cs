using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine.UI;

public class PowerupController : MonoBehaviour
{
    [HideInInspector]
    private GameObject player;



    // Letter
    public GameObject letterBoardPanel;
    public TweenPosition letterBoardTween;
    public Text lblWord;
    private List<string> words = new List<string>();
    private string activeWord;
    private int collectedWord;
    private bool wordCompleted;
    public Text lblCollectedLetterCount;


    // Fruit
    private int activeFruits;
    private int collectedFruits;
    private bool fruitsCompleted;
    public Text lblCollectedFruitCount;

   
   
    // Vacuum
    public GameObject powerupPanel;
    public TweenPosition powerupPanelTween;
    public Image powerupProgressBar;

    private Coroutine vacuumPowerupCoroutine;
    public GameObject PowerupVacuumRef;
    private GameObject powerupVacuum;
    public GameObject PowerupVacuumCollider;
    private ColliderTrigger coinMagnetCollider;


    // Message
    public TweenPosition messageBoardTween;
    public Text lblMessage;



    public enum powerUpTypes
    {
        Letter = 0,
        Vacuum = 1,
        Fruit = 2,
        Bomb = 3
    }

    public enum powerUpMode
    {
        Pickable = 0,
        PowerUp = 1
    }


    public class PowerUp
    {
        public powerUpTypes powerUpType;
        public float duration;

        public int minSpawn, maxSpawn;
        public int spawnAt, counter;

        public bool canSpawn;
        public bool active;

        public void peak()
        {
            if (counter == spawnAt)
            {
                canSpawn = true;
                counter++;
            }
            else if (counter > spawnAt)
            {
                canSpawn = false;
                spawnAt = UnityEngine.Random.Range(minSpawn, maxSpawn);
                counter = 0;
            }
            else if (counter < spawnAt)
            {
                canSpawn = false;
                counter++;
            }

        }

    }

    private List<PowerUp> powerUps = new List<PowerUp>();


    private void Awake()
    {

        player = GameObject.Find("Player");


        // Hiding Powerup Progress Icons
     
        // Vacuum
        coinMagnetCollider = PowerupVacuumCollider.GetComponent<ColliderTrigger>();
        coinMagnetCollider.OnEnter = new ColliderTrigger.OnEnterDelegate(this.onCoinEnter);

     

        // Setup Powerups
        if (powerUps.Count <= 0)
        {
            // Letter
            PowerUp letter = new PowerUp();
            letter.powerUpType = powerUpTypes.Letter;
            letter.minSpawn = 0;
            letter.maxSpawn = 3;
            letter.spawnAt = UnityEngine.Random.Range(letter.minSpawn, letter.maxSpawn);
            powerUps.Add(letter);

            // Fruit
            PowerUp fruit = new PowerUp();
            fruit.powerUpType = powerUpTypes.Fruit;
            fruit.minSpawn = 0;
            fruit.maxSpawn = 3;
            fruit.spawnAt = UnityEngine.Random.Range(fruit.minSpawn, fruit.maxSpawn);
            powerUps.Add(fruit);

            // Vacuum
            PowerUp vacuum = new PowerUp();
            vacuum.powerUpType = powerUpTypes.Vacuum;
            vacuum.duration = 10;
            vacuum.minSpawn = 0;
            vacuum.maxSpawn = 5;
            vacuum.spawnAt = UnityEngine.Random.Range(vacuum.minSpawn, vacuum.maxSpawn);
            powerUps.Add(vacuum);

            // Bomb
            PowerUp bomb = new PowerUp();
            bomb.powerUpType = powerUpTypes.Bomb;
            bomb.duration = 10;
            bomb.minSpawn = 0;
            bomb.maxSpawn = 1;
            bomb.spawnAt = UnityEngine.Random.Range(bomb.minSpawn, bomb.maxSpawn);
            powerUps.Add(bomb);

        }

        initWords();

    }

   
    public bool isPowerUpCanSpawn(powerUpTypes powerUpType)
    {
        if (isAnyPowerupActive() == true) return false;
        if (powerUpType == powerUpTypes.Letter && wordCompleted == true) return false;
        if (powerUpType == powerUpTypes.Fruit && fruitsCompleted == true) return false;

        return getPowerUp(powerUpType).canSpawn;

    }

    public void peakPowerUp(powerUpTypes powerUpType)
    {
        getPowerUp(powerUpType).peak();
    }


    private PowerUp getPowerUp(powerUpTypes powerUpType)
    {

        for (int i = 0; i < powerUps.Count; i++)
        {
            if (powerUps[i].powerUpType == powerUpType)
            {
                return powerUps[i];
            }

        }

        return null;
    }

    private bool isAnyPowerupActive()
    {

        for (int i = 0; i < powerUps.Count; i++)
        {
            if (powerUps[i].active == true)
            {
                return true;
            }

        }

        return false;
    }

    public void activatePowerUp(powerUpTypes powerUpType)
    {

        // Deactiving All Powerups
        for (int i = 0; i < powerUps.Count; i++)
        {
            powerUps[i].active = false;
        }

        for (int i = 0; i < powerUps.Count; i++)
        {
            if (powerUps[i].powerUpType == powerUpType)
            {
                powerUps[i].active = true;
            }
        }

    }



    public void DeActivateAllPowerUps()
    {

        // Deactiving All Powerups
        for (int i = 0; i < powerUps.Count; i++)
        {
            powerUps[i].canSpawn = false;
            powerUps[i].active = false;
            powerUps[i].spawnAt = UnityEngine.Random.Range(powerUps[i].minSpawn, powerUps[i].maxSpawn);
            powerUps[i].counter = 0;
        }

    }

    private void clearAllPowerupsAfterPickOne()
    {

        List<GameObject> acitveTracks = GameGlobals.Instance.trackGenerator.getActiveTracks();

        foreach (GameObject track in acitveTracks)
        {
            Transform trackObjects = track.transform.Find("trackObjects");
            if (trackObjects != null)
            {

                foreach (Transform trackItem in trackObjects.transform)
                {
                    TrackObject trackObject = trackItem.GetComponent<TrackObject>();
                    if (trackObject != null)
                    {
                        if (trackObject.placeHolder == false && trackObject.objectGroup == TrackObject.ObjectGroup.PowerUps)
                        {

                            trackObject.positioned = false;
                            trackObject.transform.parent = GameGlobals.Instance.trackGenerator.trackObjectsRoot.transform;
                            trackObject.gameObject.transform.position = GameGlobals.Instance.trackGenerator.rootPos;
                            trackObject.gameObject.SetActive(false);

                        }
                    }
                }
            }
        }

    }

    private void clearAllRemainPowerupsForType(TrackObject.ObjectType type)
    {
        List<GameObject> acitveTracks = GameGlobals.Instance.trackGenerator.getActiveTracks();

        foreach (GameObject track in acitveTracks)
        {
            Transform trackObjects = track.transform.Find("trackObjects");
            if (trackObjects != null)
            {

                foreach (Transform trackItem in trackObjects.transform)
                {
                    TrackObject trackObject = trackItem.GetComponent<TrackObject>();
                    if (trackObject != null)
                    {
                        if (trackObject.placeHolder == false && trackObject.objectType == type)
                        {

                            trackObject.positioned = false;
                            trackObject.transform.parent = GameGlobals.Instance.trackGenerator.trackObjectsRoot.transform;
                            trackObject.gameObject.transform.position = GameGlobals.Instance.trackGenerator.rootPos;
                            trackObject.gameObject.SetActive(false);

                        }
                    }
                }
            }
        }

    }


    public IEnumerator showMessage(string message, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        lblMessage.text = message;
        messageBoardTween.ResetToBeginning();
        messageBoardTween.PlayForward();
        StartCoroutine(hideTweenPanel(messageBoardTween));

    }


    private IEnumerator hideTweenPanel(TweenPosition tween)
    {
        yield return new WaitForSeconds(3.0f);
        tween.PlayReverse();
    }

    public void Reset()
    {

        DeActivateAllPowerUps();
        assignNewWord();
        assignNewFruits();

        shuffleFruits();
        wordCompleted = false;
        fruitsCompleted = false;

        if (powerupVacuum != null)
        {
            powerupVacuum.gameObject.SetActive(false);
        }

        if (PowerupVacuumCollider != null)
        {
            PowerupVacuumCollider.gameObject.SetActive(false);
        }

    }

    private IEnumerator addCoinsOnGui(int coinsTotal, float delay)
    {
        // Start delay
        yield return new WaitForSeconds(delay);

        float duration = 1.0f; // 1 sec.
        float time = 0;
        float counter = 0;

        while (true)
        {

            // Adding Coin
            float currentIncrement = ((((float)coinsTotal / duration) * time) / counter);
            if (currentIncrement > 0)
            {
                GameGlobals.Instance.achievements.currentCoins += Mathf.RoundToInt(currentIncrement);
            }

            // Clerking Sound
            float pitch = 1.0f + 0.3f / duration * time;
            GameGlobals.Instance.audioController.playSoundPitched("PowerupDefault", pitch);


            if (time >= duration)
            {
                //Debug.Log((((float)coinsTotal / duration) * time) / counter);
                break;
            }

            counter++;
            time += 0.05f;
            yield return new WaitForSeconds(0.05f);

        }

    }


    // LETTER POWERUP------------------------------------------------------------------------------------------------------------------------------

    private void initWords()
    {

        words.Clear();

        words.Add("Zebra");
        words.Add("Snake");
        words.Add("Spider");
        words.Add("Jungle");
        words.Add("Parrot");
        words.Add("Monkey");
        words.Add("Bee");
        words.Add("Lion");
        words.Add("Tiger");
        words.Add("Duck");
        words.Add("Frog");
        words.Add("Owl");
        words.Add("Bear");
        words.Add("Rabbit");
        words.Add("Racoon");

    }

    private void assignNewWord()
    {
        activeWord = words[UnityEngine.Random.Range(0, words.Count)];
        collectedWord = 0;
        refreshLetterPowerups();
        refreshCollectedLetterLabel();
    }

    private void refreshCollectedLetterLabel()
    {
        if (lblCollectedLetterCount != null)
        {
            lblCollectedLetterCount.text = activeWord.Length.ToString() + "/" + collectedWord.ToString();
        }
    }

    public void doLetterCollected()
    {
        if (wordCompleted == true) return;


        collectedWord++;
        showLetterBoard();

        int currentCollectionProgress = 100 / activeWord.Length * collectedWord;
        float collectPicth = 0.5f / 100.0f * (float)currentCollectionProgress;
        GameGlobals.Instance.audioController.playSoundPitched("PowerupLetterStep", 1.0f + collectPicth);
        refreshCollectedLetterLabel();

        if (activeWord.Length == collectedWord)
        {
            //Debug.Log("Word Complete");
            clearAllRemainPowerupsForType(TrackObject.ObjectType.PickableLetter);
            GameGlobals.Instance.audioController.playSound("PowerupLetterComplete", false);
            StartCoroutine(showMessage("You got 500 coins !".Replace("[X]", "500"), 3.0f));  // You got 500 coins !
            StartCoroutine(addCoinsOnGui(500, 4));

            wordCompleted = true;
        }

        if (wordCompleted == false)
        {
            refreshLetterPowerups();
        }

    }


    public void refreshLetterPowerups()
    {
        if (activeWord == null) return;
        if (activeWord.Equals("")) return;


        string currentLetter = activeWord.Substring(collectedWord, 1).ToLower(); ;

        for (int i = 0; i < GameGlobals.Instance.trackGenerator.trackObjectPool.Count; i++)
        {

            TrackObject curObject = GameGlobals.Instance.trackGenerator.trackObjectPool[i].GetComponent<TrackObject>();
            if (curObject != null)
            {
                if (curObject.ID.Equals("letter"))
                {

                    Transform lettersRoot = curObject.gameObject.transform.Find("letters");
                    if (lettersRoot != null)
                    {
                        foreach (Transform letter in lettersRoot.transform)
                        {
                            if (letter.name.ToLower().Equals(currentLetter))
                            {
                                letter.gameObject.SetActive(true);
                            }
                            else
                            {
                                letter.gameObject.SetActive(false);
                            }

                        }
                    }
                }
            }

        }

    }


    public void showLetterBoard()
    {
        lblWord.text = "<color=red>" + activeWord.Insert(collectedWord, "</color>").ToUpper();
        letterBoardPanel.SetActive(true);
        letterBoardTween.PlayForward();
        StartCoroutine(hideTweenPanel(letterBoardTween));
    }


    //---------------------------------------------------------------------------------------------------------------------------------------------

    // FRUIT POWERUP------------------------------------------------------------------------------------------------------------------------------

    private void assignNewFruits()
    {

        activeFruits = UnityEngine.Random.Range(4, 9);
        //activeFruits = 1;
        collectedFruits = 0;
        refreshCollectedFruitsLabel();
    }


    private void refreshCollectedFruitsLabel()
    {
        if (lblCollectedFruitCount != null)
        {
            lblCollectedFruitCount.text = activeFruits.ToString() + "/" + collectedFruits.ToString();
        }
    }

    public void doFruitCollected()
    {

        if (fruitsCompleted == true) return;

        collectedFruits++;

        int currentCollectionProgress = 100 / activeFruits * collectedFruits;
        float collectPicth = 0.5f / 100.0f * (float)currentCollectionProgress;
        GameGlobals.Instance.audioController.playSoundPitched("PowerupLetterStep", 1.0f + collectPicth);
        GameGlobals.Instance.audioController.playSound("PowerupEat", false);

        refreshCollectedFruitsLabel();

        if (activeFruits == collectedFruits)
        {
            //Debug.Log("Word Complete");
            clearAllRemainPowerupsForType(TrackObject.ObjectType.PickableFruit);

            GameGlobals.Instance.audioController.playSound("PowerupLetterComplete", false);
            StartCoroutine(showMessage("Wo hoo! You collected all fruits!", 0.5f)); 
            StartCoroutine(showMessage(" You got 500 coins".Replace("[X]", "500"), 5.0f));
            StartCoroutine(addCoinsOnGui(500, 6));


            fruitsCompleted = true;
        }

        if (fruitsCompleted == false)
        {
            shuffleFruits();
        }

    }


    public void shuffleFruits()
    {

        for (int i = 0; i < GameGlobals.Instance.trackGenerator.trackObjectPool.Count; i++)
        {

            TrackObject curObject = GameGlobals.Instance.trackGenerator.trackObjectPool[i].GetComponent<TrackObject>();
            if (curObject != null)
            {
                if (curObject.ID.Equals("fruit"))
                {

                    Transform fruitsRoot = curObject.gameObject.transform.Find("fruits");
                    if (fruitsRoot != null)
                    {
                        foreach (Transform fruit in fruitsRoot.transform)
                        {
                            fruit.gameObject.SetActive(false);
                        }

                        fruitsRoot.transform.GetChild(UnityEngine.Random.Range(0, fruitsRoot.transform.childCount)).gameObject.SetActive(true);
                    }
                }
            }

        }

    }


    //---------------------------------------------------------------------------------------------------------------------------------------------

    // VACUUM POWERUP------------------------------------------------------------------------------------------------------------------------------

    public void doVacuumCollected()
    {
        PowerUp vacuum = getPowerUp(powerUpTypes.Vacuum);
        vacuum.active = true;

        // Checking Spine
        GameObject spine = player.transform.Find("playerController/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1").gameObject;
        if (spine == null) return;

        // Checking Vacuum Ref
        Transform vacuumRefControl = spine.transform.Find("vacuum");
        if (vacuumRefControl == null)
        {
            powerupVacuum = (GameObject)Instantiate(PowerupVacuumRef, new Vector3(0, 0, 0), Quaternion.identity);
            powerupVacuum.transform.parent = spine.transform;
            powerupVacuum.transform.localPosition = new Vector3(-0.09f, 0.51f, -0.51f);
            powerupVacuum.transform.localRotation = new Quaternion(0, 180.0f, 0, 0);
        }


        if (powerupVacuum != null)
        {
            powerupVacuum.gameObject.SetActive(true);
        }

        if (PowerupVacuumCollider != null)
        {
            PowerupVacuumCollider.gameObject.SetActive(true);
        }

        powerupPanel.SetActive(true);
        powerupPanelTween.PlayForward();

        GameGlobals.Instance.audioController.playSound("PowerupCollectVacuum", false);

        vacuumPowerupCoroutine = StartCoroutine(hadleVacuumPowerUp());

        clearAllPowerupsAfterPickOne();

    }

    public IEnumerator hadleVacuumPowerUp()
    {
        PowerUp vacuum = getPowerUp(powerUpTypes.Vacuum);

        bool vacumLoopTrig = false;
        bool vacumLoopEndTrig = false;
        bool pauseTrig = false;

        float currentTime = 0;

        while (true)
        {

            if (GameGlobals.Instance.currentGameState.Equals("OnGameRunning") || GameGlobals.Instance.currentGameState.Equals("onPauseGame") || GameGlobals.Instance.currentGameState.Equals("onGameOver"))
            {

                // When Death
                if (GameGlobals.Instance.currentGameState.Equals("onGameOver"))
                {
                    vacuum.active = false;

                    if (powerupVacuum != null)
                    {
                        powerupVacuum.gameObject.SetActive(false);
                    }

                    if (PowerupVacuumCollider != null)
                    {
                        PowerupVacuumCollider.gameObject.SetActive(false);
                    }

                    powerupPanelTween.PlayReverse();
                    GameGlobals.Instance.audioController.stopSound("PowerupVacuumLoop", false);
                    GameGlobals.Instance.audioController.playSound("PowerupPoof", false);
                    GameGlobals.Instance.audioController.playSound("PowerupVacuumEnd", false);

                    GameGlobals.Instance.controller.doAnEffect(Controller.EffetcType.PowerupPickUp);

                    break;
                }

                // When pause
                while (GameGlobals.Instance.currentGameState.Equals("onPauseGame"))
                {
                    if (pauseTrig == false)
                    {
                        if (PowerupVacuumCollider != null)
                        {
                            PowerupVacuumCollider.gameObject.SetActive(false);
                        }

                        GameGlobals.Instance.audioController.stopSound("PowerupVacuumLoop", false);
                        powerupPanelTween.PlayReverse();
                        pauseTrig = true;
                    }


                    yield return new WaitForFixedUpdate();
                }


                float currentProgress = 1.0f - ((1.0f / vacuum.duration * currentTime));

                if (powerupProgressBar != null)
                {
                    powerupProgressBar.fillAmount = currentProgress;
                }

                if (currentTime > 0.4f && vacumLoopTrig == false)
                {
                    GameGlobals.Instance.audioController.playSound("PowerupVacuumLoop", false);
                    vacumLoopTrig = true;
                }

                if (pauseTrig == true)
                {
                    if (PowerupVacuumCollider != null)
                    {
                        PowerupVacuumCollider.gameObject.SetActive(true);
                    }


                    GameGlobals.Instance.audioController.playSound("PowerupVacuumLoop", false);
                    powerupPanelTween.PlayForward();
                    pauseTrig = false;
                }

                if (currentTime > vacuum.duration - 0.4f && vacumLoopEndTrig == false)
                {
                    GameGlobals.Instance.audioController.stopSound("PowerupVacuumLoop", false);
                    GameGlobals.Instance.audioController.playSound("PowerupVacuumEnd", false);
                    vacumLoopEndTrig = true;
                }


                if (currentTime >= vacuum.duration)
                {
                    vacuum.active = false;

                    if (powerupVacuum != null)
                    {
                        powerupVacuum.gameObject.SetActive(false);
                    }

                    if (PowerupVacuumCollider != null)
                    {
                        PowerupVacuumCollider.gameObject.SetActive(false);
                    }

                    powerupPanelTween.PlayReverse();

                    GameGlobals.Instance.audioController.playSound("PowerupPoof", false);
                    GameGlobals.Instance.controller.doAnEffect(Controller.EffetcType.PowerupPickUp);

                    break;
                }

                currentTime += 0.1f;
                yield return new WaitForSeconds(0.1f);


            }
            else
            {

                vacuum.active = false;

                if (powerupVacuum != null)
                {
                    powerupVacuum.gameObject.SetActive(false);
                }


                if (PowerupVacuumCollider != null)
                {
                    PowerupVacuumCollider.gameObject.SetActive(false);
                }

                powerupPanelTween.PlayReverse();
                GameGlobals.Instance.audioController.stopSound("PowerupVacuumLoop", false);

                break;

            }

        }


    }

    public void onCoinEnter(Collider collider)
    {

        if (collider.gameObject.transform.parent == null) return;

        Coin collidedCoin = collider.gameObject.transform.parent.GetComponent<Coin>();

        if (collidedCoin != null && GameGlobals.Instance.isInGamePlay() == true)
        {
            float single = 70f;
            if (player.transform.position.y >= single && collidedCoin.transform.position.y >= single || player.transform.position.y < single && collidedCoin.transform.position.y < single)
            {
                collider.GetComponent<Collider>().enabled = false;
                StartCoroutine(Vacuum(collidedCoin));

            }
        }

    }

    IEnumerator Vacuum(Coin coin)
    {

        if (powerupVacuum != null)
        {
            Vector3 from = coin.gameObject.transform.position;
            Transform to = powerupVacuum.transform.Find("magnetPos").transform;

            float distance = (from - player.transform.position).magnitude;

            yield return StartCoroutine(ShortTween.Play(distance / (150.0f * GameGlobals.Instance.controller.NormalizedGameSpeed()), new Action<float>((float t) => coin.gameObject.transform.position = Vector3.Lerp(from, to.position, t * t))));

            // Pickup
            coin.pickUp();
        }



        yield return null;
    }


    //---------------------------------------------------------------------------------------------------------------------------------------------

    public void addAchievementPoints(string message, int points)
    {

        GameGlobals.Instance.audioController.playSound("PowerupLetterComplete", false);
        StartCoroutine(showMessage(message, 1.0f));
        StartCoroutine(addCoinsOnGui(points, 3.0f));

    }

}