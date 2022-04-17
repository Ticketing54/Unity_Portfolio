using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PatchUi : MonoBehaviour
{
    [SerializeField]
    GameObject downloadMessage;
    [SerializeField]
    TextMeshProUGUI fileSizeMessage;
    [SerializeField]
    Image progressBar;
   
    public void SetUpdateMessage(long _DownloadFileSize)
    {
        downloadMessage.SetActive(true);
        fileSizeMessage.text = string.Concat(_DownloadFileSize, "  byte의  다운  받을  파일이  있습니다.");
    }
    public void YesButton()
    {
        downloadMessage.gameObject.SetActive(false);
        PatchManager.patchManager.PatchResource();
    }
    public void NoButton()
    {
        Application.Quit();
    }
}
