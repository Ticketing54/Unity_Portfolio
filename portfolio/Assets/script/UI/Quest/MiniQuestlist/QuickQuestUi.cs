
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
    List<MiniQuestSlot> runningSlotsList;

    Character character;

    private void Start()
    {
        runningMiniQuestSlots   = new Dictionary<int, MiniQuestSlot>();
        runningSlotsList        = new List<MiniQuestSlot>();
        miniQuestPool        = new PoolData<MiniQuestSlot>(exampleSlot, parent, "MiniQuestSlots");
        UIManager.uimanager.OpenQuickQuest  += OpenQuickQuest;
        UIManager.uimanager.CloseQuickQuest += CloseQuickQuest;
    }
    private void OnDisable()
    {
        ResetSlots();   
    }
    void ResetSlots()
    {
        for (int i = 0; i < runningSlotsList.Count; i++)
        {
            miniQuestPool.Add(runningSlotsList[i]);            
        }
        runningMiniQuestSlots.Clear();
    }

    void AddQuest(Quest _quest)
    {
        if(runningSlotsList.Count >= 4 )
        {
            return;
        }

        MiniQuestSlot slot = miniQuestPool.GetData();
        slot.TextingQuestSlot(_quest);
        runningMiniQuestSlots.Add(_quest.Index, slot);
        runningSlotsList.Add(slot);
        slot.transform.SetParent(miniQuestScrollRect.content.transform);
    }

    void CompleteQuest(int _quest)
    {

    }
    
    void UpdateQuest(int _index)
    {

    }
    void DoneQuest(int _index)
    {

    }
   
   
   
    public void OpenQuickQuest()
    {
        character = GameManager.gameManager.character;
        miniQuestScrollRect.gameObject.SetActive(true);
        UpdateQuickQuest();

    }

    void UpdateQuickQuest()
    {
        

    }
    public void CloseQuickQuest()
    {
        miniQuestScrollRect.gameObject.SetActive(false);
    }
}
