
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickQuestUi : MonoBehaviour
{
    [SerializeField]
    MiniQuestSlot exampleSlot;
    [SerializeField]
    GameObject parent;
    [SerializeField]
    ScrollRect miniQuestScrollRect;

    PoolData<MiniQuestSlot> miniQuestPool;    

    Dictionary<int, MiniQuestSlot> runningMiniQuestSlots;
    

    Character character;

    private void Start()
    {
        runningMiniQuestSlots   = new Dictionary<int, MiniQuestSlot>();        
        miniQuestPool        = new PoolData<MiniQuestSlot>(exampleSlot, parent, "MiniQuestSlots");
        
    }
    private void OnEnable()
    {
        character = GameManager.gameManager.character;
        UIManager.uimanager.AUpdateQuickQuestUi += UpdateQuickQuest;
        UIManager.uimanager.AAddQuickQuestUi += AddQuest;
        UpdateAllQuest();
    }
    private void OnDisable()
    {
        UIManager.uimanager.AUpdateQuickQuestUi -= UpdateQuickQuest;
        UIManager.uimanager.AAddQuickQuestUi -= AddQuest;
        ResetSlots();
        
    }
    void ResetSlots()
    {
        List<int> runningList = new List<int>(runningMiniQuestSlots.Keys);
        for (int i = 0; i < runningList.Count; i++)
        {
            miniQuestPool.Add(runningMiniQuestSlots[runningList[i]]);            
        }
        runningMiniQuestSlots.Clear();        
    }

    void AddQuest(Quest _quest)
    {   
        MiniQuestSlot slot = miniQuestPool.GetData();
        slot.TextingQuestSlot(_quest);
        runningMiniQuestSlots.Add(_quest.Index, slot);
        slot.transform.SetParent(miniQuestScrollRect.content.transform);

        if (_quest.State == QUESTSTATE.COMPLETE)
        {
            slot.transform.SetAsFirstSibling();
        }
    }
    void RemoveQuest(int _index)
    {
        if (runningMiniQuestSlots.ContainsKey(_index))
        {
            MiniQuestSlot slot = runningMiniQuestSlots[_index];
            miniQuestPool.Add(slot);
            runningMiniQuestSlots.Remove(_index);
        }
    }
    public void OpenQuickQuest()
    {
        character = GameManager.gameManager.character;
        UIManager.uimanager.AUpdateQuickQuestUi += UpdateQuickQuest;
        UIManager.uimanager.AAddQuickQuestUi += AddQuest;
        miniQuestScrollRect.gameObject.SetActive(true);
        UpdateAllQuest();
    }

    void UpdateQuickQuest(Quest _quest)
    {
        MiniQuestSlot slot = runningMiniQuestSlots[_quest.Index];

        switch (_quest.State)
        {
            case QUESTSTATE.PLAYING:
                slot.UpdatePrograss(_quest);
                slot.transform.SetAsFirstSibling();
                break;
            case QUESTSTATE.COMPLETE:
                slot.UpdatePrograss(_quest);
                slot.transform.SetAsFirstSibling();
                break;
            case QUESTSTATE.DONE:
                RemoveQuest(_quest.Index);
                break;
        }

    }
    void UpdateAllQuest()
    {   
        if(character == null)
        {
            return;
        }
        List<Quest> questList = character.quest.GetQuickSlots();
        for (int i = 0; i < questList.Count; i++)
        {
            AddQuest(questList[i]);
        }
    }
    public void CloseQuickQuest()
    {
        UIManager.uimanager.AUpdateQuickQuestUi -= UpdateQuickQuest;
        UIManager.uimanager.AAddQuickQuestUi -= AddQuest;
        ResetSlots();
        miniQuestScrollRect.gameObject.SetActive(false);
    }
}
