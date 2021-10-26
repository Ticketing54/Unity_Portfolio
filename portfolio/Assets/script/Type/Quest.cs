using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Quest
{
    
    public int Index { get; }
    public string Name { get; }
    public QuestType Type { get; }      
    public string Explain { get; }
    public int npc_Index { get; }
    public GoalType goal_Type { get; }
    public int goal_Index { get; }      //퀘스트 목표
    public int goal_Need { get; }        //퀘스트 필요갯수샤차
    public int goal_Current { get; set; }
    public QuestState State { get; }

    public Quest(int _index, string _questName,string _Type, string _questExplain,string _goalType, int _goal_Index,int _goal_Need,string _State = "NONE")
    {
        QuestType questType;
        GoalType goalType;
        QuestState questState;

        Index = _index;
        Name = _questName;
        if (Enum.TryParse(_Type, out questType))
        {
            Type = questType;
        }
        else
            Debug.LogError("퀘스트 타입 변환이 이뤄지지 않았습니다.");
        
        Explain = _questExplain;

        if (Enum.TryParse(_Type, out goalType))
        {
            goal_Type = goalType;
        }
        else
            Debug.LogError("퀘스트 목표 타입 변환이 이뤄지지 않았습니다.");        

        goal_Index = _goal_Index;
        goal_Need = _goal_Need;

        if (Enum.TryParse(_Type, out questState))
        {
            State = questState;
        }
        else
            Debug.LogError("퀘스트 상태 변환이 이뤄지지 않았습니다.");
    }
}
