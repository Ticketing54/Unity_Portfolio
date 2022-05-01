
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickQuest : MonoBehaviour
{     
    public LinkedList<MiniQuestSlot> list = new LinkedList<MiniQuestSlot>();
    public ScrollRect Mqlist;
    int Count = 0;    
        

    public void AddQuest(Quest _quest)
    {
        foreach(MiniQuestSlot one in list)
        {
            if(one.gameObject.activeSelf== false)
            {
                one.gameObject.SetActive(true);
                one.quest = _quest;
                one.TextingQuestSlot();
            }
        }                 
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
            if(one.gameObject.activeSelf == true && one.quest.Index == _index)
            {                
                list.Remove(one);
                list.AddLast(one);
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
