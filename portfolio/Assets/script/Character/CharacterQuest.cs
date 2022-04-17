using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestStateDic : SerializableDictionary<int, QUESTSTATE> { }
public class QuestDic : SerializableDictionary<int, Quest> { }
[System.Serializable]
public class CharacterQuest 
{
    [SerializeField]
    QuestStateDic allQuestDic = new QuestStateDic();
    QuestDic playingQuest = new QuestDic();
    QuestDic doneQuest = new QuestDic();



    Dictionary<int, int> questItem = new Dictionary<int, int>();        // 아이템 인덱스 / 퀘스트 인덱스 
    Dictionary<int, int> questMonster = new Dictionary<int, int>();     // 몬스터 인덱스 / 퀘스트 인덱스 


    public void AddQuest(int _index, QUESTSTATE _state=QUESTSTATE.PLAYING)
    {
        if (allQuestDic.ContainsKey(_index))
        {
            Debug.Log("있는 퀘스트를 더할려고 합니다.");
            return;
        }      
        Quest quest = new Quest(_index,_state);
        switch (quest.Type)
        {
            case QUESTTYPE.DIALOG:
                {
                    quest.State = QUESTSTATE.COMPLETE;                    
                    break;
                }
            case QUESTTYPE.BATTLE:
                {
                    questMonster.Add(quest.Goal_Index,quest.Index);
                    break;
                }
            case QUESTTYPE.COLLECT:
                {
                    questItem.Add(quest.Goal_Index,quest.Index);
                    break;
                }
            case QUESTTYPE.ETC:
                {
                    ObjectManager.objManager.GetNpc(quest.Goal_Index).EtcQuest(quest.Index);
                    break;
                }
            default:
                break;
        }
        allQuestDic.Add(_index, quest.State);
        playingQuest.Add(_index, quest);
        ObjectManager.objManager.UpdateQuestMark(quest.Start_Npc);
        ObjectManager.objManager.UpdateQuestMark(quest.Goal_Npc);
    }
    public void UpdatePlayingQuest(int _index, int _addCount)
    {
        if (playingQuest.ContainsKey(_index))
        {
            Quest quest = playingQuest[_index];

            quest.Goal_Current++;

            if(quest.Goal_Current >= quest.Goal_Need)
            {
                quest.State = QUESTSTATE.COMPLETE;
                switch (quest.Type)
                {                    
                    case QUESTTYPE.BATTLE:
                        {
                            questMonster.Remove(quest.Goal_Index);
                            break;
                        }
                    case QUESTTYPE.COLLECT:
                        {
                            questItem.Remove(quest.Goal_Index);
                            break;
                        }                    
                    default:
                        break;
                }

                ObjectManager.objManager.UpdateQuestMark(quest.Start_Npc);
                ObjectManager.objManager.UpdateQuestMark(quest.Goal_Npc);
            }


        }
        else
        {
            Debug.LogError("없는 퀘스트를 갱신하려 합니다.");
        }
    }
    public bool isQuestMonster(int _index)
    {
        if (questMonster.ContainsKey(_index))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isQuestItem(int _index)
    {
        if (questItem.ContainsKey(_index))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Quest GetQuest(int _index)
    {
        QUESTSTATE state;        
        if (allQuestDic.TryGetValue(_index, out state))
        {
            if(state == QUESTSTATE.DONE)
            {
                return doneQuest[_index];
            }
            else
            {
                return playingQuest[_index];
            }
        }
        else
        {
            return null;
        }
    }
    public void QuestComplete(int _index)
    {        
        if (allQuestDic.ContainsKey(_index))
        {
            allQuestDic[_index] = QUESTSTATE.DONE;
        }
        else
        {
            Debug.LogError("없는 퀘스트를 완료하려 합니다.");
        }
        Quest popQuest;
        if (playingQuest.TryGetValue(_index,out popQuest))
        {
            Character.Player.inven.gold += popQuest.Reward_Gold;
            Character.Player.stat.EXP += popQuest.Reward_Exp;
            Character.Player.inven.GetRewards(popQuest.Reward_Item);

            switch (popQuest.Type)
            {
                case QUESTTYPE.BATTLE:
                    {
                        questMonster.Remove(popQuest.Goal_Index);
                        break;
                    }
                case QUESTTYPE.COLLECT:
                    {
                        questItem.Remove(popQuest.Goal_Index);
                        break;
                    }
                default:
                    break;
            }

            popQuest.State = QUESTSTATE.DONE;
            playingQuest.Remove(_index);
            if (doneQuest.ContainsKey(_index))
            {
                Debug.LogError("이미 완료된 퀘스트를 완료하려합니다.");
            }
            else
            {
                doneQuest.Add(_index, popQuest);
            }
            allQuestDic[_index] = QUESTSTATE.DONE;
        }
        else
        {
            Debug.LogError("없는 퀘스트를 완료하려합니다.");
        }        
    }
    public QUESTSTATE ChracterState_Quest(int _index)
    {        
        if (!allQuestDic.ContainsKey(_index))                                                          // 퀘스트가 없을 경우
        {
            Quest quest = new Quest(_index,"PLAYING");

            List<int> precedQuestList = quest.PrecedeQuest;
                
            if (precedQuestList != null)                                                            // 선행 퀘스트가 있을 경우
            {
                foreach (int questindex in precedQuestList)
                {
                    if (!allQuestDic.ContainsKey(_index) || allQuestDic[_index] != QUESTSTATE.DONE)       // 선행 퀘스트를 안했을 경우
                    {
                        return QUESTSTATE.NONE;
                    }
                }

                return QUESTSTATE.READY;                                                            // 선행퀘스트를 다 했을 경우
            }
            else
            {
                return QUESTSTATE.READY;                                                            // 선행 퀘스트가 없을 경우
            }
        }
        else                                                                                        // 퀘스트가 있을경우
        {
            switch (GetQuest(_index).State)
            {
                case QUESTSTATE.PLAYING:
                    return QUESTSTATE.PLAYING;
                case QUESTSTATE.COMPLETE:
                    return QUESTSTATE.COMPLETE;
                case QUESTSTATE.DONE:
                    return QUESTSTATE.DONE;
                default:
                    return QUESTSTATE.NONE;
            }
        }            
    }   

    public bool ClearPrecedQuest(int _index)
    {
        Quest quest = new Quest(_index, "PLAYING");
        List<int> precedQuestList = quest.PrecedeQuest;
        if(precedQuestList == null)
        {
            return true;
        }
        else
        {
            foreach (int questindex in precedQuestList)
            {
                if (allQuestDic.ContainsKey(questindex) )
                {
                    if(allQuestDic[questindex] == QUESTSTATE.DONE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                        
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
