using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestDic : SerializableDictionary<int, QUESTSTATE> { }
[System.Serializable]
public class CharacterQuest 
{    
    [SerializeField]
    QuestDic questDic = new QuestDic();
    
    
    
    public void AddQuest(int _index)
    {
        if (questDic.ContainsKey(_index))
        {
            Debug.Log("있는 퀘스트를 더할려고 합니다.");
            return;
        }      

        Quest quest = new Quest(_index,"PLAYING");       
        questDic.Add(_index, QUESTSTATE.PLAYING);
    }
    
    public QUESTSTATE ChracterState_Quest(int _index)
    {        
        if (!questDic.ContainsKey(_index))                                                          // 퀘스트가 없을 경우
        {
            Quest quest = new Quest(_index,"PLAYING");

            List<int> precedQuestList = quest.PrecedeQuest;
                
            if (precedQuestList != null)                                                            // 선행 퀘스트가 있을 경우
            {
                foreach (int questindex in precedQuestList)
                {
                    if (!questDic.ContainsKey(_index) || questDic[_index] != QUESTSTATE.DONE)       // 선행 퀘스트를 안했을 경우
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
            switch (questDic[_index])
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
}
