using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;
    public GameObject QuestList;
    public MiniQuestSlot QuestSlot;
    public List<MiniQuestSlot> list = new List<MiniQuestSlot>();
    public ScrollRect Mqlist;
    public bool QuestComplete = false;
    
    MiniQuestSlot CMini; // 현재 선택한 퀘스트

    Quest quest;
    MiniQuestSlot IsSlot;
    List<string> data = new List<string>();



    
    public Questlist questlist_M;

    private void Awake()
    {
        if (questManager == null)
        {
            questManager = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddQuest(int _num)
    {
        data = QuestTableManager.instance.quest_Table.GetData(_num);        
        


        if (quest.Type == QuestType.BATTLE)
        {
            foreach (Npc npc in Character.Player.npcList)
            {
                if (npc.index == quest.npc_Index)
                {
                    QuestCompletenum(npc, quest.Index, 1);
                    npc.QuestMarkerNum = 1;
                }
            }
            //몬스터들 변경
            for (int i = 0; i < Character.Player.MobList.Count; i++)
            {
                if (Character.Player.MobList[i].Index == quest.goal_Index)
                    Character.Player.MobList[i].isQuestMob = true;
            }
        }
        //else if (quest.Type == QuestType.Dialog)   // 특정인물 찾는 퀘스트 사용할때
        //{
        //    quest.State = 2;

        //    foreach (Npc npc in Character.Player.npcList)
        //    {
        //        if(npc.index == quest.npc_Index)
        //        {
                    
        //            QuestCompletenum(npc, quest.Index, 2);
        //            npc.QuestMarkerNum = 2;
        //        }
        //    }


        //}
        else if (quest.Type == QuestType.ETC)   // 기타 퀘스트
        {
            foreach (Npc npc in Character.Player.npcList)
            {
                if (npc.index == quest.npc_Index)
                {
                    QuestCompletenum(npc, quest.Index, 1);
                    npc.QuestMarkerNum = 1;
                }
            }


        }
        
        MiniQuestSlot tmp = Instantiate(QuestSlot);
        tmp.quest = quest;
        tmp.TextingQuestSlot();
        tmp.transform.SetParent(Mqlist.content.transform); // 퀘스트 알람등록
        list.Add(tmp); // 퀘스트 슬롯 등록        
        Character.Player.Quest.Add(quest.Index, quest);  // 퀘스트 목록 등록   
        if (questlist_M.gameObject.activeSelf == false)
            UIManager.uimanager.TryOpenQuest();
        // questlist_M.AddQuestSlot(quest);
        UIManager.uimanager.TryOpenQuest();
        quest = null;

    }
    public void AddQuest(int _index,int completenum,int _Goal_C)
    {
        //data = QuestTableManager.instance.quest_Table.GetData(_index);
        //quest = new Quest(int.Parse(data[0]), int.Parse(data[1]), data[2], data[3], data[4], int.Parse(data[5]), int.Parse(data[6]));
        //quest.State = completenum; // 퀘스트 진행상태


        //if(completenum != 3)
        //{
            
        //    MiniQuestSlot tmp = Instantiate(QuestSlot);
        //    tmp.quest = quest;
        //    tmp.TextingQuestSlot();
        //    tmp.transform.SetParent(Mqlist.content.transform); // 퀘스트 알람등록
        //    list.Add(tmp); // 퀘스트 슬롯 등록
        //    if (completenum == 2)
        //    {
        //        tmp.finishQuest();
        //    }
        //}       
                
        Character.Player.Quest.Add(quest.Index, quest);// 퀘스트 목록 등록
        if (questlist_M.gameObject.activeSelf == false)
            UIManager.uimanager.TryOpenQuest();
        // questlist_M.AddQuestSlot(quest);
        UIManager.uimanager.TryOpenQuest();        
        quest = null;

    }
    public void QuestCompletenum(Npc npc, int index, int completeNum)  //특정 npc의 퀘스트 완료를 변경할때
    {
        foreach(Quest quest in npc.quest_list)
        {
            if(quest.Index == index)
            {
                //quest.State = completeNum; 
            }
        }
    }
    public void QuestComplete_QuestList(int _index)
    {
        if (UIManager.uimanager.QuestList_M.activeSelf == false)
            UIManager.uimanager.TryOpenQuest();

        foreach(QuestSlot one in questlist_M.questlist)
        {
            //if(one.quest.Index == _index)
            //{
            //    one.quest.State = 3;
            //    one.DoneQuest();
            //}
        }

        UIManager.uimanager.TryOpenQuest();
    }
  

    public void questUpdate(QuestType questType,int _goal_index, int _num =1) // 퀘스트 업데이트(잡거나 얻었을때)
    {
        if(questType == QuestType.BATTLE)  // 전투 퀘스트
        {
            //quest = GetQuest(Quest.QuestType.Battle, _goal_index);
            quest.goal_Current += _num;
            if(quest.goal_Current >= quest.goal_Need)    // 완료되면
            {
                for (int i = 0; i < Character.Player.MobList.Count; i++) //퀘스트몹 풀기
                {
                    if (Character.Player.MobList[i].Index == quest.goal_Index)
                        Character.Player.MobList[i].isQuestMob = false;
                }
                if (FindNpc(quest.npc_Index) != null)
                {
                    QuestCompletenum(FindNpc(quest.npc_Index), quest.Index, 2); // 지정 Npc가 맵에있을때 퀘스트갱신
                    FindNpc(quest.npc_Index).QuestMarkerNum = 2;
                }
                    
                //quest.State = 2;
                IsSlot =FindMiniQuestSlot(quest.Index);
                IsSlot.quest = quest;
                IsSlot.finishQuest();
                if (questlist_M.gameObject.activeSelf == false)
                    UIManager.uimanager.TryOpenQuest();
                //FindQuestSLot(quest.Index).quest.State = 2;
                UIManager.uimanager.TryOpenQuest();

                quest = null;
                IsSlot = null;

                return;

            }
            IsSlot = FindMiniQuestSlot(quest.Index);
            IsSlot.quest = quest;
            IsSlot.UpdatePrograss();
            IsSlot = null;
            quest = null;

           

            return;

        }
        else if(questType == QuestType.COLLECT)
        {
            //quest = GetQuest(Quest.QuestType.Item,_goal_index);
            quest.goal_Current += _num;
            if (quest.goal_Current >= quest.goal_Need) // 완료되면
            {
                //quest.State = 2;
                IsSlot = FindMiniQuestSlot(quest.Index);
                IsSlot.quest = quest;
                IsSlot.finishQuest();
                quest = null;
                IsSlot = null;
                return;

            }
            IsSlot = FindMiniQuestSlot(quest.Index);
            IsSlot.quest = quest;
            IsSlot.UpdatePrograss();
            IsSlot = null;
            quest = null;
            return;            
        }  
       
        CMini.quest.goal_Current += _num;
        CMini.UpdatePrograss();
        CMini = null;

    }
    //public void applyQuest()  //맵이동 하였을때 갱신
    //{
    //    for(int i=0; i < Character.Player.myQuest.Count; i++)
    //    {
    //        if(Character.Player.myQuest[i].QuestComplete == 1)      // 퀘스트가 완료되지 않았을때
    //        {
    //            if(Character.Player.myQuest[i].questType == Quest.QuestType.Battle)
    //            {
    //                for (int j = 0; j < Character.Player.MobList.Count; j++)
    //                {
    //                    if (Character.Player.myQuest[i].goal_Index == Character.Player.MobList[j].Index)
    //                        Character.Player.MobList[j].isQuestMob = true;
    //                }
    //                if (FindNpc(Character.Player.myQuest[i].npc_Index) != null)
    //                {
    //                    FindNpc(Character.Player.myQuest[i].npc_Index).QuestMarkerNum = 1;
    //                    QuestCompletenum(FindNpc(Character.Player.myQuest[i].npc_Index), Character.Player.myQuest[i].Index, 1);
    //                }
    //            }

    //        }
    //        else if(Character.Player.myQuest[i].QuestComplete == 2) // 퀘스트가 완료되고, 퀘스트 보상은 안받았을때
    //        {

    //            if (FindNpc(Character.Player.myQuest[i].npc_Index) != null)
    //            {
    //                FindNpc(Character.Player.myQuest[i].npc_Index).QuestMarkerNum = 2;
    //                QuestCompletenum(FindNpc(Character.Player.myQuest[i].npc_Index), Character.Player.myQuest[i].Index, 2);
    //            }


    //        }
    //        else if (Character.Player.myQuest[i].QuestComplete == 3) // 퀘스트가 완료되고 보상도 받았을때
    //        {
    //            if (FindNpc(Character.Player.myQuest[i].npc_Index) != null)
    //            {
    //                FindNpc(Character.Player.myQuest[i].npc_Index).QuestMarkerNum = -1;
    //                QuestCompletenum(FindNpc(Character.Player.myQuest[i].npc_Index), Character.Player.myQuest[i].Index, 3);
    //            }
    //        }
    //    }
    //}
  

    public void Quest_Reset() // 전부 삭제
    {
        MiniQuestSlot tmp;
        for(int i = 0; i < list.Count; i++)
        {
            tmp = list[i];
            list.Remove(tmp);
            tmp.DoneQuest();           
            
        }
     
        if (questlist_M.gameObject.activeSelf == false)
            questlist_M.gameObject.SetActive(true);
        //questlist_M.Questlist_Reset();
        questlist_M.gameObject.SetActive(false);
            
    }
    public Npc FindNpc(int _index)
    {
        foreach(Npc one in Character.Player.npcList)
        {
            if (one.index == _index)
                return one;
        }
        return null;
    }
    
    public MiniQuestSlot FindMiniQuestSlot(int _num)
    {
        foreach(MiniQuestSlot one in list)
        {
            if (one.quest.Index.Equals(_num))
            {
                return one;
                
            }
        }
        return null;
        
    }
    public QuestSlot FindQuestSLot(int _num)
    {
        //foreach(QuestSlot one in questlist_M.questlist)
        //{
        //    if (one.quest.Index.Equals(_num))
        //    {
        //        return one;
        //    }
        //}
        return null;
    }
    public void DestroyQuestSLot(int _num)
    {
        //Character.Player.QuestUpdate(_num, 3);
        MiniQuestSlot tmp = FindMiniQuestSlot(_num);
        list.Remove(tmp);
        tmp.DoneQuest();


    }
    //public Quest GetQuest(Quest.QuestType _questType,int _goalindex)  // 목표인덱스로 찾기
    //{
    //    for(int i=0; i < Character.Player.myQuest.Count; i++)
    //    {
    //        if (Character.Player.myQuest[i].questType==_questType&& Character.Player.myQuest[i].goal_Index == _goalindex)
    //            return Character.Player.myQuest[i];
    //    }


    //    return null;
    //}
    
    
}
