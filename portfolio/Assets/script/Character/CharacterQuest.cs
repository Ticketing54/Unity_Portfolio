using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterQuest 
{
    Dictionary<HaveQuestState, Dictionary<int, Quest>> AllQuestList = new Dictionary<HaveQuestState, Dictionary<int, Quest>>();
    Dictionary<int, Quest> QuestDic;
    List<int> PlayingQuestList = new List<int>();
    List<int> DoneQuestList = new List<int>();
    Quest quest = null;
    
    public void Add(HaveQuestState _state,int _index, Quest _NewQuest)
    {        
        QuestDic = AllQuestList[_state];
        
        if (QuestDic.TryGetValue(_index, out quest))
        {
            Debug.LogError($"퀘스트 인덱스 {quest.Index}");
            Debug.LogError("있는 퀘스트를 다시 받으려고 합니다.");
        }
        QuestDic.Add(_index, _NewQuest);        
    }
    public void Add(int _index, Quest _NewQuest)
    {
        QuestDic = AllQuestList[HaveQuestState.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            Debug.LogError($"퀘스트 인덱스 {quest.Index}");
            Debug.LogError("있는 퀘스트를 다시 받으려고 합니다.");
        }
        QuestDic.Add(_index, _NewQuest);        
    }
    public void AddList(HaveQuestState _state, int _Index)
    {
        switch (_state)
        {
            case HaveQuestState.PLAYING:
                PlayingQuestList.Add(_Index);
                PlayingQuestList.Sort();
                return;
            case HaveQuestState.FINISH:
                DoneQuestList.Add(_Index);
                DoneQuestList.Sort();
                return;
        }
    }
    public List<int> GetQuestList(HaveQuestState _state)
    {
        QuestDic = AllQuestList[_state];
        List<int> SoltList = QuestDic.Keys.ToList<int>();
        SoltList.Sort();
        return SoltList;
    }
    
    public bool IsQuest(int _index)
    {
        QuestDic = AllQuestList[HaveQuestState.FINISH];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return true;
        }
        QuestDic = AllQuestList[HaveQuestState.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return true;
        }
        return false;
    }
    public Quest GetQuest(int _index)
    {
        QuestDic = AllQuestList[HaveQuestState.FINISH];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest;
        }
        QuestDic = AllQuestList[HaveQuestState.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest;
        }
        return null;
    }
    public QuestState GetState(int _index)
    {
        QuestDic = AllQuestList[HaveQuestState.FINISH];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest.State;
        }
        QuestDic = AllQuestList[HaveQuestState.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            return quest.State;
        }
        return QuestState.None;
    }
    public void FinishQuest(int _index)
    {
        QuestDic = AllQuestList[HaveQuestState.PLAYING];
        if (QuestDic.TryGetValue(_index, out quest))
        {
            QuestDic.Remove(_index);
            AllQuestList[HaveQuestState.FINISH].Add(_index, quest);
            QuestDic = null;

        }
        else
            Debug.LogError("없는 퀘스트를 완료하려 합니다.");        
    }
    
}
