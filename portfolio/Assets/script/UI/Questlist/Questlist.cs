using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Questlist : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Type;
    public TextMeshProUGUI Explain;
    public TextMeshProUGUI Complete;
    public ScrollRect questScroll;


    public List<QuestSlot> questlist = new List<QuestSlot>();
    public QuestSlot questslot;


    public MoveWindow moveWindow;
    bool WindowDrag = false;
    Vector2 Window_Preset = Vector2.zero;
    public RectTransform Window;

    public void OnPointerDown(PointerEventData data)
    {
        if (moveWindow.isInRect(data.position))
        {
            WindowDrag = true;
            Window_Preset = data.position - (Vector2)Window.position;
        }




        for (int i = 0; i < questlist.Count; i++)
        {
            if (questlist[i].isInRect(data.position))
            {
                ShowQuest(questlist[i].quest);
            }



        }
    }
    public void emptyinfo()
    {
        Name.text = "";
        Type.text = "";
        Explain.text = "";
        Complete.text = "";
    }
    public void ShowQuest(Quest _quest)
    {
        if(_quest.QuestComplete == 3)
        {

            Name.text = _quest.questName;
            Name.color = Color.gray;
            Type.text = _quest.questType.ToString();
            Type.color = Color.gray;
            Explain.text = _quest.questExplain;
            Explain.color = Color.gray;
            Complete.text = "완료";
            Complete.color = Color.gray;
            return;
        }
        Name.color = Color.white;
        Type.color = Color.white;
        Explain.color = Color.white;
        Complete.color = Color.white;
        Name.text = _quest.questName;
        Type.text = _quest.questType.ToString();
        Explain.text = _quest.questExplain;
        if(_quest.QuestComplete == 1)
        {
            Complete.text = "진행중";
        }
        else if(_quest.QuestComplete == 2)
        {
            Complete.text = "달성";
            
        }
        
    }


    public void AddQuest(Quest _quest)
    {
        QuestSlot tmp = Instantiate(questslot);
        tmp.quest = _quest;
        tmp.QuestWrite();
        tmp.transform.SetParent(questScroll.content.transform);
        questlist.Add(tmp);

        if(_quest.QuestComplete == 3)
        {
            tmp.DoneQuest();
        }
    }
    public void Questlist_Reset()
    {

        QuestSlot tmp;
        for(int i = 0; i < questlist.Count; i++)
        {
            tmp = questlist[i];
            questlist.Remove(questlist[i]);
            Destroy(tmp.gameObject);
        }
      

    }
    public void OnPointerUp(PointerEventData data)
    {
        WindowDrag = false;
    }

    public void OnDrag(PointerEventData data)
    {
        if (WindowDrag == true)
        {
            Window.position = data.position - Window_Preset;
        }

    }
}
