using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public class Npc :NpcUnit
{   
    
    protected NavMeshAgent nav;
    protected Quaternion startDir;
    protected Coroutine action;
    
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
                                }
                                else
                                {
                                    EffectManager.effectManager.UpdateQuestMark(this, QUESTSTATE.COMPLETE);
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
        }            
    }
    public override void OnEnable()
    {
        base.OnEnable();
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

  


    //IEnumerator Mini_DotMove()
    //{
    //    while (true)
    //    {
    //        if (UIManager.uimanager.minimap.Minimap_n.gameObject.activeSelf == true && Character.Player != null)
    //        {
    //            if (DISTANCE <= 20f && MiniMap_Dot == null )
    //            {

    //                MiniMap_Dot = ObjectPoolManager.objManager.PoolingMiniDot_n();
    //                MiniMap_Dot.sprite = ResourceManager.resource.GetImage("Dot_N");

    //            }
    //            else if (DISTANCE <= 20f && MiniMap_Dot != null)
    //            {

    //                MiniMap_Dot.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position);

    //            }
    //            else if ((DISTANCE > 20f && MiniMap_Dot != null) )
    //            {
    //                MiniMap_Dot.gameObject.SetActive(false);
    //                MiniMap_Dot = null;
    //            }




    //        }

    //        yield return null;
    //    }
    //}

}
