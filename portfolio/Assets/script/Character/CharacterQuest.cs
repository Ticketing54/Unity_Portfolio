using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CharacterQuest 
{
    Dictionary<int, Quest> questDic = new Dictionary<int, Quest>();
    
    
    public void AddQuest(int _index)
    {
        if (questDic.ContainsKey(_index))
        {
            Debug.Log("있는 퀘스트를 더할려고합니다.");
            return;
        }
        List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable",_index);
        List<int> precedQuest;
        Quest quest;
        if (questTable[1] != "")
        {
            precedQuest = new List<int>();
            string[] arrayPreced = questTable[1].Split('/');
            for (int i = 0; i < arrayPreced.Length; i++)
            {
                precedQuest.Add(int.Parse(arrayPreced[i]));
            }
            quest = new Quest(int.Parse(questTable[0]), questTable[2], questTable[3], questTable[4], questTable[5], int.Parse(questTable[6]), int.Parse(questTable[7]), "PLAYING",precedQuest);
        }
        else
        {
            quest = new Quest(int.Parse(questTable[0]), questTable[2], questTable[3], questTable[4], questTable[5], int.Parse(questTable[6]), int.Parse(questTable[7]), "PLAYING");
        }
        
        questDic.Add(_index, quest);
    }
    public Quest GetQuest (int _index)
    {
        Quest quest;
        if(questDic.TryGetValue(_index,out quest))
        {
            return quest;
        }
        else
        {
            return null;
        }
    }
    public int ChracterState_Quest(int _index)
    {
        Quest quest = GetQuest(_index);
        if (quest == null)
        {
            return (int)QUESTSTATE.NONE;
        }
            
        List<int> precedQuestList = quest.PrecedeQuest;

        foreach(int questindex in precedQuestList)
        {
            Quest precedQuest = GetQuest(questindex);
            if(precedQuest == null || (precedQuest.State != QUESTSTATE.DONE))
            {
                return (int)QUESTSTATE.DONE;
            }
        }

        switch (quest.State)
        {
            case QUESTSTATE.PLAYING:
                return (int)QUESTSTATE.PLAYING;
            case QUESTSTATE.COMPLETE:
                return (int)QUESTSTATE.COMPLETE;
            case QUESTSTATE.DONE:
                return (int)QUESTSTATE.DONE;
            default:
                return 3;
        }        
    }
   
}
