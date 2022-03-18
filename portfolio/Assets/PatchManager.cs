using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class PatchManager : MonoBehaviour
{
    public static PatchManager patchManager;
    AsyncOperationHandle handle;
   
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
                    UIManager.uimanager.CloseUpdateMessage();
                    LoadingSceneController.Instance.LoadScene("Main");
                }
                else
                {
                    UIManager.uimanager.OpenUpdateMessage(handle.Result);
                }
            };
    }

    public void PatchResource()
    {
        handle = Addressables.DownloadDependenciesAsync("Patch", true);
        StartCoroutine(UpdateUi());

        handle.Completed += (AsyncOperationHandle down) =>
        {
            handle = down;
            UIManager.uimanager.CloseUpdateMessage();
            LoadingSceneController.instance.LoadScene("Main");
        };
    }

    IEnumerator UpdateUi()
    {
        while (true)
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                yield break;
            }


            UIManager.uimanager.updatePatchMessage(handle.PercentComplete);
            yield return null;
        }
    }

    
   
    
}
