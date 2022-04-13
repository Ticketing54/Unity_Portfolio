using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager effectManager;
    PoolingManager_Resource effectRes;
    PoolingManager<QuestMark> questMarkRes;
    Dictionary<GameObject, string> runningEffect;
    Dictionary<Npc, QuestMark> runningQuestMark;
    

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

        runningEffect = new Dictionary<GameObject, string>();
        runningQuestMark = new Dictionary<Npc, QuestMark>();        
        clickList = new List<GameObject>();

    }
    
    public void UpdateQuestMark(Npc _npc, QUESTMARKTYPE _type,QUESTSTATE _state)
    {
        QuestMark mark;
       if(runningQuestMark.TryGetValue(_npc, out mark))
        {
            if(mark.MarkType == _type)
            {
                if(QUESTSTATE.COMPLETE == _state || QUESTSTATE.NONE == _state)
                {
                    mark.StartingOrComplete();
                }
                else if(QUESTSTATE.DONE == _state)
                {
                    if (_type == QUESTMARKTYPE.EXCLAMATION)
                    {
                        questMarkRes.Add("QUESTION", mark);
                    }
                    else
                    {
                        questMarkRes.Add("EXCLAMATION", mark);
                    }
                }
            }
            else                                                        // 퀘스트 마크 타입이 다른경우
            {
                if(_type == QUESTMARKTYPE.EXCLAMATION)
                {
                    questMarkRes.Add("QUESTION", mark);
                }
                else
                {
                    questMarkRes.Add("EXCLAMATION", mark);
                }
                runningQuestMark.Remove(_npc);                
                CreateQuestMark(_npc, _state);                          // 퀘스트마크 새로 생성

            }

        }
        else
        {
            CreateQuestMark(_npc, _state);

        }
    }
    void CreateQuestMark(Npc _npc,QUESTSTATE _state)
    {        
        QuestMark questmark;
        switch (_state)
        {
            case QUESTSTATE.NONE:
                {                    
                    questmark = questMarkRes.GetData("EXCLAMATION");
                    questmark.MarkType = QUESTMARKTYPE.EXCLAMATION;
                    questmark.StartingOrComplete();
                    questmark.gameObject.transform.SetParent(_npc.transform);                    
                    questmark.gameObject.transform.localPosition = new Vector3(0,_npc.Nick_YPos + 0.2f, 0);
                    runningQuestMark.Add(_npc, questmark);
                }
                break;
            case QUESTSTATE.PLAYING:
                {                    
                    questmark = questMarkRes.GetData("QUESTION");
                    questmark.MarkType = QUESTMARKTYPE.QUESTION;
                    questmark.Playing();
                    questmark.gameObject.transform.SetParent(_npc.transform);
                    questmark.gameObject.transform.localPosition = new Vector3(0, _npc.Nick_YPos+0.2f, 0);
                    runningQuestMark.Add(_npc, questmark);
                }                
                break;
            case QUESTSTATE.COMPLETE:                
                questmark = questMarkRes.GetData("QUESTION");
                questmark.MarkType = QUESTMARKTYPE.QUESTION;
                questmark.Playing();
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
        GameObject clickNomal = ResourceManager.resource.GetEffect("Click_Nomal");
        clickNomal.transform.SetParent(this.transform);
        clickNomal.gameObject.SetActive(false);
        GameObject clickEnermy = ResourceManager.resource.GetEffect("Click_Enermy");
        clickEnermy.transform.SetParent(this.transform);
        clickEnermy.gameObject.SetActive(false);
        GameObject clickFriend = ResourceManager.resource.GetEffect("Click_Friend");
        clickFriend.transform.SetParent(this.transform);
        clickFriend.gameObject.SetActive(false);        
        clickList.Add(clickNomal);
        clickList.Add(clickEnermy);
        clickList.Add(clickFriend);


        QuestMark questMarkEx = ResourceManager.resource.GetEffect("EXCLAMATION").AddComponent<QuestMark>();
        QuestMark questMarkQu = ResourceManager.resource.GetEffect("QUESTION").AddComponent<QuestMark>();
        questMarkEx.MarkType = QUESTMARKTYPE.EXCLAMATION;
        questMarkQu.MarkType = QUESTMARKTYPE.QUESTION;
        questMarkRes.Add("EXCLAMATION", questMarkEx);
        questMarkRes.Add("QUESTION", questMarkQu);        
    }
    public void EffectReset()               // 맵 이동 시 리셋
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
            if(runningQuestMark[npcList[i]].MarkType == QUESTMARKTYPE.EXCLAMATION)
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
    
   
    GameObject GetEffect(string _effectName)                                // Effect 이름별로 꺼내오기
    {
        GameObject effectObj = effectRes.GetData(_effectName);
        if (effectObj == null)
        {
            GameObject NewEffect = ResourceManager.resource.GetEffect(_effectName);         // 리소스 수정후 수정할 것
            AddEfect(_effectName,NewEffect);
            return GetEffect(_effectName);
        }
        else
        {
            effectObj.gameObject.SetActive(true);
            return effectObj;
        }

    }
    void AddEfect(string _effectName,GameObject _effect)                                       // Effect 사용 후 저장
    {
        string effectName;
        if(runningEffect.TryGetValue(_effect,out effectName))
        {
            runningEffect.Remove(_effect);
            effectRes.Add(effectName, _effect);
        }
        effectRes.Add(_effect.name, _effect);
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
        AddEfect(_effectName,_effect);
    }


}
