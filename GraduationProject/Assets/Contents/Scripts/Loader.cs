using UnityEngine;
using System.Collections;
using ThunderEvents;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    private AsyncOperation operation;

    public GameObject blackPanel;
    public Image progressBar;

    public ThunderEventList loaderState;
    public float loaderDelay;

   // public UIProgressBar loadingProgress;

   
	// Use this for initialization
	void Start () {

        
        blackPanel.SetActive(true);
        Invoke("executeLoaderStateEcents", loaderDelay);
    }


    private void executeLoaderStateEcents()
    {
        ThunderEvent.Execute(loaderState.List);
    }

    public void loadGame()
    {
        StartCoroutine(LevelCoroutine("Main"));
    }


    IEnumerator LevelCoroutine(System.String nomScene)
    {
       
        AsyncOperation async = SceneManager.LoadSceneAsync(nomScene);
        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            progressBar.fillAmount = async.progress / 0.9f; //Async progress returns always 0 here    
            yield return null;

        }
    }



}
