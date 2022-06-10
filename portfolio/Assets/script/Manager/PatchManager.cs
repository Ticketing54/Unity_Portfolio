using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using System;

public class PatchManager : MonoBehaviour
{
    public static PatchManager patchManager;
    public Action<long> openPatchUi;
    public Action<AsyncOperationHandle> updatePatchUi;
    public Action closePatchUi;
   
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
        StartCoroutine(CoCheckUpdate());
    }
   
    IEnumerator CoCheckUpdate()
    {
        AsyncOperationHandle<long> sizeCheck = Addressables.GetDownloadSizeAsync("Patch");
        yield return sizeCheck;

        if(sizeCheck.Result == 0)
        {   
            ResourceManager.resource.StartResourceSetting();            
        }
        else
        {
            openPatchUi(sizeCheck.Result);
        }
    }
    public void PatchResource()
    {
        StartCoroutine(CoPatchResource());
    }   

    IEnumerator CoPatchResource()
    {
        AsyncOperationHandle patch = Addressables.DownloadDependenciesAsync("Patch", true);
        updatePatchUi(patch);
        
        yield return patch;
        closePatchUi();
        ResourceManager.resource.StartResourceSetting();
    }
}
