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

    private void OnEnable()
    {
        UIManager.uimanager.updateBuffUi += OnBuffIUiSetting;
    }
    private void OnDisable()
    {
        UIManager.uimanager.updateBuffUi -= OnBuffIUiSetting;
        ClearUIBuff();

    }


    private void Awake()
    {
        buffPool = new PoolData<BuffImage>(exampleBuffImage,this.gameObject,"Buff");
        runningBuffDic = new Dictionary<string, BuffImage>();
    }

    public void OnBuffIUiSetting(string _buffName, float _fillAmount,int _holdTime)
    {
        BuffImage buf;
        if(runningBuffDic.TryGetValue(_buffName,out buf))
        {
            buf.OnBuffImage(_buffName);
            buf.SetBuff(_fillAmount, _holdTime);
        }
        else
        {
            buf = buffPool.GetData();
            buf.transform.SetParent(bufLine.content.transform);
            buf.transform.localPosition = Vector3.zero;
            buf.transform.localScale = new Vector3(1f, 1f, 1f);

            runningBuffDic.Add(_buffName, buf);
            buf.OnBuffImage(_buffName);
            buf.SetBuff(_fillAmount, _holdTime);
        }
    }

    public void OffBuffUISetting(string _buffName)
    {
        BuffImage buf;
        if(runningBuffDic.TryGetValue(_buffName,out buf))
        {
            buf.Clear();
            buffPool.Add(buf);
            runningBuffDic.Remove(_buffName);
        }
        else
        {
            Debug.LogError("없는 BuffUi를 off 하려합니다.");
        }
    }

    void ClearUIBuff()
    {
        List<string> runningDickey = new List<string>(runningBuffDic.Keys);

        for (int i = 0; i < runningDickey.Count; i++)
        {
            OffBuffUISetting(runningDickey[i]);
        }
    }
    
}
