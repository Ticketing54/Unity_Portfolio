using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
public class PatchUi : MonoBehaviour
{
    [SerializeField]
    GameObject downloadMessage;
    [SerializeField]
    TextMeshProUGUI fileSizeMessage;
    [SerializeField]
    GameObject downloadProgress;
    [SerializeField]
    Image progressBar;
    [SerializeField]
    TextMeshProUGUI progressText;
    [SerializeField]
    CanvasGroup patchCanvasGroup;



    private void Start()
    {
        PatchManager.patchManager.openPatchUi += SetUpdateMessage;
        PatchManager.patchManager.closePatchUi += ()=> { downloadMessage.SetActive(false); };
        PatchManager.patchManager.updatePatchUi += UpdateDownload;
        ResourceManager.resource.closePatchUi += ClosePatchUi;
    }

    public void SetUpdateMessage(long _DownloadFileSize)
    {
        downloadMessage.SetActive(true);
        fileSizeMessage.text = string.Concat(_DownloadFileSize, "  byte의  다운  받을  파일이  있습니다.");
    }
    public void YesButton()
    {
        fileSizeMessage.gameObject.SetActive(false);
        PatchManager.patchManager.PatchResource();
    }
    public void NoButton()
    {
        Application.Quit();
    }
    
    public void UpdateDownload(AsyncOperationHandle _handle)
    {
        downloadProgress.gameObject.SetActive(true);
        StartCoroutine(Test(_handle));        
    }
    IEnumerator Test(AsyncOperationHandle _test)
    {
        while (!_test.IsDone)
        {
            DownloadStatus downstatus = _test.GetDownloadStatus();
            Debug.Log(downstatus.ToString());            
            progressBar.fillAmount = downstatus.Percent;            
            progressText.text =downstatus.DownloadedBytes+"Byte / "+downstatus.TotalBytes.ToString()+"Byte";   
            yield return null;
        }
        downloadProgress.gameObject.SetActive(false);
    }
    public void ClosePatchUi()
    {
        StartCoroutine(CoFadeoutPatch());
    }
    IEnumerator CoFadeoutPatch()
    {
        while (true)
        {
            patchCanvasGroup.alpha -= Time.unscaledDeltaTime * 0.5f;
            if (patchCanvasGroup.alpha <= 0)
            {
                break;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
