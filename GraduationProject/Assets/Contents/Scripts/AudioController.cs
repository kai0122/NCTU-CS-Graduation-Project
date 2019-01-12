using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AudioController : MonoBehaviour 
{
   
    public GameObject ambientsRoot;
    public GameObject musicsRoot;
    public GameObject shortsRoot;
    public GameObject playerEffectsRoot;
    public GameObject villainEffectsRoot;
    public GameObject powerupsRoot;
    public GameObject UiEffetcsRoot;

    public class AudioItem 
    {
        public AudioSource audioSource;
        public float defaultVolume;
        public float lastPos;
    }

    private List<AudioItem> ambients = new List<AudioItem>();
    private List<AudioItem> musics = new List<AudioItem>();
    private List<AudioItem> shorts = new List<AudioItem>();
    private List<AudioItem> playerEffects = new List<AudioItem>();
    private List<AudioItem> villainEffects = new List<AudioItem>();
    private List<AudioItem> powerups = new List<AudioItem>();
    private List<AudioItem> UiEffetcs = new List<AudioItem>();

    public enum Fade { In = 0, Out = 1 };
    private float fadeTime = 0.5f;


    private bool footStepsTrig;
    AudioItem footSteps;

    private void Awake()
    {

        if (ambientsRoot != null)   { fillAudioSources(ambientsRoot.GetComponents<AudioSource>(), ambients); }
        if (musicsRoot != null)     { fillAudioSources(musicsRoot.GetComponents<AudioSource>(), musics); }
        if (shortsRoot != null)     { fillAudioSources(shortsRoot.GetComponents<AudioSource>(), shorts); }
        if (playerEffectsRoot != null) { fillAudioSources(playerEffectsRoot.GetComponents<AudioSource>(), playerEffects); }
        if (villainEffectsRoot != null) { fillAudioSources(villainEffectsRoot.GetComponents<AudioSource>(), villainEffects); }
        if (powerupsRoot != null) { fillAudioSources(powerupsRoot.GetComponents<AudioSource>(), powerups); }
        if (UiEffetcsRoot != null) { fillAudioSources(UiEffetcsRoot.GetComponents<AudioSource>(), UiEffetcs); }
  
        footSteps = getAudoClip("PlayerFootSteps");

    }


    public AudioItem getAudoClip(string clipName)
    {

        AudioItem clip = null;
        foreach (AudioItem item in ambients) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }
        foreach (AudioItem item in musics) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }
        foreach (AudioItem item in shorts) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }
        foreach (AudioItem item in playerEffects) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }
        foreach (AudioItem item in villainEffects) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }
        foreach (AudioItem item in powerups) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }
        foreach (AudioItem item in UiEffetcs) { if (item.audioSource.clip.name.Equals(clipName)) { clip = item; } }

        return clip;
    }

    public void stopAllSounds()
    {
  

        foreach (AudioItem item in ambients) {item.audioSource.Stop();}
        foreach (AudioItem item in musics) {item.audioSource.Stop();}
        foreach (AudioItem item in shorts) { item.audioSource.Stop(); }
        foreach (AudioItem item in playerEffects) { item.audioSource.Stop();}
        foreach (AudioItem item in villainEffects) { item.audioSource.Stop();}
        foreach (AudioItem item in powerups) { item.audioSource.Stop(); }
        foreach (AudioItem item in UiEffetcs) { item.audioSource.Stop();}
    }

    private void fillAudioSources(AudioSource[] sourcesIN, List<AudioItem> sourcesTO)
    {
        if (sourcesIN != null)
        {
            foreach (AudioSource item in sourcesIN)
            {
                AudioItem nItem = new AudioItem();
                nItem.audioSource = item;
                nItem.defaultVolume = item.volume;
                sourcesTO.Add(nItem);
            }
        }
    }

    public void muteGame(bool isMuted)
    {

        switch (isMuted)
        {
            case false:

                playSound("AmbianceJungle", true);

                if (GameGlobals.Instance.currentGameState.Equals("OnOpeningScene"))
                {
                    playSound("MusicStart", true);
                }
                else if (GameGlobals.Instance.currentGameState.Equals("onPauseGame"))
                {
                    playSound("MusicPause", true);
                }


                break;
            case true:

                stopSound("AmbianceJungle", true);

                if (GameGlobals.Instance.currentGameState.Equals("OnOpeningScene"))
                {
                    stopSound("MusicStart", true);
                }
                else if (GameGlobals.Instance.currentGameState.Equals("onPauseGame"))
                {
                    stopSound("MusicPause", true);
                }

                break;
        }
    
    }

    private void FixedUpdate()
    {
        updateFootSteps();
    }

    private void updateFootSteps()
    {

        if (footStepsTrig == false && GameGlobals.Instance.controller.enabled == true)
        {
            footStepsTrig = true;
            playSound("PlayerFootSteps", false);
        }

        if (footStepsTrig == true && GameGlobals.Instance.controller.enabled == false)
        {
            footStepsTrig = false;
            stopSound("PlayerFootSteps", false);
        }

        if (GameGlobals.Instance.controller.isJumping == false)
        {
            if (footSteps != null) { footSteps.audioSource.volume = footSteps.defaultVolume; }
        }
        else
        {
            if (footSteps != null) { footSteps.audioSource.volume = 0; }
        }

    }

    public void playSound(string clipName, bool isFade)
    {

        if (PlayerPrefs.GetInt("audio", 1) == 0) return;

        AudioItem clip = getAudoClip(clipName);

        if (clip == null)
        {
            Debug.Log("Audio clip not found:" + clipName);
            return;
        }

        clip.audioSource.volume = clip.defaultVolume;
  
        if (isFade == true)
        {
            StartCoroutine(FadeAudio(clip, fadeTime, Fade.In));
        }

        clip.audioSource.Play();

    }

    public void playSoundDelayed(string clipName, bool isFade,float delay)
    {
        StartCoroutine(delayedPlayJob(clipName, isFade, delay));
    }
    private IEnumerator delayedPlayJob(string clipName, bool isFade, float delay)
    {
        yield return new WaitForSeconds(delay);
        playSound(clipName, isFade);
        yield break;
    }

    public void playSoundPitched(string clipName, float pitch)
    {
        if (PlayerPrefs.GetInt("audio", 1) == 0) return;

        AudioItem clip = getAudoClip(clipName);

        if (clip == null)
        {
            Debug.Log("Audio clip not found:" + clipName);
            return;
        }

        clip.audioSource.volume = clip.defaultVolume;
     
        clip.audioSource.pitch = pitch;
        clip.audioSource.Play();

    }



    public void stopSound(string clipName, bool isFade)
    {
        AudioItem clip = getAudoClip(clipName);

        if (clip == null)
        {
            Debug.Log("Audio clip not found:" + clipName);
            return;
        }

        clip.lastPos = clip.audioSource.time;

        if (isFade == true)
        {
            StartCoroutine(FadeAudio(clip, fadeTime, Fade.Out));
        }
        else
        {
            clip.audioSource.Stop();
        }

    }

    public void pauseSound(string clipName)
    {
        AudioItem clip = getAudoClip(clipName);

        if (clip == null)
        {
            Debug.Log("Audio clip not found:" + clipName);
            return;
        }
        clip.lastPos = clip.audioSource.time;
        clip.audioSource.Pause();
    }


    public void handlePause(bool isPaused)
    {

        switch (isPaused)
        {
            case false:

                stopSound("MusicPause", false);

                if (GameGlobals.Instance.isInCriticalMode() == true)
                {
                    playSound("MusicStumple", true);
                    playSound("VillainStumple", true);
                }

                returnToMainTheme();

                break;
            case true:

                stopSound("MusicGameLoop", false);
                stopSound("MusicStumple", false);
                stopSound("VillainStumple", false);
                stopSound("MusicKungfu", false);

                playSound("MusicPause", false);

                break;
        }
    
    }

    public void returnToMainTheme()
    {

       playSound("MusicGameLoop", true);
       
    }

    public void playRandomOuch()
    {
        playSound("PlayerOuch" + UnityEngine.Random.Range(1, 4).ToString(), false);
    }

    IEnumerator FadeAudio(AudioItem audio, float timer, Fade fadeType)
    {

        float currentTime = 0;

        if (fadeType == Fade.In)
        {
            audio.audioSource.volume = 0;
        }
        if (fadeType == Fade.Out)
        {
            audio.audioSource.volume = audio.defaultVolume;
        }

        while (currentTime <= timer)
        {
            currentTime += Time.deltaTime;

            float peakValue = audio.defaultVolume / timer * currentTime;
            float reversePeakValue = audio.defaultVolume - peakValue;

            if (fadeType == Fade.In)
            {
                audio.audioSource.volume = peakValue;
            }
            else
            {
                audio.audioSource.volume = reversePeakValue;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (fadeType == Fade.In)
        {
            audio.audioSource.volume = audio.defaultVolume;
        }
        if (fadeType == Fade.Out)
        {
            audio.audioSource.volume = 0;
            audio.audioSource.Stop();
        }

    }


}

