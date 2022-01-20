using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Questlist : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    [SerializeField]
    TextMeshProUGUI Name;
    [SerializeField]
    TextMeshProUGUI Type;
    [SerializeField]
    TextMeshProUGUI Explain;
    [SerializeField]
    TextMeshProUGUI State;
    [SerializeField]
    ScrollRect questScroll;
    [SerializeField]
    HaveQuestState mainState;

    Dictionary<int, Quest> Quest = new Dictionary<int, Quest>();
    Quest quest;
    Queue<QuestSlot> UsableSlot = new Queue<QuestSlot>();
    QuestSlot Slot;

    public List<QuestSlot> questlist = new List<QuestSlot>();           // 큐형식으로 구현해야하는데 큐를 사용하기 애매함
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

        if (IsEmpty())
            return;
        
        for (int i = 0; i < questlist.Count; i++)
        {
            if (questlist[i].isInRect(data.position))
            {
                QuestInfoUpdate(questlist[i].QuestIndex);
            }
        }
    }
    public void ClearInfo()
    {
        Name.text = "";
        Type.text = "";
        Explain.text = "";
        State.text = "";
    }
    public void QuestInfoUpdate(int _Index)
    {
        quest = Character.Player.quest.GetQuest(_Index);
        if(quest == null)
        {
            Debug.LogError("퀘스트를 찾을수 없습니다.");
            return;
        }
        switch (quest.State)
        {
            case QuestState.DONE:
                Name.text = quest.Name;
                Name.color = Color.gray;
                Type.text = quest.Type.ToString();
                Type.color = Color.gray;
                Explain.text = quest.Explain;
                Explain.color = Color.gray;
                State.text = "완료";
                State.color = Color.gray;
                quest = null;
                return;
            case QuestState.PLAYING:
                Name.color = Color.white;
                Type.color = Color.white;
                Explain.color = Color.white;
                State.color = Color.white;
                Name.text = quest.Name;
                Type.text = quest.Type.ToString();
                Explain.text = quest.Explain;
                State.text = "진행중";
                quest = null;
                return;
            case QuestState.COMPLETE:
                Name.color = Color.white;
                Type.color = Color.white;
                Explain.color = Color.white;
                State.color = Color.white;
                Name.text = quest.Name;
                Type.text = quest.Type.ToString();
                Explain.text = quest.Explain;
                State.text = "달성";
                quest = null;
                return;
            default:
                Debug.LogError("퀘스트를 찾았으나 상태를 알 수 없습니다.");
                quest = null;
                return;
        }       
    }
    


    public void UpdateQuestSlot()
    {
        CloseQuestSlot();
        List<int> List;
        QuestSlot NewQuestSlot;
        List = Character.Player.quest.GetQuestList(mainState);

        for (int i = 0; i < List.Count; i++)
        {
            NewQuestSlot = GetSlot(List[i]);
            NewQuestSlot.gameObject.SetActive(true);
            ///퀘스트 이름 확인할 것
            NewQuestSlot.QuestWrite("테스트입니다");
            NewQuestSlot.transform.SetParent(questScroll.content.transform);
            questlist.Add(NewQuestSlot);
        }        
    }
    public void CloseQuestSlot()
    {
        for (int i = 0; i < questlist.Count; i++)
        {
            InsertSlot(questlist[i]);            
        }
        questlist.Clear();
    }
    void InsertSlot(QuestSlot _UsedSlot)
    {
        UsableSlot.Enqueue(_UsedSlot);
        _UsedSlot.Clear();
        _UsedSlot.transform.SetParent(null);
        _UsedSlot.gameObject.SetActive(false);
    }
    bool IsEmpty()
    {
        return questlist.Count == 0;
    }
    QuestSlot GetSlot(int _QuestIndex)
    {
        QuestSlot NewSlot;

        if (UsableSlot.Count == 0)
        {
            NewSlot = Instantiate(questslot);
            NewSlot.QuestIndex = _QuestIndex;
            return NewSlot;           
        }
        else
        {
            NewSlot = UsableSlot.Dequeue();
            NewSlot.QuestIndex = _QuestIndex;
            return NewSlot;
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
