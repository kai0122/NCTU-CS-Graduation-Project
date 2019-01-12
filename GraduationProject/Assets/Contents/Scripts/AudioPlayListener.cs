using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioPlayListener : MonoBehaviour {



    public AudioSource audioSource;
    public UnityEvent onAudioPlayingFinished;

 
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            this.enabled = false;
            if (onAudioPlayingFinished != null)
            {
                onAudioPlayingFinished.Invoke();
            }
        }
    }





}
