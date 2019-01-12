using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DecorAnimation : MonoBehaviour
{

    public string trackID,animName;
    private Animator animator;
    private GameObject player;

    
    private bool active;

    private static List<DecorAnimation> decorAnimations = new List<DecorAnimation>();


    private void Awake()
    {
        player = GameObject.Find("U_CharacterFront(Clone)");
        animator = this.GetComponent<Animator>();
        animator.enabled = true;
        decorAnimations.Add(this);
      
    }


    private void Start()
    {
       play();
        
    }

    public void play()
    {
        active = true;
        
        if (animator != null)
        {
            animator.enabled = true;
            if (animator.gameObject.activeSelf)
            {
                animator.Play(animName, 0, UnityEngine.Random.Range(0.0f, 3.0f));
            }
        }
    }
    

    public void stop()
    {
        
        active = false;
     
        if (animator != null)
        {
            animator.enabled = false;
        }

    }

    private void FixedUpdate()
    {
        player = GameObject.Find("U_CharacterFront(Clone)");
        if (active == true && player)
        {
            this.transform.LookAt(player.transform);
        }
       

    }


    public static void activeTrackDecors(string trackID,bool active)
    {

        foreach (DecorAnimation decorAnim in decorAnimations)
        {
            if (decorAnim.trackID.ToLower().Equals(trackID.ToLower()))
            {
                if (active == false)
                {
                    decorAnim.stop();
                }
                else
                {
                    decorAnim.play();
                }

               
            }
        }
    
    }

}

