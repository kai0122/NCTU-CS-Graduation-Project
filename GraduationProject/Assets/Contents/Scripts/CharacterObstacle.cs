using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CharacterObstacle : MonoBehaviour
{

    private bool active;
    private static Animator playerAnimator;
    public Collider obstacleCollider;


    private ColliderTrigger onTriggerObject;
    private Animator animator;
   


    public GameObject hitEffect;
    public bool lastWarrior;

    public bool singleWarrior;
    private static bool inSingleWarriorPlace;


    private void Start()
    {

        playerAnimator = GameObject.Find("playerController").GetComponent<Animator>(); 
       

        if (GameObject.Find("CombatPanel") != null)
        {
            hudCombatZoneAnimation = GameObject.Find("CombatPanel").GetComponent<TweenPosition>();
            lblBeatenEnemys = GameObject.Find("lblBeatenEnemys").GetComponent<Text>();
            lblBeatenEnemysAnim = GameObject.Find("lblBeatenEnemys").GetComponent<TweenScale>();
        }
     
    
        Transform obstacle = this.transform.Find("obstacle");
        if (obstacle != null)
        {
            animator = obstacle.GetComponent<Animator>();
            //animator.enabled = false;
        }

        Transform trigger = this.transform.Find("trigger");
        if (trigger != null)
        {
            onTriggerObject = trigger.GetComponent<ColliderTrigger>();
            onTriggerObject.OnEnter = (ColliderTrigger.OnEnterDelegate)Delegate.Combine(onTriggerObject.OnEnter, new ColliderTrigger.OnEnterDelegate(this.OnPlayerEnter));
            onTriggerObject.OnExit = (ColliderTrigger.OnExitDelegate)Delegate.Combine(onTriggerObject.OnExit, new ColliderTrigger.OnExitDelegate(this.OnPlayerExit));
        }

        playAnimation("characterObstacleReady");


    }


    private bool tooCloseTrig;
    private bool karateMoveTrig;

    private void FixedUpdate()
    {

        if (active == true)
        {

            float distance = Distance(GameGlobals.Instance.player.transform.position, this.transform.position);


            if (tooCloseTrig == false && distance < 60.0f)
            {
                // Playing Ready Karate Anim
                playAnimation("close" + UnityEngine.Random.Range(1, 3).ToString());
                tooCloseTrig = true;
            }

            if (GameGlobals.Instance.controller.isRolling == true && GameGlobals.Instance.controller.trackIndex == getTrackIndex() && karateMoveTrig == false && distance < 10.0f)
            {
                karateMoveTrig = true;

                if (obstacleCollider != null)
                {
                    obstacleCollider.enabled = false;
                }

                // Hitting!
                playAnimation("hit");
                GameGlobals.Instance.audioController.playSound("VillainHit" + UnityEngine.Random.Range(1, 5).ToString(), false);

                if (hitEffect != null)
                {
                    GameObject.Instantiate(hitEffect, new Vector3(this.transform.position.x, this.transform.position.y + 5.0f, this.transform.position.z), Quaternion.identity);
                }


                GameGlobals.Instance.controller.doAnEffect(Controller.EffetcType.QuicScore);

            }


            if (inKarateMove == true && GameGlobals.Instance.controller.trackIndex == getTrackIndex() && karateMoveTrig == false && distance < 10.0f)
            {

                karateMoveTrig = true;

                if (obstacleCollider != null)
                {
                    obstacleCollider.enabled = false;
                }

                // Hitting!
                playAnimation("hit");
                GameGlobals.Instance.audioController.playSound("VillainHit" + UnityEngine.Random.Range(1, 5).ToString(), false);

                if (hitEffect != null)
                {
                    GameObject.Instantiate(hitEffect, new Vector3(this.transform.position.x, this.transform.position.y + 5.0f, this.transform.position.z), Quaternion.identity);
                }


                GameGlobals.Instance.controller.doAnEffect(Controller.EffetcType.QuicScore);

                


            }


        }

    }

    public void onCharacterHit()
    {

        active = false;
        playAnimation("hitBalance");

    }

    private void OnPlayerEnter(Collider collider)
    {
        if (collider == null) return;

        if (collider.name.Equals("Player") && GameGlobals.Instance.isInGamePlay() == true)
        {

            if (animator != null)
            {
                animator.enabled = true;
            }

            playAnimation("characterObstacleReady");
            active = true;

            if (singleWarrior == false)
            {
             
            }
            else
            {
                inSingleWarriorPlace = true;
            }

        }


    }

    private void OnPlayerExit(Collider collider)
    {
        if (collider == null) return;

        if (singleWarrior == true)
        {
            inSingleWarriorPlace = false;
        }

        if (collider.name.Equals("Player"))
        {
            doDeactive();
        }

    }

    private void playAnimation(string animName)
    {

        if (animator == null) return;
        animator.CrossFade(animName, 0.1f, 0, 0);

    }


    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    private int getTrackIndex()
    {

        if (this.transform.position.x == -6.0f)
        {
            return -1;
        }
        else if (this.transform.position.x == 0f)
        {
            return 0;
        }
        else if (this.transform.position.x == 6.0f)
        {
            return 1;
        }

        return 0;

    }


    public void doActive()
    {
        if (animator != null)
        {
            animator.Play("characterObstacleIdle", 0, 0);
            animator.enabled = true;
        }


        if (obstacleCollider != null)
        {
            obstacleCollider.enabled = true;
        }

    }



    public void doDeactive()
    {
        active = false;
        tooCloseTrig = false;
        karateMoveTrig = false;
        inKarateMove = false;
        lastWarrior = false;

        if (this.isActiveAndEnabled == true)
        {
            StartCoroutine(animatorDisabler());
        }


    }

    private IEnumerator animatorDisabler()
    {

        yield return new WaitForSeconds(0.5f);
        if (animator != null)
        {
            animator.enabled = false;
        }

    }


    // Combat Zone
    private static bool inKarateMove;
    private static int beatenEnemys;

    private static TweenPosition hudCombatZoneAnimation;
    private static Text lblBeatenEnemys;
    private static TweenScale lblBeatenEnemysAnim;

    private static Coroutine kareteMoveOverCoroutine;



    // Calling from controller script
    public static void onClick(Vector3 pos)
    {

      
        bool inWarriorHood = false;
        RaycastHit[] hitObjects;

        Vector3 rayLookingOrigin = new Vector3(pos.x, pos.y + 150.0f, pos.z);

        hitObjects = Physics.RaycastAll(rayLookingOrigin, Vector3.down, 150.0f);

        // Debug.DrawRay(rayLookingOrigin, Vector3.down * 150, Color.yellow, 20);

        if (hitObjects != null)
        {
            foreach (RaycastHit hitObject in hitObjects)
            {
                if (hitObject.collider.gameObject != null)
                {
                    if (hitObject.collider.gameObject.transform.parent != null)
                    {
                        if (hitObject.collider.gameObject.transform.parent.gameObject.GetComponent<CharacterObstacle>() != null)
                        {
                            inWarriorHood = true;
                        }
                    }
                }
            }
        }

        if (inWarriorHood == true)
        {
            makeKarateMove();
        }

    }

    public static void makeKarateMove()
    {

        string selectedMove = "";
        string selectedAudio = "";

        if (playerAnimator == null) playerAnimator = GameObject.Find("playerController").GetComponent<Animator>();

        int moveIndex = UnityEngine.Random.Range(0, 3);
        switch (moveIndex)
        {
            case 0:
                selectedMove = "kickFlying";
                selectedAudio = "PlayerKarateSingle" + UnityEngine.Random.Range(1, 5).ToString();
                break;
            case 1:
                selectedMove = "kickFlying";
                selectedAudio = "PlayerKarateSingle" + UnityEngine.Random.Range(1, 5).ToString();
                break;
            case 2:
                selectedMove = "kickHurricane";
                selectedAudio = "PlayerKarateMulti";
                break;
        }


        playerAnimator.Play(selectedMove, 0, 0);
        GameGlobals.Instance.audioController.playSound(selectedAudio, false);

        inKarateMove = true;

        if (kareteMoveOverCoroutine != null)
        {
            //StaticCoroutine.EndCoroutine(kareteMoveOverCoroutine);
        }


    }


    private static IEnumerator inKarateMoveDisabler()
    {
        yield return new WaitForSeconds(0.8f);
        inKarateMove = false;
    }

    public static void hanlePause(bool isPaused)
    {


    }

    public static void handleDeath()
    {
        inKarateMove = false;
        hudCombatZoneAnimation.PlayReverse();
        GameGlobals.Instance.audioController.stopSound("MusicKungfu", true);
    }

    public static void Reset()
    {

        inKarateMove = false;
        GameGlobals.Instance.audioController.stopSound("MusicKungfu", true);

    }

}

