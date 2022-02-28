using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CharacterQuest 
{
    Dictionary<HAVEQUESTSTATE, Dictionary<int, Quest>> All_QuesDic = new Dictionary<HAVEQUESTSTATE, Dictionary<int, Quest>>();
    Dictionary<int, Quest> QuestDic;
    List<int> PlayingQuestList = new List<int>();
    List<int> DoneQuestList = new List<int>();
    Quest quest = null;
    
    public CharacterQuest()
    {
        All_QuesDic[HAVEQUESTSTATE.PLAYING] = new Dictionary<int, Quest>();
        All_QuesDic[HAVEQUESTSTATE.FINISH] = new Dictionary<int, Quest>();
    }

    public void UpdateQuest(int _index,int _need)
    {
        QuestDic = All_QuesDic[HAVEQUESTSTATE.PLAYING];

        if (QuestDic.TryGetValue(_index, out quest))
        {
                      
        }
       else
        {
            Debug.LogError($"퀘스트 인덱스 {quest.Index}");
            Debug.LogError("없는 퀘스트를 업데이트 하려 합니다.");
            return;
        }

        quest.QuestUpdate(_need);
        // 퀘스트 UI 업데이트 할 것!      
        //UIManager.uimanager.quickQuest.UpdateQuest(_index);
        QuestDic = null;
        quest = null;
    }

    // 퀘스트 등록
    public void Add(HAVEQUESTSTATE _state,int _index, Quest _NewQuest)
    {        
        QuestDic = All_QuesDic[_state];
        
        if (QuestDic.TryGetValue(_index, out quest))
        {
            Debug.LogError($"퀘스트 인덱스 {quest.Index}");
            Debug.LogError("있는 퀘스트를 다시 받으려고 합니다.");
        }
        QuestDic.Add(_index, _NewQuest);
        AddList(_state, _index);
        QuestDic = null;
    }
    // 게임상에서 퀘스트를 받았을때
    public void Add(int _index, Quest _NewQuest)
    {
        QuestDic = All_QuesDic[HAVEQUESTSTATE.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            Debug.LogError($"퀘스트 인덱스 {quest.Index}");
            Debug.LogError("있는 퀘스트를 다시 받으려고 합니다.");
        }
        QuestDic.Add(_index, _NewQuest);
        AddList(HAVEQUESTSTATE.PLAYING, _index);
        //UIManager.uimanager.quickQuest.AddQuest(_NewQuest);
        QuestDic = null;
    }
    public void AddList(HAVEQUESTSTATE _state, int _Index)
    {
        switch (_state)
        {
            case HAVEQUESTSTATE.PLAYING:
                PlayingQuestList.Add(_Index);
                PlayingQuestList.Sort();                
                return;
            case HAVEQUESTSTATE.FINISH:
                DoneQuestList.Add(_Index);
                DoneQuestList.Sort();
                return;
        }
        QuestDic = null;
    }
    public List<int> GetQuestList(HAVEQUESTSTATE _state)
    {
        QuestDic = All_QuesDic[_state];
        List<int> SoltList = QuestDic.Keys.ToList<int>();
        SoltList.Sort();
        return SoltList;
    }
    
    public bool IsQuest(int _index)
    {
        QuestDic = All_QuesDic[HAVEQUESTSTATE.FINISH];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return true;
        }
        QuestDic = All_QuesDic[HAVEQUESTSTATE.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return true;
        }
        return false;
    }
    public Quest GetQuest(int _index)
    {
        QuestDic = All_QuesDic[HAVEQUESTSTATE.FINISH];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest;
        }
        QuestDic = All_QuesDic[HAVEQUESTSTATE.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest;
        }
        return null;
    }
    public QUESTSTATE GetState(int _index)
    {
        QuestDic = All_QuesDic[HAVEQUESTSTATE.FINISH];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest.State;
        }
        QuestDic = All_QuesDic[HAVEQUESTSTATE.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest.State;
        }
        return QUESTSTATE.NONE;
    }
    public void FinishQuest(int _index)
    {
        QuestDic = All_QuesDic[HAVEQUESTSTATE.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            QuestDic.Remove(_index);
            All_QuesDic[HAVEQUESTSTATE.FINISH].Add(_index, quest);
            QuestDic = null;

        }
        else
            Debug.LogError("없는 퀘스트를 완료하려 합니다.");        
    }
    
}
