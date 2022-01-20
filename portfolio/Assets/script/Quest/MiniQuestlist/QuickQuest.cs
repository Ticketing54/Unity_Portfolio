using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickQuest : MonoBehaviour
{ 
    public MiniQuestSlot QuestSlot;
    public List<MiniQuestSlot> list = new List<MiniQuestSlot>();
    public ScrollRect Mqlist;
    
        

    public void AddQuest(Quest _quest)
    {
        if (list.Count >= 4)
        {
            Debug.Log("더이상 추가 불가능");
            return;
        }
            
        MiniQuestSlot tmp = Instantiate(QuestSlot);
        tmp.quest = _quest;
        tmp.TextingQuestSlot();
        tmp.transform.SetParent(Mqlist.content.transform); // 퀘스트 알람등록
        list.Add(tmp); // 퀘스트 슬롯 등록               
    }
    public void ClearQuest()
    {
        list.Clear();
    }
    public MiniQuestSlot IsInQuest(int _index)
    {
        foreach(MiniQuestSlot one in list)
        {
            if (one.quest.Index == _index)
                return one;
        }

        return null;
    }
    public void RemoveQuest(int _index)
    {
        foreach(MiniQuestSlot one in list)
        {
            if(one.quest.Index == _index)
            {
                one.DoneQuest();
            }
        }
    }
    public void UpdateQuest(int _index)
    {
        foreach (MiniQuestSlot one in list)
        {
            if (one.quest.Index == _index)
            {
                one.UpdatePrograss();
            }
        }
    }
    public void CompleteQuest(int _index)
    {
        foreach (MiniQuestSlot one in list)
        {
            if (one.quest.Index == _index)
            {
                one.finishQuest();
            }
        }
    }

}
