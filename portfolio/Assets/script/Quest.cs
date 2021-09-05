using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Quest
{
    public enum QuestType
    {
        Battle,
        Item,
        Dialog,
        Etc
    }
    public int Index;           //퀘스트 인덱스
    public int npc_Index;       //NPC 인덱스
    public int QuestComplete = 0;        // 퀘스트 상태
    public string questName;    //퀘스트 이름
    public QuestType questType; // 퀘스트 타입
    public string questExplain; // 퀘스트 설명


    public int goal_Index;      //퀘스트 목표
    public int goal_num;        //퀘스트 필요갯수샤차
    public int goal_C = 0;

    public Quest(int _index,int _npcIndex,  string _questType, string _questName,string _questExplain, int _goal_index,int _goal_num)
    {
        Index = _index;
        npc_Index = _npcIndex;
        questName = _questName;
        questType = (QuestType)Enum.Parse(typeof(QuestType), _questType);
        questExplain = _questExplain;
        goal_Index = _goal_index;
        goal_num = _goal_num;
    }
    
    













}
