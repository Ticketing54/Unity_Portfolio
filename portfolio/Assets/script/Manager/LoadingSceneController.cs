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
    
    
    
    float loadingPercent = 0f;
    Coroutine coUpdatePercent;
    
    public void LoadScene(string _sceneName)
    {        
        gameObject.SetActive(true);

        StartCoroutine(CoLoadScene(_sceneName));
        
        
        
    }
    IEnumerator CoLoadScene(string _sceneName)
    {
        yield return Fade(true);


        AsyncOperationHandle<SceneInstance> loadscene = Addressables.LoadSceneAsync(_sceneName + "Scene");
        yield return loadscene;

        ResourceManager.resource.LoadSceneResource(_sceneName);
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
        while (progressBar.fillAmount < _Percent)
        {

            Timer += Time.deltaTime;
            loadingPercent = Mathf.Lerp(loadingPercent, _Percent, Timer);

            progressBar.fillAmount = loadingPercent;           

            yield return null;
        }                

        if(_Percent == 1)
        {
            coUpdatePercent = null;

            yield return Fade(false);
            gameObject.SetActive(false);
        }
    }
    
  
    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {   
            timer += Time.unscaledDeltaTime ;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
            yield return null;
        }        
    }
}
