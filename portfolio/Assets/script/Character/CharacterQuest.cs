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
    
    public void AddQuest(int _index, QUESTSTATE _state=QUESTSTATE.PLAYING)
    {
        if (allQuestDic.ContainsKey(_index))
        {
            Debug.Log("있는 퀘스트를 더할려고 합니다.");
            return;
        }      
        Quest quest = new Quest(_index,_state);
        allQuestDic.Add(_index, quest.State);
        playingQuest.Add(_index, quest);
        ObjectManager.objManager.UpdateQuestMark(quest.Start_Npc);
        ObjectManager.objManager.UpdateQuestMark(quest.Goal_Npc);
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
    public void QuestUpdate()
    {

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
