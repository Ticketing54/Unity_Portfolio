using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Quest
{
    
    public int Index { get; }
    public string Name { get; }
    public QUESTTYPE Type { get; }      
    public string Explain { get; }    
    public GOALTYPE Goal_Type { get; }
    public int Goal_Index { get; }      //퀘스트 목표
    public int Goal_Need { get; }        //퀘스트 필요갯수샤차
    public int Goal_Current { get; set; }
    public QUESTSTATE State { get; set; }
    public List<int> PrecedeQuest { get; }

    public Quest(int _index, string _questState)
    {
        List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", _index);
        if (questTable == null)
        {
            Debug.Log("테이블에 없는 퀘스트를 더할려고 합니다.");
            return;
        }

        int t_Index;
        if(int.TryParse(questTable[0],out t_Index))                                                 // 인덱스
        {
            Index = t_Index;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 인덱스 오류");
            return;
        }

        Name = questTable[0];
        QUESTTYPE t_Type;
        if(Enum.TryParse(questTable[2],out t_Type))                                                 // 퀘스트타입
        {
            Type = t_Type;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 타입 오류");
            return;
        }

        Explain = questTable[3];

        GOALTYPE t_GoalType;
        if(Enum.TryParse(questTable[4],out t_GoalType))
        {
            Goal_Type = t_GoalType;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표타입 오류");
            return;
        }
        int t_GoalIndex;
        if(int.TryParse(questTable[5],out t_GoalIndex))
        {
            Goal_Index = t_GoalIndex;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표인덱스 오류");
            return;
        }

        int t_goalNeed;

        if (!string.IsNullOrEmpty(questTable[6]))
        {
            if (int.TryParse(questTable[6], out t_goalNeed))
            {
                Goal_Need = t_goalNeed;
            }
            else
            {
                Debug.LogError("퀘스트 테이블 필요갯수 오류");
                return;
            }
        }
        else
        {
            Goal_Need = -1;
        }
        

        QUESTSTATE questState;
        if(Enum.TryParse(_questState,out questState))
        {
            State = questState;
        }
        else
        {
            Debug.LogError("퀘스트 퀘스트상태 오류");
            return;
        }
        
        string q_PrecedQuest = questTable[7];
        if (!string.IsNullOrEmpty(q_PrecedQuest))
        {
            List<int> precedQuestList = new List<int>();
            string[] arrayPreced = questTable[1].Split('/');
            for (int i = 0; i < arrayPreced.Length; i++)
            {
                precedQuestList.Add(int.Parse(arrayPreced[i]));
            }
            PrecedeQuest = precedQuestList;
        }
  
    }
    public Quest(int _index, QUESTSTATE _questState)
    {
        List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", _index);
        if (questTable == null)
        {
            Debug.Log("테이블에 없는 퀘스트를 더할려고 합니다.");
            return;
        }

        int t_Index;
        if (int.TryParse(questTable[0], out t_Index))                                                 // 인덱스
        {
            Index = t_Index;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 인덱스 오류");
            return;
        }

        Name = questTable[0];
        QUESTTYPE t_Type;
        if (Enum.TryParse(questTable[2], out t_Type))                                                 // 퀘스트타입
        {
            Type = t_Type;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 타입 오류");
            return;
        }        
        Explain = questTable[3];

        GOALTYPE t_GoalType;
        if (Enum.TryParse(questTable[4], out t_GoalType))
        {
            Goal_Type = t_GoalType;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표타입 오류");
            return;
        }
        int t_GoalIndex;
        if (int.TryParse(questTable[5], out t_GoalIndex))
        {
            Goal_Index = t_GoalIndex;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표인덱스 오류");
            return;
        }

        int t_goalNeed;

        if (!string.IsNullOrEmpty(questTable[6]))
        {
            if (int.TryParse(questTable[6], out t_goalNeed))
            {
                Goal_Need = t_goalNeed;
            }
            else
            {
                Debug.LogError("퀘스트 테이블 필요갯수 오류");
                return;
            }
        }
        else
        {
            Goal_Need = -1;
        }

        State = _questState;
        string q_PrecedQuest = questTable[7];
        if (!string.IsNullOrEmpty(q_PrecedQuest))
        {
            List<int> precedQuestList = new List<int>();
            string[] arrayPreced = questTable[1].Split('/');
            for (int i = 0; i < arrayPreced.Length; i++)
            {
                precedQuestList.Add(int.Parse(arrayPreced[i]));
            }
            PrecedeQuest = precedQuestList;
        }

    }
    public void QuestUpdate(int _need)
    {
        Goal_Current += _need;
        if(Goal_Current >= Goal_Need)
        {
            State = QUESTSTATE.COMPLETE;
        }

    }
    public void QuestDone()
    {
        State = QUESTSTATE.DONE;
    }
}
