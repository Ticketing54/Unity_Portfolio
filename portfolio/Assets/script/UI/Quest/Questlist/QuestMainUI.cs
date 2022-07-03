using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class QuestMainUI : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IDragHandler
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
    List<QuestSlot> runningSlotList;

    [SerializeField]
    MoveWindow moveWindow;
    bool WindowDrag = false;
    Vector2 Window_Preset = Vector2.zero;
    public RectTransform Window;
    Character character;
    private void Awake()
    {
        mainState = QUESTSTATE.PLAYING;
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.L, TryOpenQuest);
        questSlotPool = new PoolData<QuestSlot>(questslot, this.gameObject, "QuestSlot");
        runningSlotDic = new Dictionary<int, QuestSlot>();
        runningSlotList = new List<QuestSlot>();
    }
    private void OnEnable()
    {
        character = GameManager.gameManager.character;
        UpdateQuestInfo();
    }
    private void OnDisable()
    {
        RunningReset();
        ClearInfo();
    }

    void UpdateQuestInfo()
    {
        List<Quest> questList = character.GetQuestList(mainState);
        for (int i = 0; i < questList.Count; i++)
        {
            QuestSlot newSlot = questSlotPool.GetData();
            Quest quest = questList[i];
            newSlot.QuestWrite(quest.Name);
            newSlot.transform.SetParent(questScroll.content.transform);
            runningSlotDic.Add(quest.Index, newSlot);
            runningSlotList.Add(newSlot);
        }
    }

    void RunningReset()
    {
        List<int> runningKeys = new List<int>(runningSlotDic.Keys);

        foreach(int key in runningKeys)
        {
            QuestSlot runningSlot = runningSlotDic[key];
            runningSlotDic.Remove(key);
            runningSlot.Clear();
            questSlotPool.Add(runningSlot);
        }

        runningSlotList.Clear();
    }

    #region KeyboardShortCut
    bool questActive = false;
    void TryOpenQuest()
    {
        questActive = !questActive;
        if (questActive)
        {
            OpenQuest();
        }
        else
            CloseQuest();
    }
    void OpenQuest()
    {
        gameObject.SetActive(true);        
    }
    void CloseQuest()
    {   
        gameObject.SetActive(false);
    }
    #endregion

    public void OnPointerDown(PointerEventData data)
    {
        if (moveWindow.isInRect(data.position))
        {
            WindowDrag = true;
            Window_Preset = data.position - (Vector2)Window.position;
        }

        for (int i = 0; i < runningSlotList.Count; i++)
        {
            if (runningSlotList[i].isInRect(data.position))
            {
                QuestInfoUpdate(runningSlotList[i].QuestIndex);
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
            case QUESTSTATE.PLAYING:
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
            case QUESTSTATE.COMPLETE:
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
