using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PatchManager : MonoBehaviour
{
    public static PatchManager patchManager;    
    bool readytoplay = false;

    public delegate void StartPatch();
    public StartPatch startPatch;
    public bool readyToPlay { get { return readytoplay; } set { readytoplay = value; } }
   
    private void Awake()
    {
        if (patchManager == null)
        {
            patchManager = this;
        }           
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {        
        CheckUpdateResource();
    }
    public void CheckUpdateResource()
    {
        Addressables.GetDownloadSizeAsync("Patch").Completed +=
            (handle) =>
            {
                if (handle.Result == 0)
                {                    
                    StartCoroutine(StartGame());
                    ResourceManager.resource.StartResourceSetting();                    
                }
                else
                {
                    UIManager.uimanager.OpenUpdateMessage(handle.Result);
                }
            };
    }
    IEnumerator StartGame()
    {
        float timer = 0f;        
        while (true)
        {
            if(readyToPlay == true && timer > 3f)
            {
                
                break;
            }
            
            timer += Time.deltaTime;
            yield return null;
        }

        readyToPlay = false;
        LoadSceneMain();
    }
    void LoadSceneMain()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");        
    }

    void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == "Main")
        {
            UIManager.uimanager.ClosePatchUi();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void PatchResource()
    {
        Addressables.DownloadDependenciesAsync("Patch", true).Completed +=
            (download) =>
            {
                UIManager.uimanager.ClosePatchUi();
                StartCoroutine(StartGame());
                ResourceManager.resource.StartResourceSetting();
            };        
    }   
   
   
    
}
