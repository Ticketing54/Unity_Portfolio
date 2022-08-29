using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : MonoBehaviour
{
    public static EffectManager effectManager;
    PoolingManager_Resource effectRes;
    PoolingManager<QuestMark> questMarkRes;
    Dictionary<GameObject, string> runningEffect;
    Dictionary<Npc, QuestMark> runningQuestMark;
    Dictionary<Unit, SpeechBubble> runningSpeechBubble;
    PoolData<SpeechBubble> speechBubblePool;
    
    List<GameObject> clickList;
    Vector3 effectPreset = new Vector3(0f, 0.1f, 0f);
    Vector3 questMarkPreset = new Vector3(0f, 3f, 0f);


    private void Awake()
    {
        if (effectManager == null)
        {
            effectManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        effectRes = new PoolingManager_Resource(this.gameObject);
        questMarkRes = new PoolingManager<QuestMark>(this.gameObject);

        runningEffect       = new Dictionary<GameObject, string>();
        runningQuestMark    = new Dictionary<Npc, QuestMark>();
        runningSpeechBubble = new Dictionary<Unit, SpeechBubble>();
        clickList = new List<GameObject>();

        
    }
    private void Start()
    {
        GameManager.gameManager.moveSceneReset += ClickEffectReset;
        GameManager.gameManager.moveSceneReset += ResetSpeechBubble;
    }

    void ResetSpeechBubble()
    {
        List<Unit> runningSpeechList = new List<Unit>(runningSpeechBubble.Keys);
        for (int i = 0; i < runningSpeechList.Count; i++)
        {
            speechBubblePool.Add(runningSpeechBubble[runningSpeechList[i]]);
            runningSpeechBubble.Remove(runningSpeechList[i]);            
        }
    }
    public void AddSpeechBubbleResource(GameObject _bubble, TextMeshPro _text)
    {   
        SpeechBubble speechBubble = _text.gameObject.AddComponent<SpeechBubble>();
        speechBubble.SetSpeechBubble(_bubble, _text);
        speechBubblePool = new PoolData<SpeechBubble>(speechBubble,this.gameObject,"SpeechBubble");
    }
    public void SpeechBubble(Unit _target, string _text)
    {
        if (runningSpeechBubble.ContainsKey(_target))
        {
            speechBubblePool.Add(runningSpeechBubble[_target]);
            runningSpeechBubble.Remove(_target);
        }

        SpeechBubble bubble = speechBubblePool.GetData();
        bubble.TextingSpeechBubble(_target, _text, PushBubble);
        
    }
    void PushBubble(Unit _unit,SpeechBubble _bubble)
    {
        runningSpeechBubble.Remove(_unit);
        speechBubblePool.Add(_bubble);
    }
    
    public void UpdateQuestMark(Npc _npc,QUESTSTATE _state)
    {
        QuestMark mark;
       if(runningQuestMark.TryGetValue(_npc, out mark))
        {
            runningQuestMark.Remove(_npc);
            questMarkRes.Add(mark.MarkType.ToString(),mark);

            CreateQuestMark(_npc, _state);
        }
        else
        {
            CreateQuestMark(_npc, _state);

        }
    }
    public void RemoveQuestMark(Npc _npc)
    {
        
        if (runningQuestMark.ContainsKey(_npc))
        {
            QuestMark questMark = runningQuestMark[_npc];
            runningQuestMark.Remove(_npc);
            questMarkRes.Add(questMark.MarkType.ToString(), questMark);
        }       
    }
    void CreateQuestMark(Npc _npc,QUESTSTATE _state)
    {        
        QuestMark questmark;
        switch (_state)
        {
            case QUESTSTATE.NONE:
                {                    
                    questmark = questMarkRes.GetData("STARTABLE");
                    questmark.MarkType = QUESTMARKTYPE.STARTABLE;                    
                    questmark.gameObject.transform.SetParent(_npc.transform);                    
                    questmark.gameObject.transform.localPosition = new Vector3(0,_npc.Nick_YPos + 0.2f, 0);
                    runningQuestMark.Add(_npc, questmark);
                }
                break;
            case QUESTSTATE.PLAYING:
                {                    
                    questmark = questMarkRes.GetData("NOTCOMPLETE");
                    questmark.MarkType = QUESTMARKTYPE.NOTCOMPLETE;                    
                    questmark.gameObject.transform.SetParent(_npc.transform);
                    questmark.gameObject.transform.localPosition = new Vector3(0, _npc.Nick_YPos+0.2f, 0);
                    runningQuestMark.Add(_npc, questmark);
                }                
                break;
            case QUESTSTATE.COMPLETE:                
                questmark = questMarkRes.GetData("COMPLETE");
                questmark.MarkType = QUESTMARKTYPE.COMPLETE;
                questmark.gameObject.transform.SetParent(_npc.transform);
                questmark.gameObject.transform.localPosition = new Vector3(0, _npc.Nick_YPos + 0.2f, 0);
                runningQuestMark.Add(_npc, questmark);
                break;
            case QUESTSTATE.DONE:
                break;
            default:
                Debug.Log("퀘스트 마크 생성 오류 state 문제");
                return;
        }
    }
    

    #region ClickEffect
    public void BasicEffectAdd()
    {
        GameObject clickNomal = ResourceManager.resource.GetEffect("ClickNomal");
        clickNomal.transform.SetParent(this.transform);
        clickNomal.gameObject.SetActive(false);
        GameObject clickEnermy = ResourceManager.resource.GetEffect("ClickEnermy");
        clickEnermy.transform.SetParent(this.transform);
        clickEnermy.gameObject.SetActive(false);
        GameObject clickFriend = ResourceManager.resource.GetEffect("ClickFriend");
        clickFriend.transform.SetParent(this.transform);
        clickFriend.gameObject.SetActive(false);        
        clickList.Add(clickNomal);
        clickList.Add(clickEnermy);
        clickList.Add(clickFriend);


        QuestMark questMarkEx = ResourceManager.resource.GetEffect("EXCLAMATION").AddComponent<QuestMark>();
        QuestMark questMarkQu = ResourceManager.resource.GetEffect("QUESTION").AddComponent<QuestMark>();
        QuestMark questMarkNotQu = ResourceManager.resource.GetEffect("QUESTIONNOTCLEAR").AddComponent<QuestMark>();
        questMarkEx.MarkType = QUESTMARKTYPE.STARTABLE;
        questMarkNotQu.MarkType = QUESTMARKTYPE.NOTCOMPLETE;
        questMarkQu.MarkType = QUESTMARKTYPE.COMPLETE;        
        questMarkRes.Add("STARTABLE", questMarkEx);
        questMarkRes.Add("COMPLETE", questMarkQu);        
        questMarkRes.Add("NOTCOMPLETE", questMarkNotQu);        
    }
    public void ClickEffectReset()               // 맵 이동 시 리셋
    {
        foreach(GameObject click in clickList)
        {
            if (click.gameObject.activeSelf == true)
            {
                click.gameObject.transform.SetParent(this.transform);
                click.gameObject.SetActive(false);
            }                
        }

        List<Npc> npcList = new List<Npc>(runningQuestMark.Keys);
        for (int i = 0; i < npcList.Count; i++)
        {
            if(runningQuestMark[npcList[i]].MarkType == QUESTMARKTYPE.STARTABLE)
            {
                questMarkRes.Add("EXCLAMATION", runningQuestMark[npcList[i]]);
            }
            else
            {
                questMarkRes.Add("QUESTION", runningQuestMark[npcList[i]]);
            }
            runningQuestMark.Remove(npcList[i]);
        }
    }

    public void ClickEffectOn(CLICKEFFECT _ClickEffect, Transform _Target)                      // 대상이 있음
    {
        GameObject clickEffect = ClickEffectCheck(_ClickEffect);
        clickEffect.transform.SetParent(_Target);
        clickEffect.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        clickEffect.gameObject.transform.localPosition = effectPreset;
    }
    public void ClickEffectOn(Vector3 _Pos)                           // 바닥
    {
        GameObject clickEffect = ClickEffectCheck(CLICKEFFECT.NORMAL);
        clickEffect.gameObject.transform.position = _Pos + effectPreset;
    }
    GameObject ClickEffectCheck(CLICKEFFECT _ClickEffect)                                       // 클릭 대상에 따라 하나만 켜져있게함
    {
        GameObject SeletClickEffect = clickList[(int)_ClickEffect];

        foreach (GameObject one in clickList)
        {
            if (one == SeletClickEffect)
            {
                if (one.gameObject.activeSelf == false)
                {
                    one.gameObject.SetActive(true);
                }
            }
            else
            {
                if (one.gameObject.activeSelf == true)
                {
                    one.gameObject.transform.SetParent(this.transform);
                    one.gameObject.SetActive(false);                    
                }                
            }
        }

        return SeletClickEffect;
    }
    #endregion
    
   
    public GameObject GetEffect(string _effectName)                                // Effect 이름별로 꺼내오기
    {
        GameObject effectObj = effectRes.GetData(_effectName);
        if (effectObj == null)
        {
            GameObject newEffect = ResourceManager.resource.GetEffect(_effectName);         // 리소스 수정후 수정할 것
            AddEffect(_effectName,newEffect);
            return GetEffect(_effectName);
        }
        else
        {            
            return effectObj;
        }
    }
    public GameObject GetBuffEffect(string _effectName)
    {
        GameObject effectObj = effectRes.GetData(_effectName);
        if(effectObj == null)
        {
            GameObject newBuffEffect = ResourceManager.resource.GetEffect(_effectName);
            effectRes.Add(_effectName, newBuffEffect);
            return GetBuffEffect(_effectName);
        }
        else
        {
            return effectObj;
        }
    }
    public void PushBuffEffect(string _effectName, GameObject _effect)
    {
        effectRes.Add(_effectName, _effect);
    }
    void AddEffect(string _effectName,GameObject _effect)                                       // Effect 사용 후 저장
    {
        string effectName;
        if(runningEffect.TryGetValue(_effect,out effectName))
        {
            runningEffect.Remove(_effect);
            effectRes.Add(effectName, _effect);
            return;
        }
        effectRes.Add(_effectName, _effect);
    }
    void RunningEffectAdd(GameObject _effect)
    {
        string effectName;
        if (runningEffect.TryGetValue(_effect, out effectName))
        {
            runningEffect.Remove(_effect);
            effectRes.Add(effectName, _effect);
        }
        else
        {
            Debug.Log("runningDic 에 없음");
        }
    }

    public void LoadEffect(string _effectName, Vector3 _pos, float _holdingtime)     // 이펙트 사용
    {
        GameObject Effect = GetEffect(_effectName);
        StartCoroutine(TurnoffEffect(_effectName, Effect, _holdingtime));
    }

    IEnumerator TurnoffEffect(string _effectName,GameObject _effect, float _holdingtime)
    {
        yield return new WaitForSeconds(_holdingtime);
        AddEffect(_effectName,_effect);
    }

}
