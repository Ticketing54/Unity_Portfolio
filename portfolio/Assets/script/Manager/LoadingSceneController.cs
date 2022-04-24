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
    string loadScenenName = string.Empty;
    SceneInstance prevScene;
    public bool resourceSetting = false;
        

    public void LoadScene(string sceneName)
    {        
        gameObject.SetActive(true);
        loadSceneName = sceneName;
        Fade(true);
        
        
        StartCoroutine(LoadSceneProcess());        
        Addressables.LoadSceneAsync(loadSceneName+ "Scene").Completed += OnSceneLoaded;       

    }
    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {

        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            Addressables.LoadSceneAsync(loadSceneName + "Scene").Completed -= OnSceneLoaded;
            prevScene = obj.Result;
            ResourceManager.resource.LoadSceneResource(loadSceneName + "Table");
        }
        else
        {
            Debug.Log("씬 로드 실패");
        }        
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        while (true)
        {
            if (progressBar.fillAmount < 0.9f)
            {
                progressBar.fillAmount += Time.deltaTime*0.5f;
            }
            else
            {
                if(resourceSetting == true)
                {
                    CameraManager.cameraManager.CameraTargetOnCharacter();                    
                    progressBar.fillAmount = 1;
                    resourceSetting=false;
                    break;
                }

            }
            
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
