using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buff : MonoBehaviour
{
    PoolData<BuffImage> buffPool;
    Dictionary<string, BuffImage> runningBuffDic;

    [SerializeField]
    BuffImage exampleBuffImage;
    [SerializeField]
    ScrollRect bufLine;
    [SerializeField]
    GameObject parent;

    private void OnEnable()
    {
        UIManager.uimanager.AUpdateBuf += UpdateBuf;
        UIManager.uimanager.ARemoveBuf += RemoveBuf;
    }
    private void OnDisable()
    {
        UIManager.uimanager.AUpdateBuf -= UpdateBuf;
        UIManager.uimanager.ARemoveBuf -= RemoveBuf;
        ClearBufImage();
    }


    private void Awake()
    {
        buffPool = new PoolData<BuffImage>(exampleBuffImage, parent, "Buff");
        runningBuffDic = new Dictionary<string, BuffImage>();
    }


    void UpdateBuf(string _key, float _timer, float _duration)
    {
        if (!runningBuffDic.ContainsKey(_key))
        {
            BuffImage newbufImage = buffPool.GetData();
            newbufImage.SetBuffImage(_key);
            newbufImage.transform.SetParent(bufLine.content.transform);
            runningBuffDic.Add(_key, newbufImage);
        }
        BuffImage bufImage = runningBuffDic[_key];
        bufImage.UpdateBuff(_timer, _duration);        
    }
    void RemoveBuf(string _key)
    {
        if (runningBuffDic.ContainsKey(_key))
        {
            BuffImage removeBufImage = runningBuffDic[_key];
            removeBufImage.Clear();
            buffPool.Add(removeBufImage);
            runningBuffDic.Remove(_key);
        }
    }

    void ClearBufImage()
    {
        List<string> runningImageList = new List<string>(runningBuffDic.Keys);
        for (int i = 0; i < runningImageList.Count; i++)
        {
            RemoveBuf(runningImageList[i]);
        }
    }  
}
