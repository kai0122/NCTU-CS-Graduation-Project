using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class AnimatorHelper : MonoBehaviour {

 

    Animator animator;

    [Serializable]
    public struct OnAnimationFinishDelegate
    {
        public string animationName;
        public UnityEvent onAnimationFinished;
    }

    [SerializeField]
    public OnAnimationFinishDelegate[] onAnimationFinished;
    

    
	// Use this for initialization
	void Awake ()
    {

        animator = GetComponent<Animator>();

    }


    public void play(string animation)
    {
        if (animator == null) return;
        animator.Play(animation);

        for (int i = 0; i < onAnimationFinished.Length; i++)
        {
            if (onAnimationFinished[i].animationName.Equals(animation))
            {
                StartCoroutine(WaitForAnimation(animation));
            }
        }
    }

    public void playRebind(string animation)
    {
        if (animator == null) return;

        animator.Rebind();
        animator.Play(animation);

        for (int i = 0; i < onAnimationFinished.Length; i++)
        {
            if (onAnimationFinished[i].animationName.Equals(animation))
            {
                StartCoroutine(WaitForAnimation(animation));
            }
        }
    }


    private IEnumerator WaitForAnimation(string animation)
    {

        float animLenght = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;    
        for (int i = 0; i < ac.animationClips.Length; i++)                 
        {
            if (ac.animationClips[i].name == animation)        
            {
                animLenght = ac.animationClips[i].length;
            }
        }

        yield return new WaitForSeconds(animLenght);

        if (onAnimationFinished != null)
        {
            for (int i = 0; i < onAnimationFinished.Length; i++)
            {
                if (onAnimationFinished[i].onAnimationFinished != null)
                {
                    onAnimationFinished[i].onAnimationFinished.Invoke();
                }
            }

            //onAnimationFinished.Invoke();
        }
        
    }


}
