using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[System.Serializable]
public class QuestStateDic : SerializableDictionary<int, QUESTSTATE> { }
public class QuestDic : SerializableDictionary<int, Quest> { }
[System.Serializable]
public class CharacterQuest 
{
    Character character;

    Dictionary<int, Quest> quickQuests;

    public CharacterQuest(Character _character)
    {
        character = _character;
        allQuestDic     = new QuestStateDic();
        playingQuest    = new QuestDic();
        doneQuest       = new QuestDic();
        questItem       = new Dictionary<int, int>();
        questMonster    = new Dictionary<int, int>();
        quickQuests     = new Dictionary<int, Quest>();
        _character.AddKeyBoardSortCut(KeyCode.J, TryOpenQuest);
    }
    [SerializeField]
    QuestStateDic allQuestDic;
    QuestDic playingQuest;
    QuestDic doneQuest;


    // 아이템 인덱스 / 퀘스트 인덱스 
    Dictionary<int, int> questItem;
    // 몬스터 인덱스 / 퀘스트 인덱스 
    Dictionary<int, int> questMonster;

    bool questActive = false;
    void TryOpenQuest()
    {
        questActive = !questActive;
        if (questActive)
        {
            UIManager.uimanager.AOpenQuestMain();
        }
        else
        {
            UIManager.uimanager.ACloseQuestMain();
        }
            
    }
    
    #region QuickQuest

    void AddQuickQuest(int _index, Quest _quest)
    {
        if(quickQuests.Count >= 4)
        {
            return;
        }
        else
        {
            quickQuests.Add(_index, _quest);
            UIManager.uimanager.AUpdateQuickQuestUi(_quest);
        }
    }

    void UpdateQuickQuest(int _index)
    {
        if (quickQuests.ContainsKey(_index))
        {
            Quest quest = quickQuests[_index];
            UIManager.uimanager.AUpdateQuickQuestUi(quest);
        }
    }

    void RemoveQuickQuest(int _index)
    {
        if (quickQuests.ContainsKey(_index))
        {
            Quest quest = quickQuests[_index];
            quickQuests.Remove(_index);
            UIManager.uimanager.AUpdateQuickQuestUi(quest);
        }
    }
    public List<Quest> GetQuickSlots()
    {   
        return new List<Quest>(quickQuests.Values);
    }
    #endregion

    public List<Quest> GetQuestList(QUESTSTATE _state)
    {
        switch (_state)
        {
            case QUESTSTATE.PLAYING:
                {
                    List<Quest> playingQuestList = new List<Quest>(playingQuest.Values);
                    return playingQuestList;
                }                
            case QUESTSTATE.DONE:
                {
                    List<Quest> doneQuestList = new List<Quest>(doneQuest.Values);
                    return doneQuestList;
                }
            default:
                return null;                
        }
    }
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
                    Npc targetNpc = ObjectManager.objManager.GetNpc(quest.Goal_Index);
                    targetNpc.EtcQuest(quest.Index);                    
                    break;
                }
            default:
                break;
        }

        AddQuickQuest(_index, quest);        
        allQuestDic.Add(_index, quest.State);
        playingQuest.Add(_index, quest);
        UIManager.uimanager.AAddQuestUi(_index);
        ObjectManager.objManager.UpdateQuestMark(quest);        
    }
    public void UpdateQuest_Monster(int _monsterIndex)
    {
        if(questMonster.ContainsKey(_monsterIndex))
        {
            UpdatePlayingQuest(questMonster[_monsterIndex], 1);
        }
    }
    public void UpdateQuest_Item(int _itemIndex,int _itemCount)
    {
        if (questItem.ContainsKey(_itemIndex))
        {
            UpdatePlayingQuest(questItem[_itemIndex], _itemCount);
        }
    }
    public void UpdateQuest_Etc(int _questIndex)
    {
        UpdatePlayingQuest(_questIndex, 1);
    }
    void UpdatePlayingQuest(int _index, int _addCount)
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
                if (!string.IsNullOrEmpty(quest.CompleteQuestCutScene))
                {
                    LoadingSceneController.instance.LoadCutScene(quest.CompleteQuestCutScene);
                }                
                ObjectManager.objManager.UpdateQuestMark(quest);                
            }
            UpdateQuickQuest(_index);
            UIManager.uimanager.AQuestUpdateUi(_index,quest.State);
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
    public void QuestDone(int _index)
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
            character.inven.AddGold(popQuest.Reward_Gold); 
            character.stat.GetExp(popQuest.Reward_Exp);
            character.inven.GetRewards(popQuest.Reward_Item);

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
                UIManager.uimanager.AQuestUpdateUi(_index,popQuest.State);
                RemoveQuickQuest(_index);
                ObjectManager.objManager.UpdateQuestMark(popQuest);                
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
