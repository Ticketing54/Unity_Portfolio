using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;

public class LoadingSceneController : MonoBehaviour
{
    
    public static LoadingSceneController instance;
    
    public static LoadingSceneController Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Instantiate(Resources.Load<LoadingSceneController>("Loading")); 
                }
            }
            return instance;
        }
    }


    SceneInstance prevScene;


    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    public float tableProgress = 0;
    public float imageProgress = 0;

    

    

    AsyncOperationHandle loadAddressable;
    
    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    Image progressBar;

    private string loadSceneName;    
    
    
    
    float loadingPercent = 0f;
    Coroutine coUpdatePercent;
    
    public void LoadScene(string sceneName)
    {        
        gameObject.SetActive(true);
        loadSceneName = sceneName;
        Fade(true);
        AsyncOperationHandle<SceneInstance> Op = Addressables.LoadSceneAsync(loadSceneName + "Scene");        
        Op.Completed += OnSceneLoaded;
    }
    
    void OnSceneLoaded (AsyncOperationHandle<SceneInstance> _handle)
    {   
        ResourceManager.resource.LoadSceneResource(loadSceneName);
    }

    public void LoadingPercent(float _percent)
    {
        if(coUpdatePercent != null)
        {
            StopCoroutine(coUpdatePercent);
        }       

        coUpdatePercent = StartCoroutine(LoadSceneProcess(_percent));
    }

    private IEnumerator LoadSceneProcess(float _Percent)
    {
        float Timer = 0f;
        while (progressBar.fillAmount < 1)
        {

            Timer += Time.deltaTime;
            loadingPercent = Mathf.Lerp(loadingPercent, _Percent, Timer);

            progressBar.fillAmount = loadingPercent;           

            yield return null;
        }        
        
        StartCoroutine(Fade(false));
    }

    //private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{   
    //    if(arg0.name == loadSceneName)
    //    {

    //        StartCoroutine(Fade(false));            
    //        SceneManager.sceneLoaded -= OnSceneLoaded;      
    //    }
    //}
    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }
        if (!isFadeIn)
        {
            
            gameObject.SetActive(false);
        }
    }
}
