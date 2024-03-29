﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Timeline;
using UnityEngine.Playables;
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
    
    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    Image progressBar; 
    
    
    
    float loadingPercent = 0f;
    Coroutine coUpdatePercent;
    
    public void LoadScene(string _sceneName,MAPTYPE _type,string _cutScene = "")
    {        
        gameObject.SetActive(true);          
        StartCoroutine(CoLoadScene(_sceneName,_type,_cutScene));        
    }
    IEnumerator CoLoadScene(string _sceneName,MAPTYPE _type,string _cutScene)
    {
        yield return Fade(true);


        AsyncOperationHandle<SceneInstance> loadscene = Addressables.LoadSceneAsync(_sceneName + "Scene");
        yield return loadscene;        

        yield return StartCoroutine(ResourceManager.resource.CoLoadSceneResource(_sceneName));

        if (!string.IsNullOrEmpty(_cutScene))
        {
            yield return StartCoroutine(CoLoadCutScene(_cutScene));
        }

        if(_type == MAPTYPE.BOSS)
        {
            TutorialBoss boss = GameObject.FindGameObjectWithTag("Monster").GetComponent<TutorialBoss>();
            boss.MoveStart();
        }

        CameraManager.cameraManager.CameraTargetOnCharacter();
        SoundManager.soundmanager.BgmPlay(_sceneName + "BGM");
        GameManager.gameManager.character.SetPosition();
        UIManager.uimanager.OnBaseUI();
        GameManager.gameManager.SetMapInfo(_sceneName);
        yield return Fade(false);
        gameObject.SetActive(false);
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
    }
    
  
    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {   
            timer += Time.deltaTime ;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
            yield return null;
        }        
    }
    public void LoadCutScene(string _cutName)
    {
        gameObject.SetActive(true);
        StartCoroutine(CoLoadCutScene_dialog(_cutName));
    }
    public IEnumerator CoLoadCutScene(string _cutName)
    {   
        gameObject.SetActive(true);
        yield return StartCoroutine(Fade(true));
        CameraManager.cameraManager.enabled = false;
        UIManager.uimanager.CanvasEnabled(false);
        yield return StartCoroutine(Fade(false));

        AsyncOperationHandle<SceneInstance> cutScene = Addressables.LoadSceneAsync(_cutName, LoadSceneMode.Additive);
        yield return cutScene;        
        
        GameObject timelineObj = GameObject.Find("Timeline");
        PlayableDirector director = timelineObj.GetComponent<PlayableDirector>();
        director.playOnAwake = false;
        yield return StartCoroutine(Fade(false));


        yield return CheckDone(director);
        yield return StartCoroutine(Fade(true));
        AsyncOperationHandle<SceneInstance> quitCutScene =Addressables.UnloadSceneAsync(cutScene);
        yield return quitCutScene;


        
        CameraManager.cameraManager.enabled = true;
        UIManager.uimanager.CanvasEnabled(true);        
    }
    public IEnumerator CoLoadCutScene_dialog(string _cutName)
    {
        gameObject.SetActive(true);
        yield return StartCoroutine(Fade(true));
        CameraManager.cameraManager.enabled = false;
        UIManager.uimanager.CanvasEnabled(false);
        

        AsyncOperationHandle<SceneInstance> cutScene = Addressables.LoadSceneAsync(_cutName, LoadSceneMode.Additive);
        yield return cutScene;

        GameObject timelineObj = GameObject.Find("Timeline");
        PlayableDirector director = timelineObj.GetComponent<PlayableDirector>();
        director.playOnAwake = false;
        yield return StartCoroutine(Fade(false));


        yield return CheckDone(director);
        yield return StartCoroutine(Fade(true));
        AsyncOperationHandle<SceneInstance> quitCutScene = Addressables.UnloadSceneAsync(cutScene);
        yield return quitCutScene;


        
        CameraManager.cameraManager.enabled = true;
        UIManager.uimanager.CanvasEnabled(true);

        yield return StartCoroutine(Fade(false));
        gameObject.SetActive(false);

    }
    IEnumerator CheckDone(PlayableDirector _director)
    {
        _director.Play();
        while (true)
        {
            if(_director.gameObject.activeSelf == false)
            {   

                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
