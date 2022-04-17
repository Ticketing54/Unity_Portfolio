using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public class Npc :NpcUnit
{   
    
    protected NavMeshAgent nav;   

    public void SetNpc(int _index,string _npcName, float _nickYPos,List<int> _quests,List<int> _items,string _dialogue)
    {
        npcIndex = _index;
        unitName = _npcName;        
        nick_YPos = _nickYPos;        
        items = _items;                 
        dialogue = _dialogue;
        quests = _quests;
        SetQuestMark();
    }

    public void SetQuestMark()
    {
        if (quests == null)
        {
            return;
        }
        else
        {
            
            for (int i = 0; i < quests.Count; i++)
            {
                Quest quest = Character.Player.quest.GetQuest(quests[i]);
                if(quest == null)
                {
                    List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", quests[i]);
                    int startNpcIndex ;

                    if(int.TryParse(questTable[8],out startNpcIndex))
                    {
                        if (Character.Player.quest.ClearPrecedQuest(quests[i]) && startNpcIndex == npcIndex)
                        {
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTMARKTYPE.EXCLAMATION, QUESTSTATE.NONE);
                            return;
                        }
                    }
                                                        
                }
                else
                {
                    switch (quest.State)
                    {
                        case QUESTSTATE.PLAYING:
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTMARKTYPE.QUESTION, QUESTSTATE.PLAYING);
                            return;
                        case QUESTSTATE.COMPLETE:
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTMARKTYPE.QUESTION, QUESTSTATE.COMPLETE);
                            return;
                        case QUESTSTATE.DONE:
                            continue;
                        default:
                            Debug.LogError("npc 퀘스트 마크 생성 오류");
                            break;
                    }
                }
                
            }
        }            
    }   
    private void OnEnable()
    {
        
    }
    public virtual void Start()
    {
        UIManager.uimanager.uiEffectManager.ActiveNpcUi(this);
        nav = this.GetComponent<NavMeshAgent>();        
       
    }
    public virtual void Interact()
    {
        Character.Player.isCantMove = true;
        UIManager.uimanager.OpenDialog(this);        
    }
    public virtual void EtcQuest(int _questIndex)
    {
        
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
