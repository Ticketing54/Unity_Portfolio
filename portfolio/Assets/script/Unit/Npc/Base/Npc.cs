using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public abstract class Npc :Unit
{

    [SerializeField]
    protected List<int> quests;
    [SerializeField]
    protected List<int> items;
    [SerializeField]
    protected string dialogue;
    protected int npcIndex;
    public int NpcIndex { get => npcIndex; }

    public List<int> QUEST { get => quests; }
    public List<int> ITEMS { get => items; }
    public string DIALOGUE { get => dialogue; }

    protected NavMeshAgent nav;
    protected Quaternion startDir;
    protected Coroutine action;
    string miniDotName;
    
    public override string MiniDotSpriteName()
    {
        return miniDotName;
    }


    public virtual void OnEnable()
    {
        StartCoroutine(CoApproachChracter());
    }
    public virtual void OnDisable()
    {
        StopAllCoroutines();
        UIManager.uimanager.ARemoveNearUnit(this);
    }
    public void SetNpc(int _index,string _npcName, float _nickYPos,List<int> _quests,List<int> _items,string _dialogue)
    {
        npcIndex = _index;
        unitName = _npcName;        
        nick_YPos = _nickYPos;        
        items = _items;                 
        dialogue = _dialogue;
        quests = _quests;
        SetQuestMark();
        startPos = transform.position;
        startDir = transform.rotation;
        miniDotName = "DotN";
    }

    public void LookIdle()
    {
        if(action != null)
        {
            StopCoroutine(action);
        }        
        action = StartCoroutine(CoLookIdle());
    }
    IEnumerator CoLookCharacter()
    {
        Vector3 dir = (GameManager.gameManager.character.transform.position - transform.position).normalized;        
        dir.y = 0;
        float timer = 0f;
        while (timer <=1f)
        {

            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, timer, timer);
            timer += Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(newdir);
            yield return null;
        }            
    }
    IEnumerator CoLookIdle()
    {   
        float timer = 0f;
        while (timer<=1f)
        {

            //Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, timer, timer);
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, startDir,timer);
            yield return null;
        }
    }
    public virtual void SetQuestMark()
    {
        if (quests == null)
        {
            return;
        }
        else
        {
            
            for (int i = 0; i < quests.Count; i++)
            {
                Quest quest = GameManager.gameManager.character.quest.GetQuest(quests[i]);
                if(quest == null)
                {
                    List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", quests[i]);
                    int startNpcIndex ;

                    if(int.TryParse(questTable[8],out startNpcIndex))
                    {
                        if (GameManager.gameManager.character.quest.ClearPrecedQuest(quests[i]) && startNpcIndex == npcIndex)
                        {
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTSTATE.NONE);
                            miniDotName = "Exclamation";
                            return;
                        }
                    }
                                                        
                }
                else
                {
                    switch (quest.State)
                    {
                        case QUESTSTATE.PLAYING:
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTSTATE.PLAYING);
                            return;
                        case QUESTSTATE.COMPLETE:
                            {
                                if(quest.Start_Npc == NpcIndex && quest.Type == QUESTTYPE.DIALOG)
                                {
                                    EffectManager.effectManager.UpdateQuestMark(this, QUESTSTATE.PLAYING);
                                    miniDotName = "QuestionWhite";
                                }
                                else
                                {
                                    EffectManager.effectManager.UpdateQuestMark(this, QUESTSTATE.COMPLETE);
                                    miniDotName = "QuestionGreen";
                                }
                            }
                            
                            return;
                        case QUESTSTATE.DONE:
                            continue;
                        default:
                            Debug.LogError("npc 퀘스트 마크 생성 오류");
                            break;
                    }
                }
                
            }
            EffectManager.effectManager.RemoveQuestMark(this);
            miniDotName = "DotN";
        }            
    }
   
    public virtual void Interact()
    {
        GameManager.gameManager.character.isPossableMove = false;
        UIManager.uimanager.AOpenDialog(this);
        if(action != null)
        {
            StopCoroutine(action);
        }
        action = StartCoroutine(CoLookCharacter());
    }
    public virtual void EtcQuest(int _questIndex)
    {
        Debug.Log("기타 퀘스트");
    }

    protected IEnumerator CoApproachChracter()
    {
        yield return null;

        bool approachChracter = false;
        while (true)
        {
            if (GameManager.gameManager.character != null)
            {
                if (this.DISTANCE < 6f && approachChracter == false)
                {
                    approachChracter = true;
                    UIManager.uimanager.AAddNearUnit(this);
                }

                if (this.DISTANCE >= 6f && approachChracter == true)
                {
                    approachChracter = false;
                    UIManager.uimanager.ARemoveNearUnit(this);
                }
            }
            yield return null;
        }
    }



}
