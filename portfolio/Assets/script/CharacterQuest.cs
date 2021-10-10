using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterQuest 
{
    Dictionary<int, Quest> QuestList;
    Quest quest = null;
    public CharacterQuest()
    {
        QuestList = new Dictionary<int, Quest>();
    }
    public void Add(int _index, Quest _NewQuest)
    {
        QuestList.Add(_index, _NewQuest);
    }
    public Quest FindQuest(int _Index)
    {
        if (QuestList.TryGetValue(_Index, out quest))
        {
            return quest;
        }
        else
        {
            Debug.Log("퀘스트가 없습니다.");
            return null;
        }
    }

}
