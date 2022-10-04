using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Ui_QuestMain : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IDragHandler
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
    // 퀘스트 상태
    QUESTSTATE mainState;
    [SerializeField]
    QuestSlot questslot;

    PoolData<QuestSlot> questSlotPool;
    Dictionary<int, QuestSlot> runningSlotDic;    

    [SerializeField]
    MoveWindow moveWindow;
   
    bool WindowDrag = false;
    Vector2 Window_Preset = Vector2.zero;
    public RectTransform Window;
    Character character;
    private void Start()
    {
        mainState = QUESTSTATE.PLAYING;        
        questSlotPool = new PoolData<QuestSlot>(questslot, this.gameObject, "QuestSlot");
        runningSlotDic = new Dictionary<int, QuestSlot>();
        UIManager.uimanager.AOpenQuestMain += () => gameObject.SetActive(true);
        UIManager.uimanager.ACloseQuestMain += () => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        character = GameManager.gameManager.character;
        UpdateQuestInfo();
    }
    private void OnDisable()
    {
        ResetQuestSlots();
        ClearInfo();
    }
    public void PlayingButton()
    {
        mainState = QUESTSTATE.PLAYING;
        ClearInfo();
        UpdateQuestInfo();
    }
    public void DoneButton()
    {
        mainState = QUESTSTATE.DONE;
        ClearInfo();
        UpdateQuestInfo();
    }

    void AddQuestSlot(Quest _quest)
    {
        QuestSlot newSlot = questSlotPool.GetData();
        if(_quest.State == QUESTSTATE.DONE)
        {
            newSlot.DoneQuest();
        }
        newSlot.QuestWrite(_quest.questName);
        runningSlotDic.Add(_quest.Index, newSlot);
        newSlot.transform.SetParent(questScroll.content.transform);
    }

    void RemoveQuestSlot(int _index)
    {
        if (runningSlotDic.ContainsKey(_index))
        {
            QuestSlot removeSlot = runningSlotDic[_index];
            runningSlotDic.Remove(_index);
            removeSlot.Clear();
            questSlotPool.Add(removeSlot);
        }
    }
    void ResetQuestSlots()
    {
        List<int> questSlotIndex = new List<int>(runningSlotDic.Keys);
        if (questSlotIndex == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < questSlotIndex.Count; i++)
            {
                RemoveQuestSlot(questSlotIndex[i]);
            }
        }
        
    }
    void UpdateQuestInfo()
    {
        if(character == null || runningSlotDic == null)
        {
            return;
        }
        ResetQuestSlots();

        List<Quest> questList = character.GetQuestList(mainState);
        for (int i = 0; i < questList.Count; i++)
        {   
            AddQuestSlot(questList[i]);
        }
    }

   
    public void OnPointerDown(PointerEventData data)
    {
        if (moveWindow.isInRect(data.position))
        {
            WindowDrag = true;
            Window_Preset = data.position - (Vector2)Window.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            List<int> runninSlots = new List<int>(runningSlotDic.Keys);
            for (int i = 0; i < runninSlots.Count; i++)
            {
                if (runningSlotDic[runninSlots[i]].isInRect(data.position))
                {
                    QuestInfoUpdate(runninSlots[i]);
                }                
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
        Quest quest = new Quest(_Index, GameManager.gameManager.character.quest.ChracterState_Quest(_Index));
        if(quest == null)
        {
            Debug.LogError("퀘스트를 찾을수 없습니다.");
            return;
        }
        switch (quest.State)
        {
            case QUESTSTATE.DONE:
                Name.text = quest.questName;
                Name.color = Color.gray;
                Type.text = quest.Type.ToString();
                Type.color = Color.gray;
                Explain.text = quest.Explain;
                Explain.color = Color.gray;
                State.text = "완료";
                State.color = Color.gray;
                quest = null;
                return;
            case QUESTSTATE.PLAYING:
                Name.color = Color.white;
                Type.color = Color.white;
                Explain.color = Color.white;
                State.color = Color.white;
                Name.text = quest.questName;
                Type.text = quest.Type.ToString();
                Explain.text = quest.Explain;
                State.text = "진행중";
                quest = null;
                return;
            case QUESTSTATE.COMPLETE:
                Name.color = Color.white;
                Type.color = Color.white;
                Explain.color = Color.white;
                State.color = Color.white;
                Name.text = quest.questName;
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
