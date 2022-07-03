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
    public int Start_Npc { get; }
    public int Goal_Npc { get; }

    public int StartDialogIndex { get; }
    public int PlayingDialogIndex { get; }
    public int EndDialogIndex { get; }

    public int Reward_Gold { get; }
    public int Reward_Exp { get; }
    public List<List<int>> Reward_Item { get; }    
    public string CompleteQuestCutScene { get; }
    public string DoneQuestCutSCene { get; }
    public Quest(int _index, string _questState)
    {
        List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", _index);
        if (questTable == null)
        {
            Debug.Log("테이블에 없는 퀘스트를 더할려고 합니다.");
            return;
        }

        int t_Index;
        if(int.TryParse(questTable[0],out t_Index))                                                
        {
            Index = t_Index;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 인덱스 오류");
            return;
        }

        Name = questTable[1];
        QUESTTYPE t_Type;
        if(Enum.TryParse(questTable[2],out t_Type))                                                 
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
            //Debug.LogError("퀘스트 테이블 목표인덱스 오류");
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
            string[] arrayPreced = questTable[7].Split('/');
            for (int i = 0; i < arrayPreced.Length; i++)
            {
                precedQuestList.Add(int.Parse(arrayPreced[i]));
            }
            PrecedeQuest = precedQuestList;
        }

        int t_StartNpc;
        if (int.TryParse(questTable[8], out t_StartNpc))
        {
            Start_Npc = t_StartNpc;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표npc 오류");
            return;
        }

        int t_goalNpc;
        if (int.TryParse(questTable[9], out t_goalNpc))
        {
            Goal_Npc = t_goalNpc;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표npc 오류");
            return;
        }
        int t_StartDialog;
        if (int.TryParse(questTable[10], out t_StartDialog))
        {
            StartDialogIndex = t_StartDialog;
        }
        else
        {
            Debug.Log("퀘스트 dialognpc 오류");
            return;
        }
        int t_PlayingDialog;
        if (int.TryParse(questTable[11], out t_PlayingDialog))
        {
            PlayingDialogIndex = t_PlayingDialog;
        }
        else
        {
            Debug.Log("퀘스트 dialognpc 오류");
            return;
        }
        int t_EndDialog;
        if (int.TryParse(questTable[12], out t_EndDialog))
        {
            EndDialogIndex = t_EndDialog;
        }
        else
        {
            Debug.Log("퀘스트 dialognpc 오류");
            return;
        }

        int t_Reward_Exp;
        if (int.TryParse(questTable[13], out t_Reward_Exp))
        {
            Reward_Exp = t_Reward_Exp;
        }
        else
        {
            Reward_Exp = 0;            
        }

        int t_Reward_Gold;
        if (int.TryParse(questTable[14], out t_Reward_Gold))
        {
            Reward_Gold = t_Reward_Gold;
        }
        else
        {
            Reward_Gold = 0;            
        }


        if (!string.IsNullOrEmpty(questTable[15]))
        {
            string[] itemArray = questTable[15].Split("/");
            List<List<int>> reward_Items = new List<List<int>>();
            for (int i = 0; i < itemArray.Length; i++)
            {
                string[] subitem = itemArray[i].Split("#");
                List<int> itemInfo = new List<int>();
                for (int iteminfo_index = 0; iteminfo_index < subitem.Length; iteminfo_index++)
                {
                    itemInfo.Add(int.Parse(subitem[iteminfo_index]));
                }
                reward_Items.Add(itemInfo);
            }

            Reward_Item = reward_Items;
        }
        if (!string.IsNullOrEmpty(questTable[16]))
        {
            CompleteQuestCutScene = questTable[16];
        }
        if (!string.IsNullOrEmpty(questTable[17]))
        {
            DoneQuestCutSCene = questTable[17];
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
            string[] arrayPreced = questTable[7].Split('/');
            for (int i = 0; i < arrayPreced.Length; i++)
            {
                precedQuestList.Add(int.Parse(arrayPreced[i]));
            }
            PrecedeQuest = precedQuestList;
        }
        int t_StartNpc;
        if (int.TryParse(questTable[8], out t_StartNpc))
        {
            Start_Npc = t_StartNpc;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표npc 오류");
            return;
        }

        int t_goalNpc;
        if (int.TryParse(questTable[9], out t_goalNpc))
        {
            Goal_Npc = t_goalNpc;
        }
        else
        {
            Debug.LogError("퀘스트 테이블 목표npc 오류");
            return;
        }
        int t_StartDialog;
        if (int.TryParse(questTable[10], out t_StartDialog))
        {
            StartDialogIndex = t_StartDialog;
        }
        else
        {
            Debug.Log("퀘스트 dialognpc 오류");
            return;
        }
        int t_PlayingDialog;
        if (int.TryParse(questTable[11], out t_PlayingDialog))
        {
            PlayingDialogIndex = t_PlayingDialog;
        }
        else
        {
            Debug.Log("퀘스트 dialognpc 오류");
            return;
        }
        int t_EndDialog;
        if (int.TryParse(questTable[12], out t_EndDialog))
        {
            EndDialogIndex = t_EndDialog;
        }
        else
        {
            Debug.Log("퀘스트 dialognpc 오류");
            return;
        }
        int t_Reward_Exp;
        if (int.TryParse(questTable[13], out t_Reward_Exp))
        {
            Reward_Exp = t_Reward_Exp;
        }
        else
        {
            Reward_Exp = 0;
        }

        int t_Reward_Gold;
        if (int.TryParse(questTable[14], out t_Reward_Gold))
        {
            Reward_Gold = t_Reward_Gold;
        }
        else
        {
            Reward_Gold = 0;
        }


        if (!string.IsNullOrEmpty(questTable[15]))
        {
            string[] itemArray = questTable[15].Split("/");
            List<List<int>> reward_Items = new List<List<int>>();
            for (int i = 0; i < itemArray.Length; i++)
            {
                string[] subitem = itemArray[i].Split("#");
                List<int> itemInfo = new List<int>();
                for (int iteminfo_index = 0; iteminfo_index < subitem.Length; iteminfo_index++)
                {
                    itemInfo.Add(int.Parse(subitem[iteminfo_index]));
                }
                reward_Items.Add(itemInfo);
            }

            Reward_Item = reward_Items;
        }
        if (!string.IsNullOrEmpty(questTable[16]))
        {
            CompleteQuestCutScene = questTable[16];
        }
        if (!string.IsNullOrEmpty(questTable[17]))
        {
            DoneQuestCutSCene = questTable[17];
        }
    }
}
